using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.AI;

public class CCDIK : MonoBehaviour {
    public Transform Tooltip;
    public Transform Target;
    public CCDIKJoint[] joints;
    public Vector3 StartPos;
    public float distanceThreshold = 0.1f, timer = 0;
    public int iterations = 10;
    public AnimationCurve curve, scaleCurve;
    public Transform[] garbageCans;

    [SerializeField] private Transform Storage;
    private List<GameObject> storedObjects;

    private float maxRange;

    private void Start()
    {
        storedObjects = new List<GameObject>(0);
        maxRange = 0;

        Director.instance.releaseObject += SetTarget;
        Director.instance.copyObject += CreateCopy;

        // Calculate max distnace
        for (int i = 1; i < joints.Length - 1; i++) {
            maxRange += Vector3.Distance(joints[i].transform.position, joints[i + 1].transform.position);
        }

        maxRange *= 0.85f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(joints[0].transform.position, maxRange);
    }

    void Update() {
        if (Target == null) { timer = 0; return; }

        Vector3 targetPos = Target.position;

        // If out of range, move to the closest position within range
        if (Vector3.Distance(joints[0].transform.position, Target.position) > maxRange) {
            // Get direction of vector
            Vector3 dir = Target.position - joints[0].transform.position;

            // Normalize, and set the position to the edge of the boundry
            targetPos = joints[0].transform.position + (dir.normalized * (maxRange * 0.9f));
        }

        // Catching the obejct!
        if (StartPos == Vector3.zero && Vector3.Distance(Target.position, Tooltip.position) < distanceThreshold) {
            Director.instance.GrabObject(true);
            Rigidbody rb = Target.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.angularVelocity = Vector3.zero;
        }

        if (StartPos != Vector3.zero) {
            targetPos = Vector3.Lerp(StartPos, Storage.position, curve.Evaluate(timer));
            Target.position = targetPos;
            Target.localScale = Vector3.Lerp(Target.GetComponent<Custom_Interactable>().startScale, Vector3.zero, scaleCurve.Evaluate(timer));
            timer += Time.deltaTime * 0.5f;
        }

        for (int i = 0; i < iterations; i++) {
            for (int j = 0; j < joints.Length; j++) {
                joints[j].Evaluate(Tooltip, Target, targetPos, j < 2);
            }
        }
    }

    private void SetTarget(Transform _target) {
        Target = _target;
    }

    private void CreateCopy(GameObject origin) {
        // Find where in the list to insert the element (if the type is present in the list, insert it before the first occurance.
        // If not in the list, just add it to the end
        GarbageType type = origin.GetComponent<Garbage_Bit>().type;

        // Return the index of the first matching garbage type (if not present, returns -1)
        int index = GarbagePresentInList(type);
        GameObject copy = Instantiate(origin, new Vector3(7 * storedObjects.Count, 1000, 7 * storedObjects.Count), Quaternion.identity);

        if (index > -1) {
            storedObjects.Insert(index, copy);
        }
        else {
            storedObjects.Add(copy);
        }

        copy.AddComponent<StartSize>().startSize = copy.GetComponent<Custom_Interactable>().startScale;
        Destroy(copy.GetComponent<Custom_Interactable>());
        Destroy(copy.GetComponent<XRGrabInteractable>());

        Rigidbody rb = copy.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        Destroy(origin);
    }

    public void DumpObjects() {
        StartCoroutine(DumpObjectsCoroutine());
    }

    private IEnumerator DumpObjectsCoroutine() {
        GarbageType lastType = GarbageType.BAD;
        Transform garbageTarget = GarbagecanMoveTo(storedObjects[0].GetComponent<Garbage_Bit>().type);
        GetComponent<NavMeshAgent>().SetDestination(garbageTarget.position);

        for (int i = 0; i < storedObjects.Count; i++) {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();

            // If there is a type mistmatch, move to the correct garbage can, then wait
            if (storedObjects[i].GetComponent<Garbage_Bit>().type != lastType) {
                Transform _garbageTarget = GarbagecanMoveTo(storedObjects[i].GetComponent<Garbage_Bit>().type);
                agent.SetDestination(_garbageTarget.position);
                GetComponent<Animator>().SetTrigger("Close");

                while (Vector3.Distance(agent.destination, transform.position) > 0.1f) {
                    transform.LookAt(_garbageTarget);
                    yield return null;
                }

                GetComponent<Animator>().SetTrigger("Open");
                yield return new WaitForSeconds(0.33f);
                transform.LookAt(_garbageTarget);
            }

            yield return new WaitForSeconds(0.25f);

            storedObjects[i].transform.position = Storage.position;
            Rigidbody rb = storedObjects[i].GetComponent<Rigidbody>();
            storedObjects[i].GetComponent<StartSize>().ScaleUP();
            storedObjects[i].transform.position = Storage.position;
            rb.useGravity = true;
            rb.AddForce(transform.forward * 4, ForceMode.Impulse);
            lastType = storedObjects[i].GetComponent<Garbage_Bit>().type;
        }

        storedObjects.Clear();

        GetComponent<Animator>().SetTrigger("Close");
    }

    private int GarbagePresentInList(GarbageType type) {
        for (int i = 0; i < storedObjects.Count; i++) { 
            if (storedObjects[i].GetComponent<Garbage_Bit>().type == type) {
                return i;
            }
        }

        return -1;
    }

    private Transform GarbagecanMoveTo(GarbageType type) {
       switch (type) {
            case GarbageType.General: return garbageCans[0];
            case GarbageType.Paper: return garbageCans[1];
            case GarbageType.Plastic: return garbageCans[2];
            default: return null;
       }
    }

    public float Timer => timer;
}