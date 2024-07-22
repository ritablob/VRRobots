using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Interaction
{
    /// <summary>
    /// Cleanable is an object with a decal that can be cleaned.
    /// </summary>
    public class Cleanable : MonoBehaviour
    {
        public DecalProjector dirtDecal;
        public float cleaningTempo;


        private void Start()
        {
            //StartCoroutine(CleanDirt(1f));
        }

        public IEnumerator CleanDirt(float cleaningSpeed)
        {
            Debug.Log("Dirtiness - " + dirtDecal.fadeFactor);
            if (dirtDecal.fadeFactor > 0f)
            {
                yield return new WaitForSeconds(cleaningSpeed);
                dirtDecal.fadeFactor -= 0.1f * cleaningTempo;
                StartCoroutine(CleanDirt(cleaningSpeed));
            }
            else
            {
                Debug.Log("Done cleaning!");
            }

            yield return 0;
        }
    }
}