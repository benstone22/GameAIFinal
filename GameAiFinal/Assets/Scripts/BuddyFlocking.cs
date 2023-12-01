using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class BuddyFlocking : MonoBehaviour
{

    // Start is called before the first frame update
    Rigidbody rb;
    public GameObject player;
    private Vector3 cohesionForce = Vector2.zero;
    private Vector3 allignmentForce = Vector2.zero;
    private Vector3 separationForce = Vector2.zero;
    private Vector3 flockingForce = Vector2.zero;

    [FormerlySerializedAs("PlayerNeighborhood")] public List<GameObject> neighborhood;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        neighborhood.Add(player);
    }

    // Update is called once per frame
    void Update()
    {
        Collider col = player.GetComponent<SphereCollider>();
        CohesionCalc(col);
        AllignmentCalc(col);
        SeparationCalc(col);

        flockingForce = cohesionForce + separationForce + allignmentForce;
        
        rb.AddForce(cohesionForce + separationForce + allignmentForce);
    }
    
    private void CohesionCalc(Collider col)
    {
        Vector3 posSum = Vector3.zero;
        Vector3 PosCenter = Vector3.zero;
        
        if (neighborhood.Count != 0)
        {
            for (int i = 0; i < neighborhood.Count; ++i)
            {
                posSum += neighborhood[i].transform.position;
            }
            PosCenter = posSum / (neighborhood.Count);

            Vector3 distVect = PosCenter - this.transform.position;

            cohesionForce = distVect.normalized;
        }
        // cohesionForce = col.transform.position - gameObject.transform.position;
        // cohesionForce = cohesionForce.normalized;
        
        
    }
    private void AllignmentCalc(Collider col)
    {
        allignmentForce += player.GetComponent<Rigidbody>().velocity;
        allignmentForce /= neighborhood.Count + 1;
        allignmentForce = allignmentForce.normalized;
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
