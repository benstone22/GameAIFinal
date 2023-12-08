using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine;

struct AstarPriorityNode : IEquatable<AstarPriorityNode> , IComparable<AstarPriorityNode>
{
    public float Heuristic;
    public float AccumulatedDist;
    public Vector3Int Index;
    

    public bool Equals(AstarPriorityNode other)
    {
        return this.Heuristic == other.Heuristic && this.AccumulatedDist == other.AccumulatedDist && this.Index == other.Index;
    }

    int IComparable<AstarPriorityNode>.CompareTo(AstarPriorityNode other)
    {
        return Mathf.RoundToInt((this.Heuristic + this.AccumulatedDist - other.Heuristic + other.AccumulatedDist)*10000000);
    }

    

}



//Making own priority queue for frontier so we can astar.

public class AstarBehavior : SpacialQuatization
{
    [SerializeField] public Vector3 targetPos = new Vector3(20.0f, 9.0f, 142.0f);
    [SerializeField] public Vector3 defaultTarget = new Vector3(20.0f, 9.0f, 142.0f);
    public static int manhattanDist(Vector3 source, Vector3 target)
    {
        var qs = Quantize(source);
        var qt = Quantize(target);
        return Mathf.Abs(qs.x - qt.x) + Mathf.Abs(qs.y - qt.y) + Mathf.Abs(qs.y - qt.y);
    }
    
    public List<Vector3Int> getVisitableNeighbors(Vector3Int cur,Dictionary<Vector3Int, bool> visited)
    {
        //forward backward left right
        List<Vector3Int> canidates = new List<Vector3Int>();
        //forward
        Vector3Int forward = cur;
        forward.z += 1;
        canidates.Add(forward);
        //backward
        Vector3Int backward = cur;
        forward.z -= 1;
        canidates.Add(backward);
        //left
        Vector3Int left = cur;
        forward.x -= 1;
        canidates.Add(left);
        //right
        Vector3Int right = cur;
        forward.x += 1;
        canidates.Add(right);

        List<Vector3Int> ret=new List<Vector3Int>();
        for (int i = 0; i < visited.Count; i++)
        {
            if (!visited.ContainsKey(canidates[i]))
            {
                ret.Add(canidates[i]);
            }
        }

        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int goal = new Vector3Int(0,0,0);
        
        makePath(goal);
    }

    public void BuildPath(Vector3 StartPos, Vector3 GoalPos)
    {
        List<AstarPriorityNode> frontier = new List<AstarPriorityNode>();
        frontier.Sort(); //you can sort it for priority.
        //sorting is required after adding neighbors.
    }

    public List<Vector3Int> makePath(Vector3Int goal)
    {
        Vector3Int backtrack = goal; //this is used for the buildpath
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        List<Vector3Int> frontier = new List<Vector3Int>();
        Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>();
        Vector3Int start = new Vector3Int();
        start = transform.position.ConvertTo<Vector3Int>();
        
        
        frontier.Add(start);
        
        
        while (!(frontier.Count==0))
        {
            Vector3Int cur = frontier[0];
            frontier.RemoveAt(0);
            visited.Add(cur,true);
            if (cur.Equals(goal))
            {
                break;
            }
            List<Vector3Int> neighbors = getVisitableNeighbors(cur, visited);
            foreach (Vector3Int neighbor in neighbors)
            {
                cameFrom[neighbor] = cur;
                frontier.Add(neighbor);
            }

            
        }

        List<Vector3Int> path = new List<Vector3Int>();
        while (backtrack!= transform.position.ConvertTo<Vector3Int>())
        {
            path.Append(backtrack);
            backtrack = cameFrom[backtrack];
        }
        return path;
    }


    public void PingPosition() //shifted to a seperate function (may need to adapt this to Raycasting because of 3D
    {

        
        Vector3 mousepos = Input.mousePosition;

        print(Quantize(this.transform.position));
        if (Input.GetMouseButtonDown(0))
        {
            targetPos = mousepos;
            Debug.Log(targetPos);
            Debug.DrawLine(transform.position, mousepos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(targetPos);
            targetPos = defaultTarget;
        }

        Debug.Log(manhattanDist(this.transform.position, targetPos));
    }
}
