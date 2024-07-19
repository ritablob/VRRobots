using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;
    public float openingDuration = 1f;
    public Transform openPosition;
    public bool isOpen;
    private Vector3 OGposition;
    
    public void Open()
    {
        Debug.Log("Opening door");
        openPosition.localPosition = new Vector3(openPosition.localPosition.x, door.localPosition.y, openPosition.localPosition.z);
        OGposition = door.localPosition;
        StartCoroutine(OpenDoor());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter with "+other + " of "+other.gameObject);
        if (!isOpen)
            Open();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("TriggerExit with "+other + " of "+other.gameObject);
        // closes door with delay
        Invoke(nameof(Close), 1f);
    }

    public void Close()
    {
        StartCoroutine(CloseDoor());
    }

    private IEnumerator OpenDoor()
    {
        isOpen = true;
        float currentTime = 0;
        yield return new WaitForSeconds(1);
        while (currentTime < openingDuration)
        {
            door.localPosition = Vector3.Lerp(OGposition, openPosition.localPosition, currentTime/openingDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }


        yield return null;
    }

    private IEnumerator CloseDoor()
    {
        float currentTime = 0;
        yield return new WaitForSeconds(1);
        while (currentTime < openingDuration)
        {
            door.localPosition = Vector3.Lerp(openPosition.localPosition, OGposition, currentTime/openingDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        isOpen = false;
        yield return null;
    }
    
}
