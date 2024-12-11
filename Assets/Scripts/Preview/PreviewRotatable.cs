using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewRotatable : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation animation
    [SerializeField] private Collider cubeCollider,previewCubeCollider;
    [SerializeField] private GameData gameData;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    
    private bool isRotating = false;

    /*private void Update()
    {
        HandleSwipe();
    }*/

    private void HandleSwipe()
    {
        //if (isRotating || !CheckTouchPreview()) return;
        if (isRotating) return;
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            CheckTouchPreview(touch.position);
        }

        if(gameData.isPreviewCube)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startTouchPosition = touch.position;
                        break;

                    case TouchPhase.Ended:
                        endTouchPosition = touch.position;
                        DetectSwipe();
                        break;
                }
            }
        }
        else
            return;
    }

    private void DetectSwipe()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;

        if (swipeDirection.magnitude < 1f) return; // Ignore small swipes (adjust threshold if needed)

        swipeDirection.Normalize();

        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            // Horizontal swipe
            if (swipeDirection.x > 0) StartRotation(Vector3.up, -90); // Swipe right
            else StartRotation(Vector3.up, 90); // Swipe left
        }
        else
        {
            // Vertical swipe
            if (swipeDirection.y > 0) StartRotation(Vector3.right, 90); // Swipe up
            else StartRotation(Vector3.right, -90); // Swipe down
        }
    }

    private void StartRotation(Vector3 axis, float angle)
    {
        if (isRotating) return;

        StartCoroutine(RotateCube(axis, angle));
    }

    private IEnumerator RotateCube(Vector3 axis, float angle)
    {
        isRotating = true;
        gameData.CanSwipe = false;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.AngleAxis(angle, axis) * startRotation;

        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;
        //gameData.CanSwipe = true;
    }

    private void CheckTouchPreview(Vector2 touchPosition)
    {
        float screenHeight = Screen.height;
        float y = touchPosition.y;

        if (y > screenHeight * (2f / 3f))
        {
            // Top Section
            gameData.isPreviewCube = true;
        }
        else if (y > screenHeight * (1f / 3f))
        {
            // Middle Section
            gameData.isPreviewCube = false;
            gameData.CanSwipe=true;
        }
        else
        {
            // Bottom Section
            gameData.isPreviewCube = false;
            // Optional: You can handle incremental section logic here
        }
    }

    private IEnumerator SetPreview(bool val)
    {
        yield return new WaitForSeconds(0.5f);
        gameData.isPreviewCube=val;
    }
}
