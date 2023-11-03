using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//In memory of doug tillman 
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider boxCollider;
    public Vector3 moveForce;
    public float speed;
    public float jumpForce;
    private bool canJump;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        speed = 25f; 
        jumpForce = 1000f;
        canJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        float inpX = Input.GetAxis("Horizontal")*speed;
        moveForce.x = inpX;
        float inpZ = Input.GetAxis("Vertical")*speed;
        moveForce.z = inpZ;
        rb.AddForce(moveForce);
        

        
        //jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
           jump(gameObject,rb,jumpForce);
        }

        
        
        
    }
    public void jump(GameObject gameObj, Rigidbody rb, float jumpForce)
    {
        
        canJump = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 10f);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.red, 10f);
        if (canJump)
        {
                
            Vector3 jump = new Vector3(0, jumpForce, 0);
            rb.AddForce(jump);
            canJump = false;
        }

    }
    
    
}
