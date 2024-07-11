using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Custom_Interactable : MonoBehaviour
{
    public Vector3 startScale;

    [SerializeField] private float grabDistance = 0.33f;
    private Transform leftHand, rightHand;
    private Rigidbody rb;
    private Collider col;
    private float reGrabTimer = 1;

    private void Start() 
    {
        leftHand = Director.instance.leftHand;
        rightHand = Director.instance.rightHand;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        release += Release;
        grab += Grab;
        GetComponent<XRBaseInteractable>().selectExited.AddListener(release);
        GetComponent<XRBaseInteractable>().selectEntered.AddListener(grab);

        if (startScale == Vector3.zero) { startScale = transform.localScale; }
    }

    // If the player's hand is close to the object on the x,z plane, raise the object to be just below the hand on a y level
    private void Update()
    {
        if (reGrabTimer < 1) { reGrabTimer += Time.deltaTime; return; }

        Vector2 _leftHand = new Vector2(leftHand.position.x, leftHand.position.z);
        Vector2 _rightHand = new Vector2(rightHand.position.x, rightHand.position.z);

        if (Vector2.Distance(_leftHand, new Vector2(transform.position.x, transform.position.z)) < grabDistance) {
            if (!rb.isKinematic) { StartCoroutine(LerpUp(leftHand)); }
            rb.isKinematic = true;
        } else if (Vector2.Distance(_rightHand, new Vector2(transform.position.x, transform.position.z)) < grabDistance)  {
            if (!rb.isKinematic) { StartCoroutine(LerpUp(rightHand)); }
            rb.isKinematic = true;
        }
        else if (rb.isKinematic) {
            Release(null);
        }
    }

    private UnityAction<SelectExitEventArgs> release;
    private void Release(SelectExitEventArgs ctx)
    {
        col.enabled = true;
        StopAllCoroutines();
        reGrabTimer = 0;
        rb.isKinematic = false;
        rb.useGravity = true;

        if (ctx == null) { return; }

        Director.instance.ReleaseObject(transform);
    }

    private UnityAction<SelectEnterEventArgs> grab;
    private void Grab(SelectEnterEventArgs ctx) {
        col.enabled = false;
    }

    private IEnumerator LerpUp(Transform handToTrack) {
        float timer = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, handToTrack.position.y - 0.2f, transform.position.z);

        while (timer < 1) {
            timer += Time.deltaTime * 3;

            transform.position = Vector3.Lerp(startPos, endPos, Director.instance.interactLerpCurve.Evaluate(timer));

            yield return null;
        }
    }
}
