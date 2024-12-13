using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBuildingManager : MonoBehaviour
{
    [SerializeField] private BuildingManager[] buildingManagers;
    [SerializeField] private List<GameObject> emojis = new List<GameObject>();
    private int currentBuildingIndex = 0;

    private const string PlayerPrefsKey = "GeneralBuilding_CurrentIndex";

    private void Start()
    {
        LoadProgress();
        ActivateCurrentBuilding();
    }

    //Reset when new level or restart
    public void ResetEmojis()
    {
        emojis.Clear();
    }

    

    public void AddEmoji(GameObject emoji)
    {
        emoji.SetActive(true);
        emojis.Add(emoji);
    }

    public void TransferEmojisToBuildings()
    {
        if (currentBuildingIndex >= buildingManagers.Length)
        {
            Debug.LogWarning("No more buildings to unlock.");
            return;
        }

        int activeEmojiCount = emojis.FindAll(e => e.activeSelf).Count;

        if (activeEmojiCount > 0)
        {
            buildingManagers[currentBuildingIndex].AddGameObjects(
                emojis.ToArray(),
                0.3f // Delay interval for animation
            );

            if (buildingManagers[currentBuildingIndex].IsCompleted)
            {
                currentBuildingIndex++;
                SaveProgress();
                ActivateCurrentBuilding();
            }
        }
        else
        {
            Debug.Log("No active emojis available to transfer.");
        }
    }

    private void ActivateCurrentBuilding()
    {
        for (int i = 0; i < buildingManagers.Length; i++)
        {
            buildingManagers[i].gameObject.SetActive(i == currentBuildingIndex);
        }

        if (currentBuildingIndex < buildingManagers.Length)
        {
            Debug.Log($"Activated: {buildingManagers[currentBuildingIndex].gameObject.name}");
        }
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey, currentBuildingIndex);
    }

    private void LoadProgress()
    {
        currentBuildingIndex = PlayerPrefs.GetInt(PlayerPrefsKey, 0);
    }
}
