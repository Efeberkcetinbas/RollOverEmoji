using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceTrigger : Obstacleable
{
    [SerializeField] private Image faceImage;
    public EmojiType AssignedEmojiType  = EmojiType.None; // Enum for the face



    public FaceTrigger()
    {
        canStay=false;
        interactionTag="Emoji";
    }

    

    internal override void DoAction(TriggerControl triggered)
    {
        if(!triggered.isInteract)
        {
            if(triggered.spriteRenderer!=null && faceImage.sprite==null)
            {
                faceImage.sprite=triggered.spriteRenderer.sprite;
                triggered.gameObject.SetActive(false);
                triggered.isInteract=true;
                AssignedEmojiType = triggered.GetComponent<Emoji>().EmojiType;
                EventManager.Broadcast(GameEvent.OnCheckMatch);
                Debug.Log($"Assigned {AssignedEmojiType} to face '{gameObject.name}'");
            }
        }
        
    }

    

    public void ResetFace()
    {
        faceImage.sprite = null;  // Reset sprite
        AssignedEmojiType = EmojiType.None; // Reset enum
    }

}
