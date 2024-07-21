using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SIngleton helper for applying Highlighter VFX to every highlightable object 
/// </summary>
public class VFXApplicationHelper : MonoBehaviour
{
    public static VFXApplicationHelper instance { get; private set; }
    
    public GameObject highlightParticle;
    
    private void Awake() 
    { 
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }
    public void ApplyHighlightParticleOnStart(GameObject highlightObject)
    {
        var child = Instantiate(highlightParticle, highlightObject.transform.position, 
            highlightObject.transform.rotation, highlightObject.transform); // creates child object of a prefab 

        // sets shape mesh renderer
        var shape = child.GetComponent<ParticleSystem>().shape;
        shape.shapeType = ParticleSystemShapeType.MeshRenderer;
        shape.meshRenderer = highlightObject.GetComponent<MeshRenderer>();
    }
}
