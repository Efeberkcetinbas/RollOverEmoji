using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform targetPos;
    [SerializeField] private GameData gameData;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Cube"))
        {
            gameData.CanSwipe=false;
            StartCoroutine(TeleportInstant(other.transform));
        }
    }

    private IEnumerator TeleportInstant(Transform cube)
    {
        yield return new WaitForSeconds(1);
        cube.position=targetPos.position;
        gameData.CanSwipe = true;
    }
}
