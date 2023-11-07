using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BuddyFlocking : MonoBehaviour
{

    // Start is called before the first frame update
    Rigidbody rb;
    Vector3 cohesionForce;
    public GameObject player;
    Vector3 allignmentForce;
    Vector3 separationForce;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cohesionForce = Vector2.zero;
        allignmentForce = Vector2.zero;
        separationForce = Vector2.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider col = player.GetComponent<SphereCollider>();
        CohesionCalc(col);
        AllignmentCalc(col);
        SeparationCalc(col);
        
        rb.AddForce(cohesionForce + separationForce + allignmentForce);
    }
    
    private void CohesionCalc(Collider col)
    {
        
        cohesionForce = col.transform.position - gameObject.transform.position;
        cohesionForce = cohesionForce.normalized;
        
        
    }
    private void AllignmentCalc(Collider col)
    {
        allignmentForce= player.GetComponent<Rigidbody>().velocity;
    }
    private void SeparationCalc(Collider col)
    {
        Vector3 diff = transform.position - col.GetComponent<Transform>().transform.position;
        separationForce += (Vector3.one/diff.magnitude) / diff.magnitude;
        separationForce = separationForce.normalized;
        separationForce /= (diff.magnitude / col.bounds.size.magnitude);
        
        separationForce = separationForce.normalized;
    }

}
