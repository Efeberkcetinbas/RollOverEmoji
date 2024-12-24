using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
    [SerializeField] private List<FaceTrigger> cubeFaces; // Assign all cube face triggers in the Inspector
    [SerializeField] private GameData gameData;

    private WaitForSeconds waitForSeconds;

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(1);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCheckMatch, CheckForMatches);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCheckMatch, CheckForMatches);
    }

    private void CheckForMatches()
    {
        Dictionary<EmojiType, List<FaceTrigger>> matches = new Dictionary<EmojiType, List<FaceTrigger>>();

        // Group cube faces by their assigned emoji type
        foreach (FaceTrigger face in cubeFaces)
        {
            // Skip faces with no emoji assigned (EmojiType.None)
            if (face.AssignedEmojiType == EmojiType.None) continue;

            if (!matches.ContainsKey(face.AssignedEmojiType))
            {
                matches[face.AssignedEmojiType] = new List<FaceTrigger>();
            }

            matches[face.AssignedEmojiType].Add(face);
        }

        // Check for matches (3 or more)
        foreach (var match in matches)
        {
            if (match.Value.Count >= 3)
            {
                HandleMatch(match.Value); // Handle matched faces
            }
        }

        CheckForPotentialMatches();
    }

    private void HandleMatch(List<FaceTrigger> matchingFaces)
    {
        Debug.Log($"Match found for {matchingFaces[0].AssignedEmojiType}");
        EventManager.Broadcast(GameEvent.OnMatch);

        foreach (FaceTrigger face in matchingFaces)
        {
            face.ResetFace(); // Reset face sprite and enum
        }

        CheckForEmojiNumber();

        // Optionally trigger additional effects or animations here
    }

    private void CheckForEmojiNumber()
    {
        gameData.levelEmojiCount-=3;
        if(gameData.levelEmojiCount<=0)
        {
            Debug.Log("SUCCESS");
            gameData.isGameEnd=true;
            StartCoroutine(GetSuccess());
        }
    }

    private IEnumerator GetSuccess()
    {
        yield return waitForSeconds;
        EventManager.Broadcast(GameEvent.OnSuccess);
    }

    private void CheckForPotentialMatches()
    {
        Dictionary<EmojiType, int> emojiCounts = new Dictionary<EmojiType, int>();
        int emptyFacesCount = 0;

        // Count assigned emoji types and empty faces
        foreach (FaceTrigger face in cubeFaces)
        {
            if (face.AssignedEmojiType == EmojiType.None)
            {
                emptyFacesCount++;
            }
            else
            {
                if (!emojiCounts.ContainsKey(face.AssignedEmojiType))
                {
                    emojiCounts[face.AssignedEmojiType] = 0;
                }

                emojiCounts[face.AssignedEmojiType]++;
            }
        }

        // Check if any emoji type can form a match-3
        foreach (var emojiCount in emojiCounts)
        {
            int totalPossible = emojiCount.Value + emptyFacesCount;
            if (totalPossible >= 3)
            {
                Debug.Log($"Possible to form a match-3 for {emojiCount.Key}");
                return; // No need to check further, at least one match-3 is possible
            }
        }

        // Also consider if all empty faces could form a match for a single new emoji type
        if (emptyFacesCount >= 3)
        {
            Debug.Log("Possible to form a match-3 using all empty faces.");
        }
        else
        {
            Debug.Log("Fail: No possible way to form a match-3.");
        }
    }
}
