using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public GameData gameData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnIncreaseScore, OnIncreaseScore);
        EventManager.AddHandler(GameEvent.OnSetTotalMove,OnSetTotalMove);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnIncreaseScore, OnIncreaseScore);
        EventManager.RemoveHandler(GameEvent.OnSetTotalMove,OnSetTotalMove);
    }
    private void OnIncreaseScore()
    {
        DOTween.To(GetScore,ChangeScore,gameData.score+gameData.increaseScore,.5f).OnUpdate(UpdateUI);
    }

    private int GetScore()
    {
        return gameData.score;
    }

    private void ChangeScore(int value)
    {
        gameData.score=value;
    }

    private void UpdateUI()
    {
        EventManager.Broadcast(GameEvent.OnUIUpdate);
    }


    #region Total Move Number
    private int GetMove()
    {
        return gameData.totalMoveNumber;
    }

    private void ChangeMove(int value)
    {
        gameData.totalMoveNumber=value;
    }

    private void UpdateMoveUI()
    {
        EventManager.Broadcast(GameEvent.OnTotalMoveUI);
    }

    private void OnSetTotalMove()
    {
        DOTween.To(GetMove,ChangeMove,gameData.totalMoveNumber+gameData.moveNumber,1f).OnUpdate(UpdateMoveUI);
    }


    #endregion






}
