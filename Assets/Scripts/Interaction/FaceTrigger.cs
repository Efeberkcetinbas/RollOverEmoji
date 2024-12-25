using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FaceTrigger : Obstacleable
{   
    [Header("Sprites")]
    public SpriteRenderer faceImage;
    [SerializeField] private Image UIsprite;
    [SerializeField] private SpriteRenderer previewSprite;
    [SerializeField] private CubeProp cubeProp;


    public EmojiType AssignedEmojiType  = EmojiType.None; // Enum for the face

    private WaitForSeconds waitForSeconds;

    private GameObject tempEmoji;



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
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCleanCube,OnCleanCube);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    

    internal override void DoAction(TriggerControl triggered)
    {
        if(!triggered.isInteract)
        {
            if(triggered.spriteRenderer!=null && faceImage.sprite==null)
            {
                faceImage.sprite=triggered.spriteRenderer.sprite;

                //UI
                UIsprite.transform.localScale=Vector3.zero;
                UIsprite.sprite=triggered.spriteRenderer.sprite;
                UIsprite.transform.DOScale(Vector3.one,0.5f).SetEase(Ease.OutBounce);
                
                previewSprite.sprite=triggered.spriteRenderer.sprite;

                ParticleSystemRenderer renderer = cubeProp.StickerParticle.GetComponent<ParticleSystemRenderer>();
                if (renderer != null && renderer.material != null && triggered.spriteRenderer.sprite != null)
                {
                    // Get the texture from the SpriteRenderer's sprite
                    Texture2D spriteTexture = triggered.spriteRenderer.sprite.texture;

                    if (spriteTexture != null)
                    {
                        // Assign the texture to the particle system's material
                        renderer.material.mainTexture = spriteTexture;
                        cubeProp.StickerParticle.Play();
    
                        Debug.Log("Particle system texture successfully updated.");
                    }
                    else
                    {
                        Debug.LogWarning("The sprite does not have a valid texture.");
                    }
                }
                else
                {
                    Debug.LogError("Particle system renderer, material, or triggered sprite is missing.");
                }

                tempEmoji=triggered.gameObject;
                triggered.gameObject.SetActive(false);
                triggered.isInteract=true;
                AssignedEmojiType = triggered.GetComponent<Emoji>().EmojiType;
                EventManager.Broadcast(GameEvent.OnCollectSticker);
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
        tempEmoji=null;
        AssignedEmojiType = EmojiType.None; // Reset enum
    }

    private void OnRestartLevel()
    {
        ResetFace();
    }


    private void OnCleanCube()
    {
        if(AssignedEmojiType!=EmojiType.None)
        {
            tempEmoji.SetActive(true);
            tempEmoji.GetComponent<TriggerControl>().isInteract=false;
        }

        ResetFace();

    }

}
