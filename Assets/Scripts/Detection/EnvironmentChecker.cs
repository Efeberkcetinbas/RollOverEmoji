using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvironmentChecker
{
    public static bool IsValidMove(Vector3 currentPosition, Vector3 direction, float raycastDistance, LayerMask groundLayer, LayerMask obstacleLayer)
    {
        Vector3 rayOrigin = currentPosition + Vector3.up * 0.5f;

        // Check for obstacles
        if (Physics.Raycast(rayOrigin, direction, raycastDistance, obstacleLayer))
        {
            Debug.Log("Obstacle detected. Cannot roll in this direction.");
            EventManager.Broadcast(GameEvent.OnPlayerCantRoll);
            return false;
        }

        // Calculate the target position after rolling
        Vector3 targetPosition = currentPosition + direction;

        // Perform a raycast downwards from slightly above the target position
        Vector3 groundCheckOrigin = targetPosition + Vector3.up * 0.5f;
        if (!Physics.Raycast(groundCheckOrigin, Vector3.down, raycastDistance, groundLayer))
        {
            Debug.Log("No ground detected at target position. Cannot roll in this direction.");
            EventManager.Broadcast(GameEvent.OnPlayerCantRoll);
            return false;
        }


        return true;
    }
}