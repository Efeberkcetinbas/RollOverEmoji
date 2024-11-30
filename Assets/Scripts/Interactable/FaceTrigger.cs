using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceTrigger : Obstacleable
{
    [SerializeField] private Image faceImage;



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
                triggered.isInteract=true;
            }
        }
        
    }

    internal override void StopAction(TriggerControl triggered)
    {
        if(triggered.isInteract)
            triggered.gameObject.SetActive(false);
    }

}
