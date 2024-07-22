using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCanvasFacePlayer : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 1f;

    private Quaternion lookRotation;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //find the vector pointing from our position to the target
        direction = (player.transform.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        lookRotation = Quaternion.LookRotation(direction);
        lookRotation.eulerAngles = new Vector3(0, lookRotation.eulerAngles.y + 180, 0);
        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}