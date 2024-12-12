using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceTrigger : Obstacleable
{   
    [Header("Sprites")]
    [SerializeField] private Image faceImage;
    [SerializeField] private Image UIsprite;
    [SerializeField] private SpriteRenderer previewSprite;


    public EmojiType AssignedEmojiType  = EmojiType.None; // Enum for the face

    private WaitForSeconds waitForSeconds;

    public FaceTrigger()
    {
        canStay=false;
        interactionTag="Emoji";
    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(.5f);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCleanCube,OnCleanCube);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCleanCube,OnCleanCube);
    }

    

    internal override void DoAction(TriggerControl triggered)
    {
        if(!triggered.isInteract)
        {
            if(triggered.spriteRenderer!=null && faceImage.sprite==null)
            {
                faceImage.sprite=triggered.spriteRenderer.sprite;
                UIsprite.sprite=triggered.spriteRenderer.sprite;
                previewSprite.sprite=triggered.spriteRenderer.sprite;
                triggered.gameObject.SetActive(false);
                triggered.isInteract=true;
                AssignedEmojiType = triggered.GetComponent<Emoji>().EmojiType;
                StartCoroutine(CheckMatch());
                Debug.Log($"Assigned {AssignedEmojiType} to face '{gameObject.name}'");
            }
        }
        
    }

    private IEnumerator CheckMatch()
    {
        yield return waitForSeconds;
        EventManager.Broadcast(GameEvent.OnCheckMatch);
    }

    

    public void ResetFace()
    {
        faceImage.sprite = null;  // Reset sprite
        UIsprite.sprite=null;
        previewSprite.sprite=null;
        AssignedEmojiType = EmojiType.None; // Reset enum
    }


    private void OnCleanCube()
    {
        ResetFace();
    }

}
