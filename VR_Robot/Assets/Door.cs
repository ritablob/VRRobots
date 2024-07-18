using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;
    public float openingDuration = 1f;
    public Transform openPosition;
    private Vector3 OGposition;
    
    public void Open()
    {
        openPosition.position = new Vector3(openPosition.position.x, door.position.y, openPosition.position.z);
        OGposition = door.position;
        StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        float currentTime = 0;
        yield return new WaitForSeconds(1);
        while (currentTime < openingDuration)
        {
            door.position = Vector3.Lerp(OGposition, openPosition.position, currentTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
