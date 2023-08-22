using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkable;
    Node[,] grid;
    public Transform player;
    int[,] mazeArray;

    float nodeDiameter;
    int rows;
    int columns;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        rows = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        columns = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        CreateGrid();
        mazeArray = new int[columns, rows];
        Debug.Log("Rows: " + rows + ", Columns: " + columns);
    }


    public void CreateGrid()
    {
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        grid = new Node[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkable));
                grid[x, y] = new Node(walkable, worldPoint, x, y);

            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((columns - 1) * percentX);
        int y = Mathf.RoundToInt((rows - 1) * percentY);
        return grid[x, y];
    }

    // Function to update wall configuration in the mazeArray
    public void UpdateWallConfiguration(int x, int y, int wallCode)
    {
        //Debug.Log("Updating mazeAArray at (" + x + ", " + y + ") with wallCode: " + wallCode);
        mazeArray[rows - y-1, x] = wallCode;
    }

    // Function to print the mazeArray to the console
    public void PrintMazeArray()
    {

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

    

    public int[,] GetMazeArray()
    {
        return mazeArray;
    }

    public int GetRows()
    {
        return rows;
    }

    public int GetColumns()
    {
        return columns;
    }
}