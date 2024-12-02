using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/MapSettings")]
public class MapSettings : ScriptableObject
{
    public Vector3 mapSize = new Vector3(10, 1, 10);
    public List<EmojiData> emojis = new List<EmojiData>();
    public int obstacleCount = 5;
    public bool enableDebug = true;
}

[System.Serializable]
public class EmojiData
{
    public GameObject emojiPrefab;
    public int count;
}
