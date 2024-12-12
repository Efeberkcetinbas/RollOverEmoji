using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public void CleanCube()
    {
        EventManager.Broadcast(GameEvent.OnCleanCube);
    }
}
