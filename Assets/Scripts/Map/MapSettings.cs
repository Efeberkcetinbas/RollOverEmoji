using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/MapSettings")]
public class MapSettings : ScriptableObject
{
    [Header("Emoji Settings")]
    public List<EmojiData> emojis = new List<EmojiData>();

    [Header("Obstacle Settings")]
    public int obstacleCount = 5;

    [Header("Debug Options")]
    public bool enableDebug = true;
}

[System.Serializable]
public class EmojiData
{
    public GameObject emojiPrefab;
    public int count;
}