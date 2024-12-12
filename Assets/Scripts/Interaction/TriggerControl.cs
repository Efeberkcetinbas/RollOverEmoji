using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isInteract=false;


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSetInteract,OnSetInteract);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSetInteract,OnSetInteract);
    }

    private void OnSetInteract()
    {
        isInteract=false;
    }
}
