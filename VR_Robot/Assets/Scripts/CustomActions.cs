using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class CustomActions : MonoBehaviour
{
    private XRIDefaultInputActions inputActions;

    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Robot_Interaction_State_Machine robot;

    private void Awake()
    {
        inputActions = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.XRILeftInteraction.LeftPrimary.performed += OnLeftXButtonPressed;
        inputActions.XRILeftInteraction.Activate.canceled += ReleasePointL;
        inputActions.XRIRightInteraction.Activate.canceled += ReleasePointR;
    }

    private void OnDisable()
    {
        inputActions.XRILeftInteraction.LeftPrimary.performed -= OnLeftXButtonPressed;
        inputActions.XRILeftInteraction.Activate.canceled -= ReleasePointL;
        inputActions.XRIRightInteraction.Activate.canceled -= ReleasePointR;

        inputActions.Disable();
    }

    private void OnLeftXButtonPressed(InputAction.CallbackContext ctx)
    {
        robot.Context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.Attentive);
    }

    private void ReleasePointL(InputAction.CallbackContext ctx) {
        FindInteractable(leftHand);
    }

    private void ReleasePointR(InputAction.CallbackContext ctx) {
        FindInteractable(rightHand);
    }

    private void FindInteractable(Transform raycastStart) {
        if (Physics.Raycast(raycastStart.position, raycastStart.forward, out RaycastHit hit, 10, raycastMask)) {
            Debug.Log(hit.collider.gameObject.name, hit.collider.gameObject);

            // If robot, interact with it
            if (hit.collider.TryGetComponent<Robot_Interaction_State_Machine>(out Robot_Interaction_State_Machine robot)) {
                robot.InteractWithRobot();
            }
            else if (hit.collider.TryGetComponent<Garbagecan>(out Garbagecan garbageCan)) {
                FindObjectOfType<Robot_Interaction_State_Machine>().Context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.Dump);
            }
        }
    }
}
