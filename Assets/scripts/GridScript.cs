using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public LayerMask unWalkableMask;
    public Vector2 gridWoldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX =Mathf.RoundToInt( gridWoldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWoldSize.y / nodeDiameter);
        CreateGrid();
    } 
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 myBottomLeft=transform.position-Vector3.right*gridWoldSize.x/2 - Vector3.up * gridWoldSize.y / 2;
        for(int x=0;x<gridSizeX;x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = myBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unWalkableMask));
                grid[x, y] = new Node(walkable, new Vector2(worldPoint.x, worldPoint.y));
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWoldSize.x, gridWoldSize.y, 1));
        if(grid!=null)
        {
            foreach(var i in grid)
            {
                Gizmos.color = i.walkable ? Color.green : Color.red;
                Gizmos.DrawWireCube(i.worldPosition, (nodeDiameter-0.01f)*Vector3.one);
            }
        }
    }

}
