using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<GameObject> Emojis=new List<GameObject>();
    
    [SerializeField] private GeneralBuildingManager generalBuildingManager;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnFail,OnFail);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFail,OnFail);
    }

    private void OnFail()
    {
        Emojis.Clear();
    }


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
