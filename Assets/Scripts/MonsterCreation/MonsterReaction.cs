using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterReaction : MonoBehaviour
{
    [SerializeField] private GameObject arm;
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCubeRollingStart,OnCubeRollingStart);
        EventManager.AddHandler(GameEvent.OnMatch,OnMatch);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCubeRollingStart,OnCubeRollingStart);
        EventManager.RemoveHandler(GameEvent.OnMatch,OnMatch);
    }



    private void OnCubeRollingStart()
    {
        arm.transform.DOScaleY(1.25f,0.25f).OnComplete(()=>arm.transform.DOScaleY(1,0.25f));
    }

    private void OnMatch()
    {
        arm.transform.DORotate(new Vector3(0,0,22.5f),0.25f).OnComplete(()=>arm.transform.DORotate(Vector3.zero,0.25f));
    }
}
