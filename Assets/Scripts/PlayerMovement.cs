using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    enum lookMode  {raycast, lookAction, noRotation};
    [SerializeField]
    lookMode currMode = lookMode.raycast;
    [SerializeField]
    float speed = 5f, jumpPower = 3f, sprintSpeed = 15f, laserDuration = 0.1f, turnSpeed = 5f;
    [SerializeField]
    int maxHitPoints = 10;
    int hitpoints;
    Rigidbody rb;
    LineRenderer lr;
    InputAction moveAction, lookAction;
    Vector3 startLocation;
    Quaternion startRotation;
    GameObject foot;
    bool isGrounded = false, isSprinting = false;
    float laserTimer = 0f;
    //TODO: create the public void takeDamage, and then have the enemy call it.
   void Start()
    {
        
        hitpoints = maxHitPoints;
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        moveAction = InputSystem.actions.FindAction("Move");

        lookAction = InputSystem.actions.FindAction("Look");
        startLocation = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //MOUSE CONTROLS
        laserTimer += Time.deltaTime;

        //turn off the laser if the time has passed
        if (laserTimer > laserDuration)
        {
            lr.enabled = false;
        }
        switch (currMode)
        {
            case lookMode.raycast:
                raycastLook();
                break;
            case lookMode.lookAction:
                actionLook();
                break;
            case lookMode.noRotation:
                break;
        }


        //
        if (groundCheck())
        {
            float currSpeed = speed;
            if (isSprinting)
            {
                currSpeed = sprintSpeed;
            }
            Vector2 inputDir = moveAction.ReadValue<Vector2>();

            //Vector3 moveDir = new Vector3(inputDir.x, 0f, inputDir.y) * Time.deltaTime * speed; //would move according to world axes
            Vector3 moveDir = (transform.forward * inputDir.y + transform.right * inputDir.x); //sets movement to be based on where the player is facing
            moveDir = moveDir.normalized * currSpeed;  //we balance the direction to a magnitue of 1, then multiply by our desired speed


            // rb.AddForce(moveDir, ForceMode.Impulse);  //this line adds force -- so there's a ramp up time
            rb.linearVelocity = moveDir; // set the speed directly, instead
        }
        if (transform.position.y < -2)
        {
            die();
        }

    }
    Vector3 getMouseLocation()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        float hit;
        Plane plane = new Plane(Vector3.up, transform.position.y);

        if (plane.Raycast(ray, out hit))
        {
            Vector3 hitPoint = ray.GetPoint(hit);
            return hitPoint;

        }
        else
        {
            return Vector3.zero;
        }
    }
    void raycastLook()
    {
            Vector3 hitPoint = getMouseLocation();
            transform.LookAt(new Vector3(hitPoint.x, transform.position.y, hitPoint.z)); 
    }
    void actionLook()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
        print(mouseDelta);
        transform.Rotate(new Vector3(0f, mouseDelta.x, 0f) * Time.deltaTime * turnSpeed);

    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp = collision.GetContact(0);
        if (cp.thisCollider.gameObject.tag == "foot")
        {
            print("footCooll");
            print("enter");
            isGrounded = true;
        }
        
    }
    void OnCollisionExit(Collision collision)
    {
        print("exit");
        try
        {
            ContactPoint cp = collision.GetContact(0);
            if (cp.thisCollider.gameObject.tag == "foot")
            {
                print("--footCooll");

                isGrounded = false;
            }
        }
        catch
        {
            print("nope");
        }

    }
    void OnAttack()
    {
        Vector3 direction = transform.forward;
        if (currMode == lookMode.noRotation)
        {
            Vector3 mouseDir = getMouseLocation();

            mouseDir.y = transform.position.y;
           
            direction = mouseDir - transform.position;
            print(direction);
            //now we're going to target the mouse instead of the forward direction

        }
        print("attack");
        RaycastHit hit; //holds information about where the ray hits

        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
        {
            //means we hit something
            //set our line renderer to stop there.
            print("Hit");
            lr.SetPosition(0, transform.position); //starts at ourselves
            lr.SetPosition(1, hit.point);
            lr.enabled = true;
            laserTimer = 0f;
            //if it's an enemy, do damage to the enemy
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //do damage to the enemy
                Destroy(hit.collider.gameObject);
            }

        }
        else
        {
            //we missed, but we still have to draw a laser
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + direction * 20);
            lr.enabled = true;
            laserTimer = 0f;
        }

        
    }
    void OnJump()
    {
        if (groundCheck())
        {

            isGrounded = false;
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    void OnSprint()
    {
        print("sprint");
        isSprinting = !isSprinting;
    }
    void die()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startLocation;
        transform.rotation = startRotation;

    }
    public void takeDamage(int damage)
    {
        hitpoints -= damage;
        if (hitpoints <= 0)
        {
            die();
        }
    }
    bool groundCheck()
    {
        //this function checks if we are within a short space vertically from a ground object. 
        LayerMask groundMask = LayerMask.GetMask("Ground");

        
        // if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f, groundMask))
        if (Physics.CheckSphere(transform.position - new Vector3(0f, 0.5f, 0f), (transform.localScale.y * 1.1f)/2f, groundMask))
        {
            print("ground");
            return true;
        }
        else
        {
            print("air");
            return false;
        }

    }

}