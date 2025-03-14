using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;


    private WaitForSeconds waitForSeconds,waitForSecondsFail;

    public int LevelMatchNeededNumber;


    private void Awake() 
    {
        ClearData();
        gameData.starAmount=PlayerPrefs.GetInt("Star",0);

    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(2);
        waitForSecondsFail=new WaitForSeconds(1);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnFail,OnFail);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnFail,OnFail);

    }

    
    

    private void OnNextLevel()
    {
        ClearData();
        EventManager.Broadcast(GameEvent.OnMoveUI);

        
    }

    private void OnRestartLevel()
    {
        ClearData();
        EventManager.Broadcast(GameEvent.OnMoveUI);


    }

    
    private void ClearData()
    {
        gameData.isGameEnd=true;
        gameData.CanSwipe=false;

        gameData.moveNumber=0;
    }
   


   

    private void OnSuccess()
    {
        StartCoroutine(OpenSuccess());
    }

    private void OnFail()
    {
        gameData.isGameEnd=true;
        StartCoroutine(OpenFail());
    }
    private IEnumerator OpenSuccess()
    {
        yield return waitForSeconds;
        OpenSuccessPanel();
    }


    private void OpenSuccessPanel()
    {
        EventManager.Broadcast(GameEvent.OnSuccessUI);
    }

    private IEnumerator OpenFail()
    {
        yield return waitForSecondsFail;
        OpenFailPanel();
    }


    private void OpenFailPanel()
    {
        //effektif
        EventManager.Broadcast(GameEvent.OnFailUI);
    }



    
}
