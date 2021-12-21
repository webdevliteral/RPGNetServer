using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class LocalItemDB : MonoBehaviour
{
    public static LocalItemDB instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public List<Item> consumables = new List<Item>();
    public List<RegularEquipment> equipment = new List<RegularEquipment>();
}


