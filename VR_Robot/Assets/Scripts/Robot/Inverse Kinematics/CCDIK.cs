using UnityEngine;

public class CCDIK : MonoBehaviour {
    public Transform Tooltip;
    public Transform Target;
    public CCDIKJoint[] joints;
    public float distanceThreshold = 0.1f;
    public int iterations = 10;

    private float maxRange;

    private void Start()
    {
        maxRange = 0;

        // Calculate max distnace
        for (int i = 1; i < joints.Length - 1; i++) {
            maxRange += Vector3.Distance(joints[i].transform.position, joints[i + 1].transform.position);
        }

        maxRange *= 0.85f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(joints[1].transform.position, maxRange);
    }

    void Update() {
        if (Target == null) { return; }

        // If out of range, move to the closest position within range
        if (Vector3.Distance(joints[0].transform.position, Target.position) > maxRange) {
            return; 
        }

        // If close enough, increase the lerp amount
        if (Vector3.Distance(Target.position, Tooltip.position) < distanceThreshold) { 
            
            Destroy(Target.gameObject);
            GetComponent<Animator>().SetTrigger("Open");
        }

        for (int i = 0; i < iterations; i++) {
            for (int j = 0; j < joints.Length; j++) {
                joints[j].Evaluate(Tooltip, Target, j < 2);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "apple") {
            Target = other.transform;
        }
    }
}