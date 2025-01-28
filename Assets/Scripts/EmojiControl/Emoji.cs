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
    Angry,
    Joker,
    Poo,
    Pumpkin,
    Zombie,
    Skull,
    Frankeinstein,
    Mummy,
    Clown,
    Love,
    Vampire,


    //Animals

    Bear,
    Cow,
    Crocodile,
    Rhino,
    Narnwhal,
    Chick,

    //Planets
    Planet1,
    Planet2,
    Planet3,

    
    
    // Add more types as needed
}


public class Emoji : MonoBehaviour
{
    public EmojiType EmojiType; // Assign this in the Inspector

    public SpriteRenderer spriteRenderer;
}
