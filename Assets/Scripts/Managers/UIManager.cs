using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scene Texts")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI levelText;
    

    [Header("Move Texts")]
    [SerializeField] private TextMeshProUGUI moveText;
    [SerializeField] private TextMeshProUGUI totalMoveText;

    [Header("DATA'S")]
    public GameData gameData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.AddHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);

        //Move
        EventManager.AddHandler(GameEvent.OnMoveUI,OnMoveUI);
        EventManager.AddHandler(GameEvent.OnTotalMoveUI,OnTotalMoveUI);
        
        
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);

        EventManager.RemoveHandler(GameEvent.OnMoveUI,OnMoveUI);
        EventManager.RemoveHandler(GameEvent.OnTotalMoveUI,OnTotalMoveUI);
    }

    
    private void OnUIUpdate()
    {
        score.SetText(gameData.score.ToString());
        score.transform.DOScale(new Vector3(1.5f,1.5f,1.5f),0.2f).OnComplete(()=>score.transform.DOScale(new Vector3(1,1f,1f),0.2f));
    }

    private void OnLevelUIUpdate()
    {
        levelText.SetText("LEVEL " + (gameData.levelNumber+1).ToString());
    }
    
    private void OnMoveUI()
    {
        moveText.SetText("Move " + gameData.moveNumber.ToString());
    }

    private void OnTotalMoveUI()
    {
        totalMoveText.SetText(gameData.totalMoveNumber.ToString());
    }

    
    


    
    
}
