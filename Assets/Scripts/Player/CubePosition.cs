using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePosition : MonoBehaviour
{
    public InitialPosConfig initialPosConfig;


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGameStart, OnGameStart);
        EventManager.AddHandler(GameEvent.OnRestartLevel, OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGameStart, OnGameStart);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel, OnRestartLevel);
    }

    private void OnGameStart()
    {
        transform.position = initialPosConfig.GivenPosition;
    }

    private void OnRestartLevel()
    {
        transform.position = initialPosConfig.GivenPosition;
    }
}
