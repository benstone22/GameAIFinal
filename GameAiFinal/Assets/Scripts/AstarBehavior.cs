using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AstarBehavior : SpacialQuatization
{
    [SerializeField] public Vector3 targetPos = new Vector3(20.0f, 9.0f, 142.0f);

    public static int manhattanDist(Vector3 source, Vector3 target)
    {
        var qs = Quantize(source);
        var qt = Quantize(target);
        return Mathf.Abs(qs.x - qt.x) + Mathf.Abs(qs.y - qt.y) + Mathf.Abs(qs.y - qt.y);
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(Quantize(this.transform.position));
        Debug.Log(manhattanDist(this.transform.position, targetPos));

        // if (manhattanDist(this.transform.position, targetPos) != 0)
        // {
        //     
        // }
    }

    public void BuildPath(Vector3 StartPos, Vector3 GoalPos)
    {
        
    }
}
