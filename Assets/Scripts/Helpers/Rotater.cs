using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation animation
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform cube;

    //Or use Events for rotation
    public void RotateLeftRight()
    {
        StartRotation(Vector3.up, -90,cube);
    }

    public void RotateUpDown()
    {
        StartRotation(Vector3.right, 90,cube);
    }

    private void StartRotation(Vector3 axis, float angle,Transform cube)
    {
        StartCoroutine(RotateCube(axis, angle,cube));
    }

    private IEnumerator RotateCube(Vector3 axis, float angle,Transform cube)
    {
        gameData.CanSwipe = false;

        Quaternion startRotation = cube.rotation;
        Quaternion endRotation = Quaternion.AngleAxis(angle, axis) * startRotation;

        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;
            cube.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
        }

        cube.rotation = endRotation;
        gameData.CanSwipe=true;
    }



}
