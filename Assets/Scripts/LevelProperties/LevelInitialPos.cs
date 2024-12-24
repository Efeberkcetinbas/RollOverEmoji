using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitialPos : MonoBehaviour
{
    [SerializeField] private Vector3 levelcubeposition;
    [SerializeField] InitialPosConfig initialPosConfig;

    private void Start()
    {
        OnNextLevel();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    private void OnNextLevel()
    {
        initialPosConfig.GivenPosition=levelcubeposition;
    }
}
