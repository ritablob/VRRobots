using System.Collections;
using UnityEngine;

namespace Interaction
{
    public class Cleanable : MonoBehaviour
    {
        public float cleannessPercent;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public IEnumerator CleanDirt(float cleaningSpeed)
        {
            Debug.Log("Cleanness = "+cleannessPercent);
            if (cleannessPercent < 100f)
            {
                cleannessPercent += 10f;
                yield return new WaitForSeconds(cleaningSpeed);
                StartCoroutine(CleanDirt(cleaningSpeed));
            }
            
            yield return 0;
        }
    }
}
