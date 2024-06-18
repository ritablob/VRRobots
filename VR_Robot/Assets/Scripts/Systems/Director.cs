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
}
