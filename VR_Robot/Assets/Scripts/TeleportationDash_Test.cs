using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class DashTeleportationProvider : TeleportationProvider
{
    public float dashDuration = 1f; // Duration of the dash in seconds
    private Coroutine dashCoroutine;

    public override bool QueueTeleportRequest(TeleportRequest teleportRequest)
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }

        dashCoroutine = StartCoroutine(DashToLocation(teleportRequest.destinationPosition));
        return true;
    }

    private IEnumerator DashToLocation(Vector3 destination)
    {
        // Start position
        Vector3 startPosition = system.xrOrigin.Camera.transform.position;

        // Calculate the dash time
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            // Move the player towards the destination
            Vector3 newPosition = Vector3.Lerp(startPosition, destination, elapsedTime / dashDuration);
            MoveRig(newPosition - system.xrOrigin.Camera.transform.position);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Final position adjustment
        MoveRig(destination - system.xrOrigin.Camera.transform.position);
    }

    private void MoveRig(Vector3 translation)
    {
        system.xrOrigin.transform.position += translation;
    }
}
