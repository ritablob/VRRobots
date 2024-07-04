using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Task_Interactable : MonoBehaviour
{
    [HideInInspector] public bool grabbed;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetAsCurrentLookat()
    {
        if (!grabbed) { Director.instance.SetCurrentLookat(transform); rb.constraints = RigidbodyConstraints.FreezeRotation; }
        else { Director.instance.ReleaseObject(transform); rb.constraints = RigidbodyConstraints.None; }

        grabbed = !grabbed;
    }
}
