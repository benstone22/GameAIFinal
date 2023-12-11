using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowState : MonoBehaviour
{
    // Start is called before the first frame update
    public string state;
    void Start()
    {
        state = "Left Click for Astar. Right Click for Flocking";
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TMP_Text>().text = state;
        if (Input.GetMouseButtonDown(0))
        {
            state = "Current State: Astar.       " + " Right Click for Flocking";
        }
        if (Input.GetMouseButtonDown(1))
        {
            state = "Current State: Flocking." + " Left Click for Astar";
        }
    }
}
