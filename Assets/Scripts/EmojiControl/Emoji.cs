using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmojiType
{
    None,  // Default when no emoji is assigned
    Happy,
    Money,
    Sad,
    Cool,
    Devil,
    Joker,
    
    // Add more types as needed
}


public class Emoji : MonoBehaviour
{
    public EmojiType EmojiType; // Assign this in the Inspector
}
