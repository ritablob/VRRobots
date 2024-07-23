using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

/// <summary>
/// Add this on cleaning interactables (objects that clean). 
/// </summary>
public class CleaningManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<Cleanable>()) return;
        Cleanable cleanable = collision.gameObject.GetComponent<Cleanable>();
        cleanable.StartCoroutine(cleanable.CleanDirt(1f));
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.GetComponent<Cleanable>()) return;
        Cleanable cleanable = collision.gameObject.GetComponent<Cleanable>();
        cleanable.StopAllCoroutines();
        Debug.Log("Not cleaning anymore");
    }

    // Update is called once per frame
    void Update()
    {
    }
}