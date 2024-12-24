using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject 
{

    public int score;
    public int increaseScore;
    public int increaseCoin;
    public int decreaseScore;
    public int levelIndex;
    public int levelNumber;
    public int levelEmojiCount;
    public int comboCount;

    //Move

    //Set 0 when Restart or NextLevel
    public int moveNumber;
    public int totalMoveNumber;
    

    public float elapsedTime;
    public float currentInterval;

    public bool isGameEnd=false;
    public bool CanSwipe=false;
    public bool isPreviewCube=false;

}
