using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private List<Sticker> stickers;

    [SerializeField] private float unlockInterval = 0.5f; // Time interval between unlocking stickers
    private bool isUnlocking = false;

   /* private void Start()
    {
        // Load saved data
        LoadStickers();
    }*/

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCollectSticker,OnCollectSticker);
        EventManager.AddHandler(GameEvent.OnStickerPanelOpen,LoadStickers);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCollectSticker,OnCollectSticker);
        EventManager.RemoveHandler(GameEvent.OnStickerPanelOpen,LoadStickers);
    }

    private void OnCollectSticker()
    {
        gameData.starAmount++;
        PlayerPrefs.SetInt("Star",gameData.starAmount);
        EventManager.Broadcast(GameEvent.OnIncreaseStar);
    }
    public void UnlockStickers()
    {
        if (!isUnlocking)
        {
            StartCoroutine(UnlockStickersWithInterval());
        }
    }

    private IEnumerator UnlockStickersWithInterval()
    {
        isUnlocking = true;

        foreach (Sticker sticker in stickers)
        {
            if (gameData.starAmount <= 0)
            {
                Debug.Log("Stars depleted!");
                //EventManager.Broadcast(GameEvent.OnFillStarFull);
                break;
            }

            if (sticker.percentOpen < 100) // Process only stickers not fully opened
            {
                int starsNeeded = sticker.requiredStars - Mathf.FloorToInt(sticker.requiredStars * (sticker.percentOpen / 100));
                if (gameData.starAmount >= starsNeeded)
                {
                    // Fully unlock the sticker
                    gameData.starAmount -= starsNeeded;
                    PlayerPrefs.SetInt("Star",gameData.starAmount);
                    sticker.percentOpen = 100;
                }
                else
                {
                    // Partially unlock the sticker
                    float addedPercent = (gameData.starAmount / (float)sticker.requiredStars) * 100;
                    sticker.percentOpen += addedPercent;
                    gameData.starAmount = 0;
                    PlayerPrefs.SetInt("Star",gameData.starAmount);
                }

                // Update the UI incrementally during unlocking
                EventManager.Broadcast(GameEvent.OnIncreaseStar);
                EventManager.Broadcast(GameEvent.OnStickerSound);
                sticker.UpdateStickerUI();
                // Wait for the specified interval before processing the next sticker
                yield return new WaitForSeconds(unlockInterval);
            }
        }

        SaveStickers();
        isUnlocking = false; // Reset the unlocking flag after completion
    }

    // Save stickers' data
    public void SaveStickers()
    {
        List<StickerData> stickerDataList = new List<StickerData>();
        foreach (var sticker in stickers)
        {
            stickerDataList.Add(sticker.SaveSticker());
        }

        // Save stickerDataList to PlayerPrefs or a JSON file
        StickerSaveSystem.SaveStickerData(stickerDataList);
    }

    // Load stickers' data
    private void LoadStickers()
    {
        List<StickerData> stickerDataList = StickerSaveSystem.LoadStickerData();
        if (stickerDataList == null) return;

        for (int i = 0; i < stickers.Count; i++)
        {
            if (i < stickerDataList.Count)
            {
                stickers[i].LoadSticker(stickerDataList[i].percentOpen, stickerDataList[i].isFullyOpened);
            }
        }
    }
   
}
