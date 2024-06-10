using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Director : MonoBehaviour
{
    public static Director instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action<string> grabItem;
    public void GrabItem(string _itemName) { if (grabItem != null) { grabItem.Invoke(_itemName); } }
}
