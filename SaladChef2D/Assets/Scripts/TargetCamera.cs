using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TargetCamera : MonoBehaviour
{
    public List<Transform> players;

    public Vector3 offset;
    public float smoothTime = 0.2f;
    
    private Vector3 velocity;


    private void LateUpdate()
    {
        if (players.Count == 0)
            return;

        MoveCamera();
        
    }
    

    private void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        //Function to smoothout movement of camera
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        if(players.Count == 1)
        {
            return players[0].position;
        }

        var bounds = new Bounds(players[0].position, Vector3.zero);

        for(int i=0;i<players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }
}
