using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class HandWaveController : MonoBehaviour
    {
        // TODO: add rotation to the equation
        
        public event Action handWaved;
        public GameObject RightHandAnchor;
        private Vector3 lastPosition;
        public TextMeshProUGUI text;
        public float responseTime = 1.0f;
        private bool coroutineRunning = false;
        public float movementThreshold = 1.2f;

        private void Start()
        {
            handWaved += CheckVelocity;
        }

        private void FixedUpdate()
        {   
            float distance = Vector3.Distance(lastPosition, RightHandAnchor.transform.position) / Time.deltaTime;
            //Debug.LogWarning("distance "+distance);
            if (distance > 1 && !coroutineRunning)
            {
                handWaved?.Invoke();
            }
            else if (!coroutineRunning)
            {
                text.color = Color.red;
                text.text = "Speed: " +$"{distance:0.0}";
            }
            lastPosition = RightHandAnchor.transform.position;
        }
        /*
        * - rotation is facing up
         * - velocity of hand has an increase
        * How to do it:
        * - Quaternion
         * - Dot product of normalized vectors
         * https://docs.unity3d.com/ScriptReference/Vector3.Angle.html
         * 
         * 
        */
        // Start is called before the first frame updat

        // Update is called once per frame
        private void Update()
        {
        
        }

        private void CheckRotation()
        {
            
        }

        private void CheckVelocity()
        {
            // track velocity every fixed frame for given time

            StartCoroutine(GetAverageVelocity());
        }

        private IEnumerator GetAverageVelocity()
        {
            coroutineRunning = true;
            //Debug.Log("Called coroutine!");
            float currentTime = 0f;
            float cumulativeVelocity = 0f;
            while (currentTime < responseTime){
                //Debug.Log("Tracking: "+currentTime);
                cumulativeVelocity +=
                    Vector3.Distance(lastPosition, RightHandAnchor.transform.position) / Time.deltaTime;
                lastPosition = RightHandAnchor.transform.position;
                // check velocity 
                currentTime += Time.deltaTime;
                yield return null;
            }     
            //Debug.Log("Done!");
            float velocity = cumulativeVelocity / currentTime*Time.deltaTime;
            currentTime = 0;
            if (velocity > movementThreshold)
            {
                text.color = Color.green;
                text.text = "Waving! Speed: " +$"{velocity:0.0}";
            }
            else
            {
                text.color = Color.red;
                text.text = "Speed: " +$"{velocity:0.0}";
            }
            yield return velocity;
            coroutineRunning = false;
        }
        protected virtual void OnHandWaved()
        {
            handWaved?.Invoke();
        }
    }
}
