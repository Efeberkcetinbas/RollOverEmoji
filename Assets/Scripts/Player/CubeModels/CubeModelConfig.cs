using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CubeModelData", menuName = "CubeModel/CubeModelData", order = 0)]
public class CubeModelConfig : ScriptableObject
{
    public Vector3 FacePosition;
    public Vector3 FaceScale;


}
