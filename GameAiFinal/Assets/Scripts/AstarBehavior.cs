using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine;

struct AstarPriorityNode : IEquatable<AstarPriorityNode>, IComparable<AstarPriorityNode> 
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




public class AstarBehavior : SpacialQuatization
{
    public TerrainCollider terrainCollider;
    public Camera cam;
    
    [SerializeField] GameObject Player;
    [SerializeField] public float directionTowardsNextPosForce = 0.1f;
    [SerializeField] public Vector3 targetPos;

    

    
    //private Dictionary<Vector2Int, List<Vector3Int>> SpatialGrid = new Dictionary<Vector2Int, List<Vector3Int>>();
    //private Quadtree<Vector3Int> qTree;
    public void Start()
    {
        
        
    }
    public static int manhattanDist(Vector3 source, Vector3 target)
    {
        
        var qs = Quantize(source);
        var qt = Quantize(target);
        return Mathf.Abs(qs.x - qt.x) + Mathf.Abs(qs.y - qt.y) + Mathf.Abs(qs.y - qt.y);
    }
    
    public List<Vector3Int> getVisitableNeighbors(Vector3Int cur,HashSet<Vector3Int> visited)
    {
        //forward backward left right
        List<Vector3Int> canidates = new List<Vector3Int>();
        //forward
        Vector3Int forward = cur;
        forward.z += 1;
        canidates.Add(forward);
        //backward
        Vector3Int backward = cur;
        backward.z -= 1;
        canidates.Add(backward);
        //left
        Vector3Int left = cur;
        left.x -= 1;
        canidates.Add(left);
        //right
        Vector3Int right = cur;
        right.x += 1;
        canidates.Add(right);

        List<Vector3Int> ret=new List<Vector3Int>();
        for (int i = 0; i <canidates.Count; i++)
        {
            
            if (!visited.Contains(canidates[i]))
            {
                ret.Add(canidates[i]);
            }
        }

        return ret;
    }

    public Vector3Int findMin(HashSet<Vector3Int> frontier, Dictionary<Vector3Int, float> distKeeper)
    {
        Vector3Int minPos = new Vector3Int();
        float minDist = float.MaxValue;
        foreach (var pos in frontier)
        {
            if (distKeeper.TryGetValue(pos,out float dist)&&dist<minDist)
            {
                minPos = pos;
                minDist = dist;
            }
        }

        return minPos;

    }
    

    public List<Vector3Int> findPath(Vector3Int goal)
    {

        HashSet<Vector3Int> frontier = new HashSet<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> distKeeper = new Dictionary<Vector3Int, float>();
        Dictionary<Vector3Int, float> heuKeeper = new Dictionary<Vector3Int, float>();

       
        Vector3Int start = new Vector3Int();
        start = Quantize(transform.position);


        frontier.Add(start);
        distKeeper[start] = 0;
        heuKeeper[start] = manhattanDist(Dequantize(start), Dequantize(goal));
        
        int loopbreak = 0;
        
        Vector3Int cur = start;
        while (frontier.Count>0)
        {
            cur = findMin(frontier, distKeeper);
            if (cur.Equals(goal))
            {
                Debug.Log(cur+ " "+goal+" Done!");
                return makePath(cFrom,cur);
            }
            frontier.Remove(cur);
            visited.Add(cur);
           
            List<Vector3Int> neighbors = getVisitableNeighbors(cur, visited);
            
            float tempAccDist = distKeeper[cur] + 1;
            
            for (int i = 0; i < neighbors.Count; i++)
            {
                
                Vector3Int neighbor = neighbors[i];
                if (visited.Contains(neighbor))
                {
                    continue;
                }
                if (!frontier.Contains(neighbor)||tempAccDist<distKeeper[neighbor])
                {
                    cFrom[neighbor] = cur;
                    
                    distKeeper[neighbor] = tempAccDist;
                    heuKeeper[neighbor] = distKeeper[neighbor] + manhattanDist(Dequantize(neighbor), Dequantize(goal));
                    //frontier.Add(neighbor);
                    if (!frontier.Contains(neighbor))
                    {
                        frontier.Add(neighbor);
                    }

                }
            }
                
                
                
            
            
            loopbreak++;
            /*if (loopbreak>1000)
            {
                
                Debug.Log("this is infinetly screwed");
                break;
                
            }*/
        }


        return null;

    }

    static List<Vector3Int> makePath(Dictionary<Vector3Int, Vector3Int> cFrom, Vector3Int cur)
    {
        int loopbreak = 0;
        List<Vector3Int> path = new List<Vector3Int>(){cur};
        

        while (cFrom.TryGetValue(cur,out Vector3Int prev))
        {
            cur = prev;
            path.Insert(0,cur);
            
            
            loopbreak++;
            
            if (loopbreak>1000)
            {
                Debug.Log("this is infinetly screwed");
                break;
                
            }
        }
        

        for (int i = 0; i < path.Count-1; i++)
        {
            
            Debug.DrawLine(path[i], path[i + 1], Color.cyan, 10000f);
            Debug.Log("position in path index: " + i+ " is "+ path[i]);
            
        }
        //Debug.DrawLine(path[path.Count-1],Dequantize(goal),Color.cyan, 10f);
        
        return path; 
    }
    void PrintPath(List<Vector3Int> path)
    {
        if (path != null)
        {
            Debug.Log("Path found:");
            foreach (Vector3Int pos in path)
            {
                Debug.Log($"({pos.x}, {pos.z})");
            }
        }
        else
        {
            Debug.Log("No path found.");
        }
    }


    public void PingPosition() //shifted to a seperate function (may need to adapt this to Raycasting because of 3D
    {

        RaycastHit hit;
        Vector3 mousepos = Input.mousePosition;
        Ray mouseRay = cam.ScreenPointToRay(mousepos);
        Vector3 mouseHit;
        
        if (terrainCollider.Raycast(mouseRay, out hit,70))
        {
            
            mouseHit = hit.point;
            
            Debug.Log("mouse pos: " + mouseHit);
            print(Quantize(this.transform.position));
            

            mouseHit.y = transform.position.y;
            targetPos = mouseHit;
            Debug.Log("Target Position: " + targetPos);
            Debug.DrawLine(transform.position, mouseHit,Color.red,100f);
            

            Debug.Log("Manhattan distance from current position at ping to target position: "+ manhattanDist(this.transform.position, targetPos));

        }

        
    }
    
    
    public void Update()
    {

        Vector3Int quantpos = Quantize(transform.position);
        
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            
            GetComponent<BuddyFlocking>().enabled = false;
            
            PingPosition(); 
            Vector3Int quantTargetPos = Quantize(targetPos);
            targetPos.y = quantpos.y;
            
            var path = findPath(quantTargetPos);
            
            if (path.Count>3)
            {
                var destination = path[2];
                var direction = (destination - transform.position).normalized;
                //rb.velocity+=directionTowardsNextPosForce*direction;
                rb.AddForce(directionTowardsNextPosForce*direction);
            }
            
        }

        

        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<BuddyFlocking>().enabled = true;
            GetComponent<AstarBehavior>().enabled = false;

        }
        
        
    }
    
}
