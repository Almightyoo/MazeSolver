using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodArray : MonoBehaviour
{
    private int[,] floodArray;
    private int rows, columns;
    public Grid gridScript;
    public Transform player;
    public float speed;
    private float stepDistance = 4.0f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool updateWallTrigger = false;

    private void Start()
    {
        // Initialize floodArray and rows/columns

        rows = gridScript.GetRows();
        columns = gridScript.GetColumns();
        floodArray = new int[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                floodArray[i, j] = -1;
            }
        }

        // Set center cells to 0
        int center = rows / 2;
        floodArray[center - 1, center - 1] = 0;
        floodArray[center - 1, center] = 0;
        floodArray[center, center - 1] = 0;
        floodArray[center, center] = 0;

        

       

        // Print the floodArray
        //PrintFloodArray();
    }

    public void FloodStep(int[,] _mazeArray)
    {
        int[,] mazeArray = _mazeArray;
        Debug.Log("FloodStep method called.");

        //Debug.Log("Initial floodArray:");
        //PrintFloodArray();

        

        for (int num = 0; num < 20; num++)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (floodArray[i, j] == num)
                    {
                        
                        int[] neighbors = GetNeighbours(i, j);
                        UpdateNeighbours(neighbors, i, j, mazeArray);
                        
                    }
                }
            }
        }
        Debug.Log("Updated floodArray:");
        //PrintFloodArray();
        //PrintFloodArray();
    }

    private int[] GetNeighbours(int x, int y)
    {
        int[] neighbors = new int[4];

        if (x - 1 >= 0)
        {
            neighbors[0] = floodArray[x - 1, y];
        }
        else
        {
            neighbors[0] = -2;
        }

        if (x + 1 < rows)
        {
            neighbors[1] = floodArray[x + 1, y];
        }
        else
        {
            neighbors[1] = -2;
        }

        if (y - 1 >= 0)
        {
            neighbors[2] = floodArray[x, y - 1];
        }
        else
        {
            neighbors[2] = -2;
        }

        if (y + 1 < columns)
        {
            neighbors[3] = floodArray[x, y + 1];
        }
        else
        {
            neighbors[3] = -2;
        }

        return neighbors;
    }


    private void UpdateNeighbours(int[] list, int i, int j, int[,] _mazeArray)
    {
        int x = i;
        int y = j;
        int[] U = new int[4];
        int[,] mazeArray = _mazeArray;

        for (int a = 0; a < list.Length; a++)
        {
            if (list[a] == -1)
            {
                U[a] = floodArray[x, y] + 1;
            }
            else
            {
                U[a] = list[a];
            }
        }
        // Define exclusion lists for each direction
        List<int> upExclusions = new List<int> { 2, 3, 6, 7, 10, 11, 14, 15 };
        List<int> downExclusions = new List<int> { 8, 9, 10, 11, 12, 13, 14, 15 };
        List<int> leftExclusions = new List<int> { 1, 3, 5, 7, 9, 11, 13, 15 };
        List<int> rightExclusions = new List<int> { 4, 5, 6, 7, 12, 13, 14, 15 };

        // Update the floodArray based on mazeArray conditions
        if (x - 1 >= 0 && !upExclusions.Contains(mazeArray[x, y]) && !downExclusions.Contains(mazeArray[x - 1, y]))
        {
            //Debug.Log("Updating floodArray[x - 1, y] up");
            floodArray[x - 1, y] = U[0];
        }
        if (x + 1 < rows && !downExclusions.Contains(mazeArray[x, y]) && !upExclusions.Contains(mazeArray[x + 1, y]))
        {
            floodArray[x + 1, y] = U[1];
            //Debug.Log("Updating floodArray[x+1, y] down");
        }
        if (y - 1 >= 0 && !leftExclusions.Contains(mazeArray[x, y]) && !rightExclusions.Contains(mazeArray[x, y - 1]))
        {
            floodArray[x, y - 1] = U[2];
            //Debug.Log("Updating floodArray[x, y-1] left");
        }
        if (y + 1 < columns && !rightExclusions.Contains(mazeArray[x, y]) && !leftExclusions.Contains(mazeArray[x, y + 1]))
        {
            floodArray[x, y + 1] = U[3];
            //Debug.Log("Updating floodArray[x, y+1] right");
        }

    }
        private void PrintFloodArray()
        {
            Debug.Log("Updated flood_array after processing all numbers:");
            for (int i = 0; i < rows; i++)
            {
                string rowStr = "";
                for (int j = 0; j < columns; j++)
                {
                    rowStr += floodArray[i, j].ToString() + " ";
                }
                Debug.Log(rowStr);
            }
        }


    private bool floodStepInProgress = false;

    private IEnumerator DelayedFloodStep(float delay)
    {
        yield return new WaitForSeconds(delay);
        floodStepInProgress = false;
    }

    private void Update()
    {
        if (!floodStepInProgress)
        {
            floodArray = new int[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    floodArray[i, j] = -1;
                }
            }

            int center = rows / 2;
            floodArray[center - 1, center - 1] = 0;
            floodArray[center - 1, center] = 0;
            floodArray[center, center - 1] = 0;
            floodArray[center, center] = 0;

            Debug.Log("FloodArray Update called.");

            int[,] mazeArray = gridScript.GetMazeArray();
            PrintMazeArray(mazeArray);
            floodStepInProgress = true;
            FloodStep(mazeArray);
            int playerX = 0, playerY = 0;
            if (player != null)
            {
                Node playerNode = gridScript.NodeFromWorldPoint(player.position);
                playerX = playerNode.gridX;
                playerY = playerNode.gridY;
            }
            // Get the neighboring nodes
            int[] neighbors = GetNeighbours(rows - playerY-1, playerX);
            Debug.Log(neighbors[0]);
            Debug.Log(neighbors[1]);
            Debug.Log(neighbors[2]);
            Debug.Log(neighbors[3]);
            Debug.Log(floodArray[rows - playerY - 1, playerX]);

            // Find a direction with a flood value 1 less than the current node's flood value
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] == floodArray[rows-playerY-1, playerX] - 1)
                {
                    MoveInDirection(i,playerY,playerX, mazeArray);
                    
                    break;
                    
                }
            }

            StartCoroutine(DelayedFloodStep(0.5f)); // Delay for 0.5 seconds after FloodStep
            PrintFloodArray();
        }
    }


    private void MoveInDirection(int directionIndex, int playerY, int playerX, int[,] mazeArray)
    {
        
        if (isMoving)
            return;

        Vector3 movement = Vector3.zero;
        List<int> upExclusions = new List<int> { 2, 3, 6, 7, 10, 11, 14, 15 };
        List<int> downExclusions = new List<int> { 8, 9, 10, 11, 12, 13, 14, 15 };
        List<int> leftExclusions = new List<int> { 1, 3, 5, 7, 9, 11, 13, 15 };
        List<int> rightExclusions = new List<int> { 4, 5, 6, 7, 12, 13, 14, 15 };

        switch (directionIndex)
        {
            case 0: // Forward
                if (!upExclusions.Contains(mazeArray[rows - playerY - 1, playerX]) &&
                    !downExclusions.Contains(mazeArray[rows - playerY - 2, playerX]))
                {
                    movement = transform.forward;
                    
                }
                break;
            case 1: // Backward
                if (!downExclusions.Contains(mazeArray[rows - playerY - 1, playerX]) &&
                    !upExclusions.Contains(mazeArray[rows - playerY, playerX]))
                {
                    movement = -transform.forward;
                    
                }
                break;
            case 2: // Left
                if (!leftExclusions.Contains(mazeArray[rows - playerY - 1, playerX]) &&
                    !rightExclusions.Contains(mazeArray[rows - playerY - 1, playerX - 1]))
                {
                    movement = -transform.right;
                    //Debug.Log("moving left");
                }
                break;
            case 3: // Right
                if (!rightExclusions.Contains(mazeArray[rows - playerY - 1, playerX]) &&
                    !leftExclusions.Contains(mazeArray[rows - playerY - 1, playerX + 1]))
                {
                    movement = transform.right;
                    //Debug.Log("moving right");
                }
                break;
        }

        targetPosition = player.position + (movement * stepDistance);
        StartCoroutine(MoveToPosition());
    }

    private IEnumerator MoveToPosition()
    {
        isMoving = true;
        while (Vector3.Distance(player.position, targetPosition) > 0.05f)
        {
            player.position = Vector3.MoveTowards(player.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        updateWallTrigger = true;

        isMoving = false;
    }






    public void PrintMazeArray(int [,] _mazeArray)
    {
        int[,] mazeArray = _mazeArray;

        for (int i = 0; i < rows; i++)
        {
            string rowStr = "";
            for (int j = 0; j < columns; j++)
            {
                rowStr += mazeArray[i, j].ToString() + " ";
            }
            Debug.Log(rowStr);
        }
    }
    public void ResetUpdateWallTrigger()
    {
        updateWallTrigger = false;
    }

    public bool IsUpdateWallTriggerActive()
    {
        return updateWallTrigger;
    }

}


