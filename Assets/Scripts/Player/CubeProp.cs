using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeProp : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem StickerParticle;
    
    [SerializeField] private ParticleSystem rollEndParticle;
    [SerializeField] private ParticleSystem matchParticle;


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatch, OnMatch);
        EventManager.AddHandler(GameEvent.OnCubeRollingEnd, OnCubeRollingEnd);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatch, OnMatch);
        EventManager.RemoveHandler(GameEvent.OnCubeRollingEnd, OnCubeRollingEnd);
    }

    private void OnMatch()
    {
        matchParticle.Play();
    }

    private void OnCubeRollingEnd()
    {
        rollEndParticle.Play();
    }
}
