using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private float raycastDistance = 1.1f; // Raycast length for detection
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private Stack<ICommand> commandHistory = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();

    private Vector2 startTouchPosition;
    private const float swipeThreshold = 50f;

    private bool isRolling = false;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (isRolling) return; // Prevent input while rolling

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    Vector2 endTouchPosition = touch.position;
                    Vector2 swipeDelta = endTouchPosition - startTouchPosition;

                    if (swipeDelta.magnitude >= swipeThreshold)
                    {
                        Vector3 direction = GetSwipeDirection(swipeDelta);
                        TryRoll(direction);
                    }
                    break;
            }
        }
    }

    private Vector3 GetSwipeDirection(Vector2 swipeDelta)
    {
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            return swipeDelta.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            return swipeDelta.y > 0 ? Vector3.forward : Vector3.back;
        }
    }

    private void TryRoll(Vector3 direction)
    {
        if (!CanRoll(direction)) return; // Abort if the roll is invalid

        ExecuteCommand(new RollCommand(transform, direction, gridSize, rollSpeed, OnRollComplete));
    }

    private bool CanRoll(Vector3 direction)
    {
        return EnvironmentChecker.IsValidMove(transform.position, direction, raycastDistance, groundLayer, obstacleLayer);
    }

    private void ExecuteCommand(ICommand command)
    {
        isRolling = true; // Lock movement
        command.Execute();
        commandHistory.Push(command);
        redoStack.Clear(); // Clear redo stack after a new action
    }

    private void OnRollComplete()
    {
        isRolling = false; // Unlock movement
    }

    public void Undo()
    {
        if (isRolling || commandHistory.Count == 0) return;

        isRolling = true;
        ICommand lastCommand = commandHistory.Pop();
        lastCommand.Undo();
        redoStack.Push(lastCommand);
        OnRollComplete();
    }

    public void Redo()
    {
        if (isRolling || redoStack.Count == 0) return;

        isRolling = true;
        ICommand commandToRedo = redoStack.Pop();
        commandToRedo.Redo();
        commandHistory.Push(commandToRedo);
        OnRollComplete();
    }
}
