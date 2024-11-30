using System.Collections;
using UnityEngine;

public class RollCommand : ICommand
{
    private readonly Transform cube;
    private readonly Vector3 direction;
    private readonly float gridSize;
    private readonly float rollSpeed;

    private Vector3 previousPosition;
    private Quaternion previousRotation;

    public RollCommand(Transform cube, Vector3 direction, float gridSize, float rollSpeed)
    {
        this.cube = cube;
        this.direction = direction;
        this.gridSize = gridSize;
        this.rollSpeed = rollSpeed;
    }

    public void Execute()
    {
        SaveState();
        Vector3 pivot = cube.position + (Vector3.down + direction) * (gridSize / 2f);
        Vector3 axis = Vector3.Cross(Vector3.up, direction);
        CoroutineManager.Instance.StartCoroutine(RollRoutine(pivot, axis));
    }

    public void Undo()
    {
        RestoreState();
    }

    public void Redo()
    {
        Execute();
    }

    private IEnumerator RollRoutine(Vector3 pivot, Vector3 axis)
    {
        float angle = 0f;
        while (angle < 90f)
        {
            float step = rollSpeed * Time.deltaTime;
            step = Mathf.Min(step, 90f - angle);
            cube.RotateAround(pivot, axis, step);
            angle += step;
            yield return null;
        }

        SnapToGrid();
    }

    private void SnapToGrid()
    {
        cube.position = new Vector3(
            Mathf.Round(cube.position.x / gridSize) * gridSize,
            Mathf.Round(cube.position.y / gridSize) * gridSize,
            Mathf.Round(cube.position.z / gridSize) * gridSize
        );
    }

    private void SaveState()
    {
        previousPosition = cube.position;
        previousRotation = cube.rotation;
    }

    private void RestoreState()
    {
        cube.position = previousPosition;
        cube.rotation = previousRotation;
    }
}
