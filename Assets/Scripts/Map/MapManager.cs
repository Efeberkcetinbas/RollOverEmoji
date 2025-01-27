using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<GameObject> Emojis=new List<GameObject>();
    internal GameObject temmMap;
    
    [SerializeField] private GeneralBuildingManager generalBuildingManager;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnFail,OnFail);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFail,OnFail);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
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

    private void OnNextLevel()
    {
        Destroy(temmMap);
    }

    

    //Reset OnNextLevel
}
