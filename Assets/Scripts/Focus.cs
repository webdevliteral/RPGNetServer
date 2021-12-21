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

    public void SetFocus(Transform _target)
    {
        _target.parent.GetComponent<Interactable>().isFocused = true;
        target = _target;
        //Debug.Log($"Now focusing {target}");
    }

    public void ClearFocus()
    {
        if (target != null)
        {
            target.parent.GetComponent<Interactable>().isFocused = false;
            target = null;
        }

        //Debug.Log($"Now focusing {target}");
    }
}
