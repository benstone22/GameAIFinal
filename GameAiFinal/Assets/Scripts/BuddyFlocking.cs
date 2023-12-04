using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class BuddyFlocking : MonoBehaviour
{

    // Start is called before the first frame update

    [SerializeField] private float distance = 5;
    private Vector3 pos;
    Rigidbody rb;
    public GameObject player;
    private Vector3 cohesionForce = Vector2.zero;
    private Vector3 allignmentForce = Vector2.zero;
    private Vector3 separationForce = Vector2.zero;
    private Vector3 flockingForce = Vector2.zero;

    [SerializeField] public float DESIREDMINDIST = 0.5f;
    [SerializeField] private SphereCollider FlockRange;
    //[SerializeField] private float BoundsLeft;

    public List<GameObject> neighborhood;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FlockRange = player.GetComponent<SphereCollider>();
        neighborhood.Add(player);
    }

    // Update is called once per frame
    void Update()
    {
        Collider col = player.GetComponent<SphereCollider>(); //good for a when dealing with single agent pairs bad if multiple buddies are around player
        CohesionCalc(col);
        AllignmentCalc(col);
        SeparationCalc(col);

        flockingForce = cohesionForce + separationForce + allignmentForce;
        
        rb.AddForce(flockingForce);
    }

    private void FixedUpdate()
    {
        // pos.x = Mathf.Clamp(transform.position.x, (player.transform.position.x - 5f), (player.transform.position.x + 5f));
        // pos.y = Mathf.Clamp(transform.position.y, (player.transform.position.y - 5f), (player.transform.position.y + 5f));
        // pos.z = Mathf.Clamp(transform.position.z, (player.transform.position.z - 5f), (player.transform.position.z + 5f));
        //
        // transform.position = pos;
        
        //Dont deal with position deal with velocity.
        
        //TODO: lOOK AT Photo notes from class for making the flock force work better
        
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

            Vector3 distVect = PosCenter - transform.position;

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
    {   //fix calculation
        int closeAgents = 0;
        Vector3 pos = Vector2.zero;

        if (neighborhood.Count != 0)
        {
            pos = transform.position;
            for (int i = 0; i < neighborhood.Count; i++)
            {
                Vector3 neighborPos = neighborhood[i].transform.position;
                Vector3 diffVect = transform.position - neighborPos;
                float distance = diffVect.magnitude;

                if (distance < DESIREDMINDIST)
                {
                    var hatVect = diffVect.normalized;
                    separationForce += hatVect/distance;
                    closeAgents++;
                }
            }

            if (closeAgents != 0)
            {
                separationForce /= closeAgents;
            }

            separationForce = separationForce.normalized;
        }


        Vector3 diff = transform.position - col.GetComponent<Transform>().transform.position;
        separationForce += (Vector3.one/diff.magnitude) / diff.magnitude;
        separationForce = separationForce.normalized;
        separationForce /= (diff.magnitude / col.bounds.size.magnitude);
        
        separationForce = separationForce.normalized;
    }

}
