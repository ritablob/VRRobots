using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatTarget : MonoBehaviour
{
    void Update()
    {
        Vector3 fixedPos = new Vector3(Director.instance.playerCamera.position.x, transform.position.y, Director.instance.playerCamera.position.z);

        transform.LookAt(fixedPos);

        transform.localEulerAngles += new Vector3(0, 180, 0);
    }
}
