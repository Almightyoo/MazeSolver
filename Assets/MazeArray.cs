using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeArray : MonoBehaviour
{
    public LayerMask unwalkable;
    public float maxWallDistance = 2f;
    public Grid grid;
    public FloodArray FloodArray;

    

    private void Update()
    {
        bool left = false, front = false, right = false, back = false;

        
        for (int i = 0; i < 4; i++)
        {
            Vector3 direction = transform.forward;
            if (i == 1) direction = transform.right;
            else if (i == 2) direction = -transform.forward;
            else if (i == 3) direction = -transform.right;

            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            
            if (Physics.Raycast(ray, out hit, maxWallDistance, unwalkable))
            {
                if (i == 0) front = true;
                else if (i == 1) right = true;
                else if (i == 2) back = true;
                else if (i == 3) left = true;

                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red); // Visualize the raycast hit
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * maxWallDistance, Color.green); // Visualize the raycast when it doesn't hit anything
            }
        }
        
        int wallCode = 0;
        if (left) wallCode += 1;
        if (front) wallCode += 2;
        if (right) wallCode += 4;
        if (back) wallCode += 8;


        if (FloodArray.IsUpdateWallTriggerActive())
        {
            Node playerNode = grid.NodeFromWorldPoint(transform.position);
            grid.UpdateWallConfiguration(playerNode.gridX, playerNode.gridY, wallCode);
            FloodArray.ResetUpdateWallTrigger();
        }

        //grid.PrintMazeArray();
    }
    

}


