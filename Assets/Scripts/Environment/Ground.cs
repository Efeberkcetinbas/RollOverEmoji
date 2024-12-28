using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ground : MonoBehaviour
{
    [SerializeField] private List<Transform> groundMeshes=new List<Transform>();

    [SerializeField] private int selectedIndex;


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGameStart, OnGameStart);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGameStart, OnGameStart);
    }

    private void Start()
    {
        for (int i = 0; i < groundMeshes.Count; i++)
        {
            groundMeshes[i].gameObject.SetActive(false);
        }
    }
    private void OnGameStart()
    {
        SelectGroundMesh();
    }

    

    private void SelectGroundMesh()
    {
        for (int i = 0; i < groundMeshes.Count; i++)
        {
            groundMeshes[i].gameObject.SetActive(false);
        }

        groundMeshes[selectedIndex].gameObject.SetActive(true);
        groundMeshes[selectedIndex].localScale=Vector3.zero;
        groundMeshes[selectedIndex].DOScale(Vector3.one,0.5f).SetEase(Ease.OutBounce);
    }

    
}
