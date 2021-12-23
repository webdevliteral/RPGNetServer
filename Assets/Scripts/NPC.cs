using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(NetworkComponent))]
public class NPC : Entity
{
    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        return base.Interact(_fromCID, _comparePosition);
    }
}

