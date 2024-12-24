using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CubePositionData", menuName = "CubePositionData/CubePosition", order = 0)]
public class InitialPosConfig : ScriptableObject
{
    public Vector3 GivenPosition;
}
