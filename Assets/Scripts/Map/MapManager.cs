using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<GameObject> Emojis=new List<GameObject>();

    [SerializeField] private GeneralBuildingManager generalBuildingManager;


    public void SetEmojisToTarget()
    {
        generalBuildingManager.ResetEmojis();
        
        for (int i = 0; i < Emojis.Count; i++)
        {
            generalBuildingManager.AddEmoji(Emojis[i]);
            Emojis[i].SetActive(true);
        }

        generalBuildingManager.TransferEmojisToBuildings();
    }

    //Reset OnNextLevel
}
