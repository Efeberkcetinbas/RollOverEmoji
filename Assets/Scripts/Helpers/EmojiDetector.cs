using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EmojiDetector : MonoBehaviour
{   
    [SerializeField] private List<Transform> faces=new List<Transform>();
    [SerializeField] private GameData gameData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCollect3Emoji,OnCollect3Emoji);        
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCollect3Emoji,OnCollect3Emoji);
    }


    private void OnCollect3Emoji()
    {
        EventManager.Broadcast(GameEvent.OnCleanCube);
        DetectThreeMatchingEmojis();
    }
    public void DetectThreeMatchingEmojis()
    {
        // Find all Emoji objects in the scene
        Emoji[] allEmojis = FindObjectsOfType<Emoji>();

        if (allEmojis.Length == 0)
        {
            Debug.Log("No emojis found in the scene.");
            return;
        }

        // Get the type of the first detected emoji
        EmojiType firstEmojiType = allEmojis[0].EmojiType;
        Debug.Log($"First detected emoji type: {firstEmojiType}");

        List<Emoji> matchingEmojis = new List<Emoji>();

        // Find all emojis of the same type
        foreach (Emoji emoji in allEmojis)
        {
            if (emoji.EmojiType == firstEmojiType)
            {
                matchingEmojis.Add(emoji);
                if (matchingEmojis.Count == 3)
                {
                    break;
                }
            }
        }

        // Check if there are at least 3 of the same type
        if (matchingEmojis.Count == 3)
        {
            Debug.Log($"Found at least 3 emojis of type {firstEmojiType}.");
            MoveEmojis(matchingEmojis); // Optional: Highlight the matched emojis
        }
        else
        {
            Debug.Log($"Less than 3 emojis of type {firstEmojiType} found.");
        }
    }

    private void MoveEmojis(List<Emoji> emojis)
    {
        int completedCount = 0; // Counter to track completed animations
        gameData.CanSwipe=false;
        for (int i = 0; i < emojis.Count; i++)
        {
            emojis[i].transform.DOJump(faces[i].position, 1, 1, 0.5f).OnComplete(() =>
            {
                completedCount++; // Increment counter when animation completes
                if (completedCount == emojis.Count)
                {
                    Debug.Log("All animations completed!");
                    gameData.CanSwipe=true;
                }
            });
        }
    }
}
