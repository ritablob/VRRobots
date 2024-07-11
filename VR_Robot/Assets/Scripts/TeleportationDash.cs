using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportationDash : TeleportationProvider
{
    /* check when player is teleporting
     * how fast a player needs to dash to the position
     * lerp the player position to the teleport destination over the dashtime
     * 
    */
    // Start is called before the first frame update


    public Transform player;
    public InputActionAsset actionAsset;
    public Transform xrRigTransform;
    private InputAction teleportAction;
    public XRRayInteractor interactor;
    private Vector3 playerPosition;
    private Vector3 teleportationPosition;
    private float currentTime;
    private void Start()
    {
        teleportAction = actionAsset.FindActionMap("XRI Right Locomotion").FindAction("Teleport Mode");
        teleportAction.canceled += DashToDestination;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void DashToDestination(InputAction.CallbackContext ctx)
    {
        //Vector3 dashDistance = Vector3.Distance(xrRigTransform.position, TODO: GET POSITION OF RAYCAST);
        //StartCoroutine(Dash(dashDistance));
        //Vector3 speed = dashDistance / delayTime;
        
        interactor.GetLineOriginAndDirection(out playerPosition, out teleportationPosition);
        playerPosition = player.position;
        Vector3 dashPosition = currentRequest.destinationPosition;
        //var dashDistance = Vector3.Distance(playerPosition, teleportationPosition);
        Debug.Log(playerPosition + "player pos, " + dashPosition + "dash pos");
        var dashDirection = teleportationPosition - playerPosition;
        //float speed = dashDistance / delayTime;
        StartCoroutine(Dash(playerPosition, dashPosition));
    }

    private IEnumerator Dash(Vector3 startPos, Vector3 endPos)
    {
        /* calculate speed based on dash distance and delayTime 
         * every frame, move according to speed
         * StartCoroutine until it's done 
         */
        currentTime = 0f;
        yield return new WaitForSeconds(Time.deltaTime);
        while (currentTime < delayTime)
        {
            xrRigTransform.position = Vector3.Lerp(startPos, endPos, currentTime);
            //xrRigTransform.position += new Vector3(direction.x + speed * Time.deltaTime,
                //direction.y + speed * Time.deltaTime, direction.z + speed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
        
        // move xrrig by speed
        yield return null;
    }
}
