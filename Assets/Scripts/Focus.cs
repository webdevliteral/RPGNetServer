using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Focus : MonoBehaviour
{
    [SerializeField]
    public Transform target;

    public bool isFocused;

    public void SetFocus(Transform _target)
    {
        _target.parent.GetComponent<Focus>().isFocused = true;
        target = _target;
    }

    public void ClearFocus()
    {
        if (target != null)
        {
            target.parent.GetComponent<Focus>().isFocused = false;
            target = null;
        }
    }
}
