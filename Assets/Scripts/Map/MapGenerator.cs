using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private MapSettings mapSettings;

    [Header("Prefabs")]
    [SerializeField] private GameObject obstaclePrefab;

    [Header("Debug Options")]
    [SerializeField] private GameObject debugPathMarker;

    [Header("Ground Setup")]
    [SerializeField] private List<Transform> groundTransforms;

    private Transform mapParent;
    private List<Vector3> groundPositions = new List<Vector3>();
    private List<Vector3> obstaclePositions = new List<Vector3>();
    private List<Vector3> emojiPositions = new List<Vector3>();
    private List<Vector3> unreachablePositions = new List<Vector3>();

    public void GenerateMap()
    {
        if (mapParent != null) Destroy(mapParent.gameObject); // Clear old map
        mapParent = new GameObject("Map").transform; // Create new map parent

        groundPositions.Clear();
        obstaclePositions.Clear();
        emojiPositions.Clear();
        unreachablePositions.Clear();

        Debug.Log("Starting map generation...");

        // Collect ground positions
        CollectGroundPositions();

        // Validate settings BEFORE placement
        if (!ValidateMapSettings())
        {
            Debug.LogError("Map settings are invalid. Adjust emoji or obstacle counts or ground layout.");
            return;
        }

        // Place emojis and obstacles
        PlaceEmojis();
        PlaceObstacles();

        // Validate map accessibility
        if (!AreAllEmojisAccessible())
        {
            Debug.LogWarning("Some emojis are unreachable. Adjust ground or obstacle placement.");
        }

        // Optional: Visualize debug information
        if (mapSettings.enableDebug)
        {
            VisualizeDebugInfo();
        }

        Debug.Log("Map generation complete.");
    }

    private void CollectGroundPositions()
    {
        foreach (var ground in groundTransforms)
        {
            if (ground != null)
            {
                groundPositions.Add(ground.position);
            }
        }

        Debug.Log($"Ground collection complete. Total ground tiles: {groundPositions.Count}");
    }

    private bool ValidateMapSettings()
    {
        int totalObjects = mapSettings.obstacleCount;
        foreach (var emoji in mapSettings.emojis)
        {
            totalObjects += emoji.count;
        }

        if (totalObjects > groundPositions.Count)
        {
            Debug.LogError("Too many objects for the available ground. Reduce emoji or obstacle counts.");
            return false;
        }

        return true;
    }

    private void PlaceEmojis()
    {
        foreach (var emojiData in mapSettings.emojis)
        {
            int placedCount = 0;

            while (placedCount < emojiData.count)
            {
                Vector3 position = GetRandomPosition(true);

                if (position != Vector3.zero && !emojiPositions.Contains(position))
                {
                    Instantiate(emojiData.emojiPrefab, position + Vector3.up * 0.3f, Quaternion.identity, mapParent);
                    emojiPositions.Add(position);
                    placedCount++;
                }
            }
        }
    }

    private void PlaceObstacles()
    {
        int placedCount = 0;
        int maxAttempts = 100; // Prevent infinite loops

        while (placedCount < mapSettings.obstacleCount && maxAttempts > 0)
        {
            maxAttempts--;

            // Get a unique random position
            Vector3 position = GetRandomPosition(false);

            if (position == Vector3.zero) continue; // Skip invalid positions

            // Ensure no duplicate and map remains accessible
            if (!obstaclePositions.Contains(position) && IsMapAccessibleAfterAddingObstacle(position))
            {
                Instantiate(obstaclePrefab, position + Vector3.up * 0.85f, Quaternion.identity, mapParent);
                obstaclePositions.Add(position);
                placedCount++;
            }
        }

        if (placedCount < mapSettings.obstacleCount)
        {
            Debug.LogWarning($"Could only place {placedCount} out of {mapSettings.obstacleCount} obstacles.");
        }
    }

    private Vector3 GetRandomPosition(bool avoidOccupied)
    {
        int maxAttempts = 100;
        Vector3 position;

        for (int attempts = 0; attempts < maxAttempts; attempts++)
        {
            position = groundPositions[Random.Range(0, groundPositions.Count)];

            bool isOccupied = emojiPositions.Contains(position) || obstaclePositions.Contains(position);

            if (!avoidOccupied || !isOccupied)
            {
                return position;
            }
        }

        Debug.LogWarning("Max attempts reached while finding a random position.");
        return Vector3.zero; // Indicate failure
    }

    private bool IsMapAccessibleAfterAddingObstacle(Vector3 obstaclePosition)
    {
        obstaclePositions.Add(obstaclePosition);
        bool accessible = AreAllEmojisAccessible();
        obstaclePositions.Remove(obstaclePosition);
        return accessible;
    }

    private bool AreAllEmojisAccessible()
    {
        if (emojiPositions.Count == 0) return true;

        Queue<Vector3> queue = new Queue<Vector3>();
        HashSet<Vector3> visited = new HashSet<Vector3>();

        Vector3 startPosition = groundPositions[0];
        queue.Enqueue(startPosition);
        visited.Add(startPosition);

        int accessibleEmojis = 0;

        while (queue.Count > 0)
        {
            Vector3 current = queue.Dequeue();

            if (emojiPositions.Contains(current))
            {
                accessibleEmojis++;
                if (accessibleEmojis == emojiPositions.Count)
                {
                    return true;
                }
            }

            foreach (Vector3 neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) &&
                    groundPositions.Contains(neighbor) &&
                    !obstaclePositions.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return false;
    }

    private IEnumerable<Vector3> GetNeighbors(Vector3 position)
    {
        yield return position + Vector3.right;
        yield return position + Vector3.left;
        yield return position + Vector3.forward;
        yield return position + Vector3.back;
    }

    private void VisualizeDebugInfo()
    {
        foreach (var pos in unreachablePositions)
        {
            var marker = Instantiate(debugPathMarker, pos + Vector3.up * 0.5f, Quaternion.identity, mapParent);
            marker.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
