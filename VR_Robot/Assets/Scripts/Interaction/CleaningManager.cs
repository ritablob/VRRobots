using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

public class CleaningManager : MonoBehaviour
{
    /* check if grabbed
     * check if colliding
     * check if cleanable object
     * get cleaning agent
     * perform cleaning and track cleaning progress
     * 
     * 
     */
    // Start is called before the first frame update

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
