using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

struct AstarPriorityNode : IEquatable<AstarPriorityNode> , IComparable<AstarPriorityNode>
{
    public float Heuristic;
    public float AccumulatedDist;
    public Vector3Int index;

    public bool Equals(AstarPriorityNode other)
    {
        return this.Heuristic == other.Heuristic && this.AccumulatedDist == other.AccumulatedDist && this.index == other.index;
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
   

    // Update is called once per frame
    void Update()
    {
        PingPosition();
    }

    public void BuildPath(Vector3 StartPos, Vector3 GoalPos)
    {
        List<AstarPriorityNode> frontier = new List<AstarPriorityNode>();
        frontier.Sort(); //you can sort it for priority.
        //sorting is required after adding neighbors.
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
