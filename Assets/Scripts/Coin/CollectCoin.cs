using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] private Transform WorldPoint;
    [SerializeField] private GameData gameData;
    private CoinAnimation coinAnimation;

    private int coinCounter;
    private int counter;
    private void Start()
    {
        coinAnimation=FindObjectOfType<CoinAnimation>();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCollectCoin,OnCollectCoin);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCollectCoin,OnCollectCoin);
    }

    private void OnCollectCoin()
    {
        counter=gameData.increaseCoin*gameData.comboCount;
        coinCounter=Mathf.Max(1,counter);
        gameData.increaseScore=coinCounter;
        StartCollectCoin(coinCounter);
    }


    internal void StartCollectCoin(int coincount)
    {
        coinAnimation.PlayAnim(coincount,WorldPoint.position);
    }

}
