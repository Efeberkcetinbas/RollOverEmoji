using UnityEngine;

public static class StateManager
{
    private const string PositionKey = "CubePosition";
    private const string RotationKey = "CubeRotation";

    public static void SaveState(Transform cube)
    {
        PlayerPrefs.SetString(PositionKey, JsonUtility.ToJson(cube.position));
        PlayerPrefs.SetString(RotationKey, JsonUtility.ToJson(cube.rotation));
        PlayerPrefs.Save();
    }

    public static void LoadState(Transform cube)
    {
        if (PlayerPrefs.HasKey(PositionKey) && PlayerPrefs.HasKey(RotationKey))
        {
            cube.position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString(PositionKey));
            cube.rotation = JsonUtility.FromJson<Quaternion>(PlayerPrefs.GetString(RotationKey));
        }
    }
}