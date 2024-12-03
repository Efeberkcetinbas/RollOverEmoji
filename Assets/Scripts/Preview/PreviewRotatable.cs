using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreviewRotatable : MonoBehaviour
{
    [SerializeField] private InputAction pressed,axis;
    [SerializeField] private float speed;
    [SerializeField] private bool inverted;
    [SerializeField] private GameData gameData;
    [SerializeField] private Collider cubeCollider;

    private Vector2 rotation;
    private bool rotateAllowed;
    private Transform cam;

    private void Awake()
    {
        cam=Camera.main.transform;
        pressed.Enable();
        axis.Enable();
        pressed.performed += _ => {StartCoroutine(Rotate());};
        pressed.canceled += _ => {StartCoroutine(CancelIt()); };
        axis.performed += context => {rotation = context.ReadValue<Vector2>();}; 
    }

    private IEnumerator Rotate()
    {
        if(CheckTouchPreview())
        {
            rotateAllowed=true;
            gameData.CanSwipe=false;
            while(rotateAllowed)
            {
                //apply rotation
                rotation*=speed;
                transform.Rotate(Vector3.up*(inverted?1:-1),rotation.x,Space.World);
                transform.Rotate(cam.right * (inverted? 1:-1),rotation.y,Space.World);
                yield return null;
            }
        }
    }

    private IEnumerator CancelIt()
    {
        yield return null;
        rotateAllowed=false;
        gameData.CanSwipe=true;
    }

    private bool CheckTouchPreview()
    {
        Vector2 touchPosition = Input.mousePosition; // For desktop, this will work with a mouse
        if (Input.touchSupported && Input.touchCount > 0) // For mobile touch input
        {
            touchPosition = Input.GetTouch(0).position;
        }
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider == cubeCollider;
        }
        return false;
    }
}
