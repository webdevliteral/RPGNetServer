using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NPC : Interactable
{
    public string entityName;
    public float gravity = -9.18f;
    public float moveSpeed = 5f;

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id)
    {
        id = _id;
    }
}

