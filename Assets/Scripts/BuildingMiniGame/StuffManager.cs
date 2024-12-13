using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StuffManager : MonoBehaviour
{
    [SerializeField] private StuffConfig stuffConfig; // The configuration for this stuff
    [SerializeField] private GameObject stuffVisual; // The visual representation of the stuff

    private int collectedGameObjects = 0;
    private const string PlayerPrefsKey = "Stuff_";

    public bool IsUnlocked { get; private set; } = false;

    private void Start()
    {
        LoadProgress();
        UpdateStuffVisual();
    }

    public void AddGameObjects(GameObject[] emojis, float delayInterval)
    {
        if (IsUnlocked) return;

        for (int i = 0; i < emojis.Length; i++)
        {
            GameObject emoji = emojis[i];
            float delay = i * delayInterval;
            MoveEmoji(emoji, delay);
            
        }

        DOTween.Sequence()
            .AppendInterval(emojis.Length * delayInterval) // Wait for all animations
            .AppendCallback(() =>
            {
                collectedGameObjects += emojis.Length;

                if (collectedGameObjects >= stuffConfig.requiredGameObjects)
                {
                    UnlockStuff();
                }
                SaveProgress();
            });
    }

    private void MoveEmoji(GameObject emoji, float delay)
    {
        emoji.transform.DOMove(stuffVisual.transform.position, 0.5f)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                emoji.SetActive(false); // Deactivate emoji after use
            });
    }

    private void UnlockStuff()
    {
        IsUnlocked = true;
        Debug.Log($"{stuffConfig.stuffName} unlocked!");
        UpdateStuffVisual();
    }

    private void UpdateStuffVisual()
    {
        stuffVisual.SetActive(IsUnlocked);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey + stuffConfig.name, IsUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(PlayerPrefsKey + stuffConfig.name + "_collected", collectedGameObjects);
    }

    private void LoadProgress()
    {
        IsUnlocked = PlayerPrefs.GetInt(PlayerPrefsKey + stuffConfig.name, 0) == 1;
        collectedGameObjects = PlayerPrefs.GetInt(PlayerPrefsKey + stuffConfig.name + "_collected", 0);
        UpdateStuffVisual();
    }
}
