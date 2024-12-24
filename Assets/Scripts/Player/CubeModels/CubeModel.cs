using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeModel : MonoBehaviour
{
    [SerializeField] private List<FaceTrigger> faceTriggers=new List<FaceTrigger>();
    [SerializeField] private List<CubeModelConfig> cubeModelConfigs=new List<CubeModelConfig>();


    private void Start()
    {
        AssignFaceSpriteScaleandPos();
    }


    private void AssignFaceSpriteScaleandPos()
    {
        for (int i = 0; i < faceTriggers.Count; i++)
        {
            faceTriggers[i].faceImage.transform.localPosition=cubeModelConfigs[i].FacePosition;
            faceTriggers[i].faceImage.transform.localScale=cubeModelConfigs[i].FaceScale;   
        }
    }
}
