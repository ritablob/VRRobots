using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.AI;

public class CustomActions : MonoBehaviour
{
    private XRIDefaultInputActions inputActions;

    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Robot_Interaction_State_Machine robot;

    bool pressed;
    private Transform raycastStart;
    private Highlighter prevHighlighted;

    private void Awake()
    {
        inputActions = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.XRILeftInteraction.LeftPrimary.performed += OnLeftXButtonPressed;

        inputActions.XRILeftInteraction.Activate.performed += PressPointL;
        inputActions.XRILeftInteraction.Activate.canceled += ReleasePointL;

        inputActions.XRIRightInteraction.Activate.performed += PressPointR;
        inputActions.XRIRightInteraction.Activate.canceled += ReleasePointR;
    }

    private void OnDisable()
    {
        inputActions.XRILeftInteraction.LeftPrimary.performed -= OnLeftXButtonPressed;

        inputActions.XRILeftInteraction.Activate.performed += PressPointL;
        inputActions.XRILeftInteraction.Activate.canceled -= ReleasePointL;

        inputActions.XRIRightInteraction.Activate.performed += PressPointR;
        inputActions.XRIRightInteraction.Activate.canceled -= ReleasePointR;

        inputActions.Disable();
    }

    private void Update()
    {
        // If currently holding the point button, highlight aany interactable that you come across
        if (pressed) {
            if (Physics.Raycast(raycastStart.position, raycastStart.forward, out RaycastHit hit, 10, raycastMask)) {
                Director.instance.Log("Raycast good!");
                if (hit.collider.gameObject.TryGetComponent<Highlighter>(out Highlighter _highlight)) {
                    Director.instance.Log("Found highlight component");
                    // If highlighting a different object, un-highlight the previous one, and highlight this
                    if (prevHighlighted != null &&_highlight != prevHighlighted) {
                        prevHighlighted.HoverHighlight(false);
                        Director.instance.Log("Remove prev highlight!");
                        prevHighlighted = _highlight;
                        prevHighlighted.HoverHighlight(true);
                        return;
                    }
                    else if (prevHighlighted == null) {
                        prevHighlighted = _highlight;
                        prevHighlighted.HoverHighlight(true);
                        Director.instance.Log("Add new highlight good!");
                    }
                }
            }
            return;
        }

        if (prevHighlighted != null) {
            prevHighlighted.HoverHighlight(false);
            prevHighlighted = null;
        }
    }

    private void OnLeftXButtonPressed(InputAction.CallbackContext ctx)
    {
        robot.Context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.Attentive);
    }

    private void ReleasePointL(InputAction.CallbackContext ctx) {
        pressed = false;

        FindInteractable(leftHand);
    }

    private void ReleasePointR(InputAction.CallbackContext ctx) {
        pressed = false;

        FindInteractable(rightHand);
    }

    private void PressPointR(InputAction.CallbackContext ctx) {
        pressed = true;

        raycastStart = rightHand;
    }
    private void PressPointL(InputAction.CallbackContext ctx) {
        pressed = true;

        raycastStart = leftHand;
    }

    private void FindInteractable(Transform raycastStart) {
        if (Physics.Raycast(raycastStart.position, raycastStart.forward, out RaycastHit hit, 10, raycastMask)) {

            // If robot, interact with it
            if (hit.collider.TryGetComponent<Robot_Interaction_State_Machine>(out Robot_Interaction_State_Machine _robot)) {
                robot.InteractWithRobot();
            }
            // If garbage can, throw it away
            else if (hit.collider.TryGetComponent<Garbagecan>(out Garbagecan garbageCan)) {
                robot.Context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.Dump);
            }
            // If it is a piece of garbage, clean it up
            else if (hit.collider.TryGetComponent<Garbage_Bit>(out Garbage_Bit garbageBit)) {
                robot.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                robot.Context.IKController.Target = hit.collider.transform;
                robot.Context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.Grab);
            }
            // If nothing else, move the robot to that location
            else if (robot.GetCurrentState == Robot_Interaction_State_Machine.ERobotInteractionState.Attentive) {
                Director.instance.Log("MOVE TO POS!");
                robot.gameObject.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                robot.Context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.Idle);
            }
        }
    }
}
