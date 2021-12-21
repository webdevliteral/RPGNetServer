using System;
using System.Collections.Generic;

/// <summary>
/// A list that can store <see cref="NetworkComponent"/> elements with O(1) access.
/// Not currently thread-safe.
/// </summary>
public class NetworkList<T> where T : NetworkComponent
{
    private List<T> _list;
    private Dictionary<uint, int> _networkIdToIndex;

    public int Count => _list.Count;

    public NetworkList()
    {
        _list = new List<T>();
        _networkIdToIndex = new Dictionary<uint, int>();
    }

    /// <summary>
    /// Adds a <see cref="NetworkComponent"/> element to the list.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void Add(T element)
    {
        if (element == null)
        {
            throw new ArgumentNullException("element");
        }

        // Store a map from the network id to the index, so that we can uniquely access this element in O(1) time.
        _networkIdToIndex[element.NetworkId] = _list.Count;

        _list.Add(element);
    }

    /// <summary>
    /// Remove a <see cref="NetworkComponent"/> element from the list.
    /// Unlike a List, this method has O(1) complexity.
    /// </summary>
    /// <returns>True if the element was removed, false if the element isn't present in the list.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool Remove(T element)
    {
        if (element == null)
        {
            throw new ArgumentNullException("element");
        }

        return RemoveByNetworkId(element.NetworkId);
    }

    /// <summary>
    /// Remove a <see cref="NetworkComponent"/> element from the list that matches the given network id.
    /// Unlike a List, this method has O(1) complexity.
    /// </summary>
    /// <returns>True if the element was removed, false if the element isn't present in the list.</returns>
    public bool RemoveByNetworkId(uint networkId)
    {
        if (!CheckRemoveErrorConditions(networkId))
        {
            return false;
        }

        var index = _networkIdToIndex[networkId];

        // Get a reference to the last element so that we can swap it with the current element.
        var last = _list[_list.Count - 1];

        // Update the network id map to move the last element's index to the current element's index.
        _networkIdToIndex[last.NetworkId] = index;

        // Remove the current network id-index mapping.
        _networkIdToIndex.Remove(networkId);

        // Do the swap.
        _list[_list.Count - 1] = _list[index];
        _list[index] = last;

        // Remove the element at the end, which is now the "current" element after swapping.
        _list.RemoveAt(_list.Count - 1);
        return true;
    }

    /// <summary>
    /// Returns the <see cref="NetworkComponent"/> element with the given network id.
    /// This method has O(1) complexity.
    /// </summary>
    /// <returns>The element with the given network id, or null if an element with the given network id doesn't exist in the list.</returns>
    public T GetByNetworkId(uint networkId)
    {
        if (_networkIdToIndex.TryGetValue(networkId, out int index))
        {
            return _list[index];
        }

        return null;
    }

    private bool CheckRemoveErrorConditions(uint networkId)
    {
        if (networkId == 0)
        {
            return false;
        }

        if (_list.Count == 0)
        {
            return false;
        }

        // Do a quick lookup to see if the element has an entry in the network id to index map - if it doesn't, it's not in the list.
        if (!_networkIdToIndex.TryGetValue(networkId, out int _))
        {
            return false;
        }

        return true;
    }
}
