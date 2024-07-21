using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorSwitch : MonoBehaviour
{
    public UnityEvent onPressSwitch;
    private void OnTriggerEnter(Collider other)
    {
        onPressSwitch.Invoke();
    }
}
