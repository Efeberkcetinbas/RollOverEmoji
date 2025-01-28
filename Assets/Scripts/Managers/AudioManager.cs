using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AudioClipsGameManagement
{
    public AudioClip SuccessSound;
    public AudioClip SuccessUISound;
    public AudioClip RestartSound;
    public AudioClip NextLevelSound;
    public AudioClip StartSound;
    public AudioClip FailUISound;
    public AudioClip PanelChangeSound;
    public AudioClip ChangeMonsterPanelSound;
    public AudioClip ChangeMonsterPartSound;
    public AudioClip ChangeMonsterColorSound;
}

[Serializable]
public class AudioClipsPlayer
{
    public AudioClip CubeRollStartSound;
    public AudioClip CubeRollEndSound;
    public AudioClip IncreaseScoreSound;
    public AudioClip StickerSound;
    public AudioClip Match3Sound;
    public AudioClip CantRollSound;
    public AudioClip StickerSpawnSound;
    public AudioClip CantCollectSound;
    public AudioClip StickerStarSound;
    public AudioClip UpdateStickerSound;

}

[Serializable]
public class AudioClipsHelpers
{
    public AudioClip RotateHelperSound;
    public AudioClip CleanHelperSound;
    public AudioClip Collect3Sound;

}
public class AudioManager : MonoBehaviour
{
    public AudioClip GameLoop,BuffMusic;

    AudioSource musicSource,effectSource;

    [Header("Game Management")]
    public AudioClipsGameManagement audioClipsGameManagement;
    [Header("Player")]
    public AudioClipsPlayer audioClipsPlayer;
    [Header("Helpers")]
    public AudioClipsHelpers audioClipsHelpers;



    private void Start() 
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = GameLoop;
        //musicSource.Play();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable() 
    {
        // GAME MANAGEMENT
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.AddHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.AddHandler(GameEvent.OnPanelsChange,OnPanelsChange);
        EventManager.AddHandler(GameEvent.OnChangeMonsterPart,OnChangeMonsterPart);
        EventManager.AddHandler(GameEvent.OnChangePartSprite,OnChangePartSprite);
        EventManager.AddHandler(GameEvent.OnChangePartColor,OnChangePartColor);

        //PLAYER
        EventManager.AddHandler(GameEvent.OnCubeRollingStart,OnCubeRollingStart);
        EventManager.AddHandler(GameEvent.OnCubeRollingEnd,OnCubeRollingEnd);
        EventManager.AddHandler(GameEvent.OnCoinIncreaseSound,OnCoinIncreaseSound);
        EventManager.AddHandler(GameEvent.OnMatch,OnMatch);
        EventManager.AddHandler(GameEvent.OnCollectSticker,OnCollectSticker);
        EventManager.AddHandler(GameEvent.OnPlayerCantRoll,OnPlayerCantRoll);
        EventManager.AddHandler(GameEvent.OnStickerSpawn,OnStickerSpawn);
        EventManager.AddHandler(GameEvent.OnCantCollect,OnCantCollect);
        EventManager.AddHandler(GameEvent.OnStickerSound,OnStickerSound);
        EventManager.AddHandler(GameEvent.OnUpdateSticker,OnUpdateSticker);

        //BOOSTERS
        EventManager.AddHandler(GameEvent.OnRotateHorizontal,OnRotateHorizontal);
        EventManager.AddHandler(GameEvent.OnRotateVertical,OnRotateVertical);
        EventManager.AddHandler(GameEvent.OnCollect3Emoji,OnCollect3Emoji);
        EventManager.AddHandler(GameEvent.OnCleanCube,OnCleanCube);
        

    }
    private void OnDisable() 
    {
        // GAME MANAGEMENT
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.RemoveHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.RemoveHandler(GameEvent.OnPanelsChange,OnPanelsChange);
        EventManager.RemoveHandler(GameEvent.OnChangeMonsterPart,OnChangeMonsterPart);
        EventManager.RemoveHandler(GameEvent.OnChangePartSprite,OnChangePartSprite);
        EventManager.RemoveHandler(GameEvent.OnChangePartColor,OnChangePartColor);

        //PLAYER
        EventManager.RemoveHandler(GameEvent.OnCubeRollingStart,OnCubeRollingStart);
        EventManager.RemoveHandler(GameEvent.OnCubeRollingEnd,OnCubeRollingEnd);
        EventManager.RemoveHandler(GameEvent.OnCoinIncreaseSound,OnCoinIncreaseSound);
        EventManager.RemoveHandler(GameEvent.OnMatch,OnMatch);
        EventManager.RemoveHandler(GameEvent.OnCollectSticker,OnCollectSticker);
        EventManager.RemoveHandler(GameEvent.OnPlayerCantRoll,OnPlayerCantRoll);
        EventManager.RemoveHandler(GameEvent.OnStickerSpawn,OnStickerSpawn);
        EventManager.RemoveHandler(GameEvent.OnCantCollect,OnCantCollect);
        EventManager.RemoveHandler(GameEvent.OnStickerSound,OnStickerSound);
        EventManager.RemoveHandler(GameEvent.OnUpdateSticker,OnUpdateSticker);

        //BOOSTERS
        EventManager.RemoveHandler(GameEvent.OnRotateHorizontal,OnRotateHorizontal);
        EventManager.RemoveHandler(GameEvent.OnRotateVertical,OnRotateVertical);
        EventManager.RemoveHandler(GameEvent.OnCollect3Emoji,OnCollect3Emoji);
        EventManager.RemoveHandler(GameEvent.OnCleanCube,OnCleanCube);
    }

    
    #region GameManagement


    private void OnSuccess()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.SuccessSound);
    }

    private void OnSuccessUI()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.SuccessUISound);
    }


    private void OnRestartLevel()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.RestartSound);
    }

    private void OnNextLevel()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.NextLevelSound);
    }

    private void OnGameStart()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.StartSound);
    }

    private void OnFailUI()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.FailUISound);
    }

    private void OnPanelsChange()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.PanelChangeSound);
    }

    private void OnChangeMonsterPart()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.ChangeMonsterPanelSound);
    }

    private void OnChangePartSprite()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.ChangeMonsterPartSound);
    }

    private void OnChangePartColor()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.ChangeMonsterColorSound);
    }
    
    #endregion

  

    #region Player
    
    private void OnCubeRollingStart()
    {
        effectSource.pitch=Random.Range(1, 1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.CubeRollStartSound);
    }

    private void OnCubeRollingEnd()
    {
        effectSource.pitch=Random.Range(1, 1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.CubeRollEndSound);
    }

    private void OnCoinIncreaseSound()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.IncreaseScoreSound);
    }

    private void OnCollectSticker()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.StickerSound);
    }

    private void OnMatch()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.Match3Sound);
    }

    private void OnPlayerCantRoll()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.CantRollSound);
    }

    private void OnStickerSpawn()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.StickerSpawnSound);
    }

    private void OnCantCollect()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.CantCollectSound);
    }

    private void OnStickerSound()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.StickerStarSound);
    }

    private void OnUpdateSticker()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.UpdateStickerSound);
    }

    #endregion

    #region Helpers

    private void OnRotateHorizontal()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsHelpers.RotateHelperSound);
    }

    private void OnRotateVertical()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsHelpers.RotateHelperSound);
    }

    private void OnCleanCube()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsHelpers.CleanHelperSound);
    }

    private void OnCollect3Emoji()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsHelpers.Collect3Sound);
    }

    #endregion

}
