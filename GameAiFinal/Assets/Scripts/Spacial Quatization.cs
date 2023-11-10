using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacialQuatization : MonoBehaviour
{
    private Dictionary<Vector3Int, HashSet<GameObject>> hashmap;
    private Dictionary<Vector3Int, Vector3Int> cameFrom;  //flowfield
    public static Vector3Int Quantize(Vector3 v, float resolution = 1f)
    {
        return new Vector3Int(Mathf.FloorToInt(v.x / resolution), Mathf.FloorToInt(v.y /resolution), Mathf.FloorToInt(v.z / resolution));
    }

    public static Vector3 Dequantize(Vector3Int index, float resolution = 1f)
    {
        return new Vector3((float)index.x * resolution + resolution / 2.0f, (float)index.y * resolution + resolution / 2.0f, (float)index.z);
    }

    public void Move(GameObject go, Vector3 previous, Vector3 current)
    {
        Vector3Int previousBucket = Quantize(previous);
        Vector3Int currentBucket = Quantize(current);

        if (previousBucket != currentBucket) 
        {
            hashmap[previousBucket].Remove(go);
            hashmap[currentBucket].Add(go);
        }

        //recalc camefrom map 
    }

    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
