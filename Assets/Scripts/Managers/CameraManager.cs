using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cm;


    [Header("Rolling")]
    [SerializeField] private float shakeTimeRoll = 0.5f;
    [SerializeField] private float amplitudeGainRoll=1;
    [SerializeField] private float frequencyGainRoll=1;

    [Header("Match")]
    [SerializeField] private float shakeTimeMatch = 0.5f;
    [SerializeField] private float amplitudeGainMatch=1;
    [SerializeField] private float frequencyGainMatch=1;
    private CinemachineBasicMultiChannelPerlin noise;




    
    

    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnCubeRollingEnd,OnCubeRollingEnd);
        EventManager.AddHandler(GameEvent.OnMatch,OnMatch);
    }

    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnCubeRollingEnd,OnCubeRollingEnd);
        EventManager.RemoveHandler(GameEvent.OnMatch,OnMatch);

    }


   

   

    private void OnNextLevel()
    {

    }

    

    private void OnCubeRollingEnd()
    {
        Noise(amplitudeGainRoll,frequencyGainRoll,shakeTimeRoll);
    }
    
    private void OnMatch()
    {
        Noise(amplitudeGainMatch,frequencyGainMatch,shakeTimeMatch);
    }
    
    

    private void Start() 
    {
        noise=cm.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        if(noise == null)
            Debug.LogError("No MultiChannelPerlin on the virtual camera.", this);
        else
            Debug.Log($"Noise Component: {noise}");

        OnNextLevel();

    }

    

    private void Noise(float amplitudeGain,float frequencyGain,float shakeTime) 
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
        StartCoroutine(ResetNoise(shakeTime));    
    }

    private IEnumerator ResetNoise(float duration)
    {
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;    
    }
    public void ChangeFieldOfView(float fieldOfView, float duration = 1)
    {
        DOTween.To(() => cm.m_Lens.FieldOfView, x => cm.m_Lens.FieldOfView = x, fieldOfView, duration);
    }

    

    public void ChangeFieldOfViewHit(float newFieldOfView, float oldFieldOfView, float duration = 1)
    {
        DOTween.To(() => cm.m_Lens.FieldOfView, x => cm.m_Lens.FieldOfView = x, newFieldOfView, duration).OnComplete(()=>{
            DOTween.To(() => cm.m_Lens.FieldOfView, x => cm.m_Lens.FieldOfView = x, oldFieldOfView, duration);
        });
    }

    public void ChangeFollow(Transform Ball)
    {
        cm.m_Follow=Ball;
    }

    public void ChangeLookAt(Transform target)
    {
        cm.m_LookAt=target;
    }

    private void ChangePriority(int val)
    {
        cm.m_Priority=val;
    }


}
