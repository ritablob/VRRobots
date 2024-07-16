using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.AI;
using Interaction;

public class CustomActions : MonoBehaviour
{
    private XRIDefaultInputActions inputActions;

    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform head;
    [SerializeField] private Robot_Interaction_State_Machine robot;
    [SerializeField] private HandWaveController waver;

    public float lookAngleThreshold;

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

        waver.handWaved += Wave;

        inputActions.XRILeftInteraction.LeftPrimary.performed += DEBUG_Wave;

        inputActions.XRILeftInteraction.Activate.performed += PressPointL;
        inputActions.XRILeftInteraction.Activate.canceled += ReleasePointL;

        inputActions.XRIRightInteraction.Activate.performed += PressPointR;
        inputActions.XRIRightInteraction.Activate.canceled += ReleasePointR;
    }

    private void OnDisable()
    {
        waver.handWaved -= Wave;

        inputActions.XRILeftInteraction.LeftPrimary.performed -= DEBUG_Wave;

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
                if (hit.collider.gameObject.TryGetComponent<Highlighter>(out Highlighter _highlight)) {
                    // If highlighting a different object, un-highlight the previous one, and highlight this
                    if (prevHighlighted != null &&_highlight != prevHighlighted) {
                        prevHighlighted.HoverHighlight(false);
                        prevHighlighted = _highlight;
                        prevHighlighted.HoverHighlight(true);
                        return;
                    }
                    else if (prevHighlighted == null) {
                        prevHighlighted = _highlight;
                        prevHighlighted.HoverHighlight(true);
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

    private void Wave(Transform hand)
    {
        // If not looking near the robot, return
        if (!IsLookingAt()) { return; }

        robot.Context.SetAttentive(true);
        Director.instance.Log("robot_idle True");
    }

    private void DEBUG_Wave(InputAction.CallbackContext ctx)
    {
        Debug.Log("robot_idle pressed");

        // If not looking near the robot, return
        if (!IsLookingAt()) { return; }

//        Director.instance.Log("robot_idle entered attentive!");
        robot.Context.SetAttentive(true);
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
                robot.GetState.DEBUG_SwitchState(Robot_Interaction_State_Machine.ERobotInteractionState.Dump);
            }
            // If it is a piece of garbage, clean it up
            else if (hit.collider.TryGetComponent<Garbage_Bit>(out Garbage_Bit garbageBit)) {
                robot.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                robot.Context.IKController.Target = hit.collider.transform;
                robot.GetState.DEBUG_SwitchState(Robot_Interaction_State_Machine.ERobotInteractionState.Grab);
            }
            // If nothing else, move the robot to that location
            else if (robot.GetCurrentState == Robot_Interaction_State_Machine.ERobotInteractionState.Attentive) {
                Director.instance.Log("MOVE TO POS!");
                robot.gameObject.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                robot.GetState.DEBUG_SwitchState(Robot_Interaction_State_Machine.ERobotInteractionState.Idle);
            }
        }
    }

    private bool IsLookingAt() {
        // Normalize position vectors to player-head height
        Vector3 roboPos = new Vector3(robot.transform.position.x, head.position.y, robot.transform.position.z);

        Vector3 directionToB = (roboPos - head.position).normalized;
        float angle = Vector3.Angle(head.forward, directionToB);

        Director.instance.Log($"Angle = {angle}, = {lookAngleThreshold / 2}");

        return angle <= lookAngleThreshold / 2;
    }
}
