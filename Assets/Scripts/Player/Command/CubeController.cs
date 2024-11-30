using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private float rollSpeed = 5f;

    private Stack<ICommand> commandHistory = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();

    private Vector2 startTouchPosition;
    private const float swipeThreshold = 50f;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
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
                        ExecuteCommand(new RollCommand(transform, direction, gridSize, rollSpeed));
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

    private void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
        redoStack.Clear(); // Clear redo stack after a new action
    }

    public void Undo()
    {
        if (commandHistory.Count > 0)
        {
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
            redoStack.Push(lastCommand);
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            ICommand commandToRedo = redoStack.Pop();
            commandToRedo.Redo();
            commandHistory.Push(commandToRedo);
        }
    }
}
