using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum MapTypes
{
    EmojiWorld,
    AnimalMap,
    CardMap,
}

[Serializable]
public class ModelsDueToMap
{
    public string Name;
    public MapTypes mapTypes;

    [SerializeField] private List<FaceTrigger> faceTriggers = new List<FaceTrigger>();
    [SerializeField] private List<CubeModelConfig> cubeModelConfigs = new List<CubeModelConfig>();
    

    // Properties to access the private fields
    public List<FaceTrigger> FaceTriggers => faceTriggers;
    public List<CubeModelConfig> CubeModelConfigs => cubeModelConfigs;
}
public class CubeModel : MonoBehaviour
{

    public MapTypes currentMapType; // Assign this in the Inspector or dynamically at runtime
    public List<ModelsDueToMap> modelsDueToMaps = new List<ModelsDueToMap>();
    [SerializeField] private InitialPosConfig initialPosConfig;

    private void Start()
    {
        AssignFaceSpriteScaleandPos();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUpdateMapType,OnUpdateMapType);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUpdateMapType,OnUpdateMapType);
    }

    private void AssignFaceSpriteScaleandPos()
    {
        // Find the ModelsDueToMap object that matches the current MapType
        ModelsDueToMap selectedModel = modelsDueToMaps.Find(model => model.mapTypes == currentMapType);

        if (selectedModel == null)
        {
            Debug.LogWarning($"No model configuration found for MapType: {currentMapType}");
            return;
        }

        // Ensure lists have the same count before proceeding
        if (selectedModel.FaceTriggers.Count != selectedModel.CubeModelConfigs.Count)
        {
            Debug.LogError("FaceTriggers and CubeModelConfigs lists must have the same count.");
            return;
        }

        // Assign FaceScale and FacePosition to each FaceTrigger
        for (int i = 0; i < selectedModel.FaceTriggers.Count; i++)
        {
            FaceTrigger faceTrigger = selectedModel.FaceTriggers[i];
            CubeModelConfig config = selectedModel.CubeModelConfigs[i];

            faceTrigger.faceImage.transform.localPosition = config.FacePosition;
            faceTrigger.faceImage.transform.localScale = config.FaceScale;
        }
    }

    private void OnUpdateMapType()
    {
        currentMapType=initialPosConfig.GivenMapType;
        AssignFaceSpriteScaleandPos();
    }
}
