using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    public GameObject hero;
    public float maxPosX = 30f;

    void Start()
    {
        player = hero.transform;
    }

    private void LateUpdate() 
    {
        CameraMove();
    }

    private void CameraMove()
    {
        Vector3 cameraPosition = transform.position;
        if (cameraPosition.x >= maxPosX)
            return;
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
        transform.position = cameraPosition;
    }
}
