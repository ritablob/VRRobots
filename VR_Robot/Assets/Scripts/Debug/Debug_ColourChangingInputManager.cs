using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Debug_ColourChangingInputManager : MonoBehaviour
{
    public InputActionReference inputReference;

    public Material newMaterial;
    public MeshRenderer mesh;

    private Material oldMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (inputReference != null)
        {
            inputReference.action.performed += ChangeMaterial;
        }
    }

    private void OnDisable()
    {
        if (inputReference != null)
        {
            inputReference.action.performed -= ChangeMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeMaterial(InputAction.CallbackContext ctx)
    {
        oldMaterial = mesh.material;
        mesh.material = newMaterial;
        newMaterial = oldMaterial;
    }
    private void OnTriggerEnter(Collider other)
    {
        oldMaterial = mesh.material;
        mesh.material = newMaterial;
        newMaterial = oldMaterial;
    }
}
