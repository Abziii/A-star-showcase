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
        gridSizeX =Mathf.RoundToInt( gridWoldSize.x / nodeDiameter);//how much cells in grid for x
        gridSizeY = Mathf.RoundToInt(gridWoldSize.y / nodeDiameter);//how much cells in grid for y
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
                grid[x, y] = new Node(walkable, new Vector2(worldPoint.x, worldPoint.y),x,y);
            }
        }
    }
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {

        float percentX = (-transform.position.x + worldPosition.x / gridWoldSize.x) +0.5f;
        float percentY = (-transform.position.y + worldPosition.y / gridWoldSize.y) + 0.5f;
        int x =Mathf.FloorToInt(Mathf.Clamp((gridSizeX)*percentX,0,gridSizeX-1));
        int y = Mathf.FloorToInt(Mathf.Clamp((gridSizeY) * percentY, 0, gridSizeY - 1));
        return grid[x, y];

    }
    public List<Node>GetNeighbours(Node node)
    {
        List<Node> neigbours = new List<Node>();
        for(int x=-1;x<=1;x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x==0 && y == 0) { continue; }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if(checkX>=0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) { neigbours.Add(grid[checkX, checkY]); }
            }
        }
        return neigbours;
    }
    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWoldSize.x, gridWoldSize.y, 1));

        if(grid!=null)
        {

            foreach (var i in grid)
            {
                Gizmos.color = i.walkable ? Color.green : Color.red;
                if(path!=null && path.Contains(i))
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(i.worldPosition, (nodeDiameter - 0.01f) * Vector3.one);
                }
                else
                {
                    Gizmos.DrawWireCube(i.worldPosition, (nodeDiameter - 0.01f) * Vector3.one);
                }

            }
        }
    }

}
