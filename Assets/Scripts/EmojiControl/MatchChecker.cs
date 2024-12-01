using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
    [SerializeField] private List<FaceTrigger> cubeFaces; // Assign all cube face triggers in the Inspector

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
    }

    private void HandleMatch(List<FaceTrigger> matchingFaces)
    {
        Debug.Log($"Match found for {matchingFaces[0].AssignedEmojiType}");

        foreach (FaceTrigger face in matchingFaces)
        {
            face.ResetFace(); // Reset face sprite and enum
        }

        // Optionally trigger additional effects or animations here
    }
}
