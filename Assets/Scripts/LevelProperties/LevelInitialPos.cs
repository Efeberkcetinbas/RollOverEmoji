using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitialPos : MonoBehaviour
{
    [SerializeField] private Vector3 levelcubeposition;
    [SerializeField] private InitialPosConfig initialPosConfig;
    [SerializeField] private MapTypes mapType;

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
        initialPosConfig.GivenMapType=mapType;
        EventManager.Broadcast(GameEvent.OnUpdateMapType);
    }
}
