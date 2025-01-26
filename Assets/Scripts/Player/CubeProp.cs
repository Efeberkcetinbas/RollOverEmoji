using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeProp : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem StickerParticle;
    
    [SerializeField] private ParticleSystem rollEndParticle;
    [SerializeField] private ParticleSystem matchParticle;

    [Header("Collect")]
    [SerializeField] private MeshRenderer cubeMesh;

    private WaitForSeconds waitForSeconds;

    private Color defcolor;


    private void Start()
    {
        waitForSeconds = new WaitForSeconds(.1f);
        defcolor=cubeMesh.material.color;
    }


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatch, OnMatch);
        EventManager.AddHandler(GameEvent.OnCubeRollingEnd, OnCubeRollingEnd);
        EventManager.AddHandler(GameEvent.OnCubeRollingStart,OnCubeRollingStart);
        EventManager.AddHandler(GameEvent.OnCantCollect, OnCantCollect);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatch, OnMatch);
        EventManager.RemoveHandler(GameEvent.OnCubeRollingEnd, OnCubeRollingEnd);
        EventManager.RemoveHandler(GameEvent.OnCubeRollingStart, OnCubeRollingStart);
        EventManager.RemoveHandler(GameEvent.OnCantCollect, OnCantCollect);

    }

    private void OnMatch()
    {
        matchParticle.transform.position=transform.position+new Vector3(0,2,0);
        matchParticle.Play();
    }

    private void OnCubeRollingEnd()
    {
        rollEndParticle.Play();

    }

    private void OnCubeRollingStart()
    {
        transform.DOScale(Vector3.one*0.85f,0.25f).SetEase(Ease.InBounce).OnComplete(()=>{
            transform.DOScale(Vector3.one,0.25f).SetEase(Ease.OutBounce);
        });
    }

    private void OnCantCollect()
    {
        StartCoroutine(SetColorEffect());
    }

    private IEnumerator SetColorEffect()
    {
        cubeMesh.material.color=Color.white;
        yield return waitForSeconds;
        cubeMesh.material.color=defcolor;
    }
}
