using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPat : MonoBehaviour
{
    public List<Collider> hands;

    private void Update()
    {
        foreach(Collider hand in hands) { 
            if (Vector3.Distance(hand.transform.position, transform.position) < 0.3f) {
                hand.enabled = true;
            }
            else {
                hand.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hand") { 
            Director.instance.robot.GetComponent<Robot_Interaction_State_Machine>().Headpat(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "hand")
        {
            Director.instance.robot.GetComponent<Robot_Interaction_State_Machine>().Headpat(false);
        }
    }
}
