﻿using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
 
    public float distance = -10f;
 
    public float height = 0f;
 
    public float damping = 5f;
 
    public float maxX = 15f;
    public float minX = -15f;
    public float maxY = 25f;
    public float minY = -25f;
  
 
    void FixedUpdate () {
 
        var wantedPosition = target.TransformPoint(0, height, distance);
 
        wantedPosition.x = (wantedPosition.x < minX) ? minX : wantedPosition.x;
        wantedPosition.x = (wantedPosition.x > maxX) ? maxX : wantedPosition.x;
 
        wantedPosition.y = (wantedPosition.y < minY) ? minY : wantedPosition.y;
        wantedPosition.y = (wantedPosition.y > maxY) ? maxY : wantedPosition.y;

        transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);
    }
}
