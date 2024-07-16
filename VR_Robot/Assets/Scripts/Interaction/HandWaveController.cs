using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Interaction
{
    public sealed class HandWaveController : MonoBehaviour
    {
        public event Action<Transform> handWaved;
        
        public Transform leftHandAnchor;
        public Transform rightHandAnchor;
        public TextMeshProUGUI text;
        public float responseTime = 1.0f;
        public float movementThreshold = 1.2f;
        private bool coroutineRunning;
        private Vector3 lastPositionLeft;
        private Vector3 lastPositionRight;

        private void Start()
        {
            coroutineRunning = false;
        }

        private void FixedUpdate()
        {
            // if coroutine is not running (i.e. no waving coroutine has started)
            if (!coroutineRunning)
            {
                float velocityLeft = Vector3.Distance(lastPositionLeft, leftHandAnchor.transform.position)/Time.fixedDeltaTime;
                float velocityRight = Vector3.Distance(lastPositionRight, rightHandAnchor.transform.position)/Time.fixedDeltaTime;
                
                //preview text
                text.color = Color.red;
                text.text = "Speed left: " + $"{velocityLeft:0.0}" + ", Speed right: " + $"{velocityRight:0.0}";
                
                //CheckRequirements(velocityLeft, leftHandAnchor);
                CheckRequirements(velocityRight, rightHandAnchor);
                
                lastPositionLeft = leftHandAnchor.position;
                lastPositionRight = rightHandAnchor.position;
            }
        }

        private bool IsUpwardRotation(Transform handTransform)
        {
            //.Log("Rotation "+handTransform.localEulerAngles.x);
            //return handTransform.localRotation.eulerAngles.x;
            // if hand rotation x is between -45 and -135, return true
            return handTransform.localEulerAngles.x > 315;
        }
        
/// <summary>
/// Checks whether the immediate requirements are met, then starts the GetAverageVelocity coroutine to check whether
/// the requirements are being met over a period of time.
/// </summary>
/// <param name="velocity">current velocity of an object.</param>
/// <param name="handTransform">Transform of a hand </param>
        private void CheckRequirements(float velocity, Transform handTransform)
        {
            if (velocity > movementThreshold)
            {
                StartCoroutine(GetAverageVelocity(handTransform));
            }
        }

/// <summary>
/// Coroutine to calculate an average velocity over a period of time.
/// </summary>
/// <param name="anchorTransform">transform of the hand object</param>
/// <returns></returns>
        private IEnumerator GetAverageVelocity(Transform anchorTransform)
        {
            coroutineRunning = true;
            Debug.Log("Called coroutine for "+anchorTransform.gameObject);
            
            var currentTime = 0f;
            var cumulativeVelocity = 0f;
            var lastPosition = anchorTransform.position;
            yield return null;
            // every frame for the duration of the responseTime
            while (currentTime < responseTime)
            {
                // add velocity to cumulativeVelocity 
                cumulativeVelocity +=
                    Vector3.Distance(lastPosition, anchorTransform.position);
                // reset the position 
                lastPosition = anchorTransform.position;
                // Add frame length
                currentTime += Time.fixedDeltaTime;
                yield return null;
            }


            // calculate average velocity
            var velocity = cumulativeVelocity / currentTime;
            Debug.Log("Done! Avg velocity of "+anchorTransform.gameObject+": "+velocity);
            
            if (velocity > movementThreshold && IsUpwardRotation(anchorTransform))
            {
                Debug.Log("Passed");
                // velocity passed the check 
                text.color = Color.green;
                text.text = "Waving! Speed: " + $"{velocity:0.0}";
                // TODO: check hand rotation 
                //handWaved.Invoke(anchorTransform);
            }
            else
            {
                // velocity failed the check
                text.color = Color.red;
                text.text = "Speed: " + $"{velocity:0.0}";
            }
            
            coroutineRunning = false;
            yield return null;
        }
    }
 }