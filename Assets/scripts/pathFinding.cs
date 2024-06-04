using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFinding : MonoBehaviour
{
    public Transform seeker, target;
    GridScript grid;
    private void Awake()
    {
        grid = GetComponent<GridScript>();//defaulut grid , i need function after to swap between a stars
    }
    private void Update()
    {
        if(seeker==null || target == null) { return; }
        FindPath(seeker.position, target.position);
    }
    void FindPath(Vector3 startPos,Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        List<Node> openSet = new List<Node>();
        HashSet<Node> closetSet = new HashSet<Node>();
        openSet.Add(startNode);
      
        while(openSet.Count>0)
        {
            Node currentNode = openSet[0];
            for(int i=1;i<openSet.Count;i++)
            {
                if(openSet[i].fCost<currentNode.fCost ||( openSet[i].fCost==currentNode.fCost && openSet[i].hCost<currentNode.hCost))//need optimze this line
                {
                    currentNode = openSet[i];
                }

            }
            openSet.Remove(currentNode);
            closetSet.Add(currentNode);
            if(currentNode==targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closetSet.Contains(neighbour))
                {
                    continue;
                }
                int newMovmentCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovmentCostToNeighbour<neighbour.gCost|| !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovmentCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while(currentNode!=startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }


    int GetDistance(Node A,Node B)
    {
        int dstX = Mathf.Abs(A.gridX - B.gridX);

        int dstY = Mathf.Abs(A.gridY - B.gridY);
        if(dstX>dstY)
        {
            return 14 * dstY + 10*(dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }


}
