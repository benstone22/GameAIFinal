using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class BuddyFlocking : MonoBehaviour
{

    // Start is called before the first frame update
    public AstarBehavior astar;
    private Vector3 pos;
    Rigidbody rb;
    private Rigidbody playerRB;
    public GameObject player;
    private Vector3 cohesionForce = Vector2.zero;
    private Vector3 allignmentForce = Vector2.zero;
    private Vector3 separationForce = Vector2.zero;
    private Vector3 flockingForce = Vector2.zero;

    [SerializeField] private float forceConstant = 1f;
    [SerializeField] private Vector3 MinDistFromPlayer = new Vector3(2, 0,2 );
    [SerializeField] private Vector3 MaxDistFromPlayer = new Vector3(8, 0, 2);
    [SerializeField] public float DesiredMinDist = 0.5f;


    //[SerializeField] private float BoundsLeft;

    public List<GameObject> neighborhood;
    void Start()
    {
        astar = GetComponent<AstarBehavior>();
        rb = GetComponent<Rigidbody>();
        playerRB = player.GetComponent<Rigidbody>();
        neighborhood.Add(player);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            astar.enabled = true;
            GetComponent<BuddyFlocking>().enabled = false;

        }

        if (Input.GetMouseButtonDown(1))
        {
            astar.enabled = false;
        }
        Collider col = player.GetComponent<SphereCollider>(); //good for a when dealing with single agent pairs bad if multiple buddies are around player
        CohesionCalc(col);
        AllignmentCalc(col);
        SeparationCalc(col);

        Vector3 distanceDiff = (player.transform.position - transform.position);

        flockingForce = cohesionForce + separationForce + allignmentForce;
        
        Vector3 forceToAdd = flockingForce.normalized * forceConstant;

        forceToAdd.y = 0;  //Dont want y changes



        if (distanceDiff.magnitude >= MinDistFromPlayer.magnitude && playerRB.velocity != Vector3.zero) 
        {
            rb.AddForce(forceToAdd);
        }
        else if (distanceDiff.magnitude <= MinDistFromPlayer.magnitude && playerRB.velocity != Vector3.zero)
        {
            rb.AddForce(-forceToAdd);
            Debug.Log(-forceToAdd);
            Debug.Log(distanceDiff.magnitude);
        }
        else if (distanceDiff.magnitude >= MaxDistFromPlayer.magnitude && playerRB.velocity != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
        }

        if (playerRB.velocity == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
        }
    }
    //TODO: lOOK AT Photo notes from class for making the flock force work better

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

                if (distance < DesiredMinDist)
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
        // Vector3 diff = transform.position - col.GetComponent<Transform>().transform.position;
        // separationForce += (Vector3.one/diff.magnitude) / diff.magnitude;
        // separationForce = separationForce.normalized;
        // separationForce /= (diff.magnitude / col.bounds.size.magnitude);
        //
        // separationForce = separationForce.normalized;
    }

}
