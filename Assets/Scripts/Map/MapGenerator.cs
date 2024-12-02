using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private MapSettings mapSettings;

    [Header("Prefabs")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject obstaclePrefab;

    [Header("Debug Options")]
    [SerializeField] private GameObject debugPathMarker;

    private Transform mapParent;
    private List<Vector3> groundPositions = new List<Vector3>();
    private List<Vector3> obstaclePositions = new List<Vector3>();
    private List<Vector3> emojiPositions = new List<Vector3>();
    private List<Vector3> debugPath = new List<Vector3>();
    private List<Vector3> unreachablePositions = new List<Vector3>();

    public void GenerateMap()
    {
        if (mapParent != null) Destroy(mapParent.gameObject); // Clear old map
        mapParent = new GameObject("Map").transform; // Create new map parent

        groundPositions.Clear();
        obstaclePositions.Clear();
        emojiPositions.Clear();
        debugPath.Clear();
        unreachablePositions.Clear();

        Debug.Log("Starting map generation...");

        // Generate ground first (this is critical for base setup)
        GenerateGround();

        // Validate settings AFTER ground generation
        if (!ValidateMapSettings())
        {
            Debug.LogError("Map settings are invalid. Adjust settings in the MapSettings ScriptableObject.");
            return;
        }

        // Proceed to generate emojis and obstacles
        PlaceEmojis();
        PlaceObstacles();

        // Optional: Visualize debug information
        if (mapSettings.enableDebug)
        {
            VisualizeDebugInfo();
        }

        Debug.Log("Map generation complete.");
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
            Debug.Log(totalObjects + "G" + groundPositions.Count);
            Debug.LogError("Too many objects for the available ground. Reduce emoji or obstacle counts.");
            return false;
        }

        return true;
    }

    private void GenerateGround()
    {
        // Check map size dimensions
        if (mapSettings.mapSize.x <= 0 || mapSettings.mapSize.z <= 0)
        {
            Debug.LogError("Map size dimensions are invalid. Ensure mapSize.x and mapSize.z are greater than zero.");
            return;
        }

        for (int x = 0; x < mapSettings.mapSize.x; x++)
        {
            for (int z = 0; z < mapSettings.mapSize.z; z++)
            {
                Vector3 position = new Vector3(x, 0, z);

                // Instantiate ground tile
                var ground = Instantiate(groundPrefab, position, Quaternion.identity, mapParent);

                // Ensure it's correctly added to the map structure
                groundPositions.Add(position);
            }
        }

        Debug.Log($"Ground generation complete. Generated {groundPositions.Count} tiles.");
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
                    Instantiate(emojiData.emojiPrefab, position + Vector3.up * 0.5f, Quaternion.identity, mapParent);
                    emojiPositions.Add(position);
                    placedCount++;
                }
            }
        }
    }

    private void PlaceObstacles()
    {
        int placedCount = 0;
        int maxAttempts = 50;

        while (placedCount < mapSettings.obstacleCount)
        {
            Vector3 position = GetRandomPosition(false);
            if (position == Vector3.zero) break;

            if (IsMapAccessibleAfterAddingObstacle(position))
            {
                Instantiate(obstaclePrefab, position + Vector3.up * 0.5f, Quaternion.identity, mapParent);
                obstaclePositions.Add(position);
                placedCount++;
            }
            else
            {
                maxAttempts--;
                if (maxAttempts <= 0)
                {
                    Debug.LogWarning("Failed to place all obstacles within attempt limit.");
                    break;
                }
            }
        }
    }

    private Vector3 GetRandomPosition(bool avoidOccupied)
    {
        int maxAttempts = 100;
        int attempts = 0;
        Vector3 position;

        do
        {
            position = groundPositions[Random.Range(0, groundPositions.Count)];
            attempts++;

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Max attempts reached while finding a random position.");
                return Vector3.zero;
            }
        }
        while ((avoidOccupied && (emojiPositions.Contains(position) || obstaclePositions.Contains(position))) ||
               (!groundPositions.Contains(position)));

        return position;
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

        Vector3 startPosition = new Vector3(0, 0, 0);
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
        foreach (var pos in debugPath)
        {
            Instantiate(debugPathMarker, pos + Vector3.up * 0.5f, Quaternion.identity, mapParent);
        }

        foreach (var pos in unreachablePositions)
        {
            var marker = Instantiate(debugPathMarker, pos + Vector3.up * 0.5f, Quaternion.identity, mapParent);
            marker.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void DebugMapState(string phase)
    {
        Debug.Log($"{phase}:");
        Debug.Log($"Ground Positions: {groundPositions.Count}");
        Debug.Log($"Obstacle Positions: {obstaclePositions.Count}");
        Debug.Log($"Emoji Positions: {emojiPositions.Count}");
    }
}
