using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BuddyFlocking : MonoBehaviour
{

    // Start is called before the first frame update
    Rigidbody rb;
    float cohesionForce;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cohesionForce = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
    float CohesionCalc(Collider col)
    {

        cohesionForce = col.transform.position.magnitude - gameObject.transform.position.magnitude;
        return cohesionForce;
    }
    
}
