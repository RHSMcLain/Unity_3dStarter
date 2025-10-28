using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 5f, jumpPower = 3f, sprintSpeed = 15f;
    Rigidbody rb;
    InputAction moveAction;
    Vector3 startLocation;
    Quaternion startRotation;
    bool isGrounded = false, isSprinting = false;
   void Start()
    {
        print("test");
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        startLocation = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //MOUSE CONTROLS
        Plane plane = new Plane(Vector3.up, transform.position.y);
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        float hit;
        if (plane.Raycast(ray, out hit))
        {
            Vector3 hitPoint = ray.GetPoint(hit);
            transform.LookAt(new Vector3(hitPoint.x, transform.position.y, hitPoint.z));
        }

        //
        if (isGrounded)
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
        try
        {
            ContactPoint cp = collision.GetContact(0);
            if (cp.thisCollider.gameObject.tag == "foot")
            {
                print("--footCooll");
                print("exit--");
                isGrounded = false;
            }
        }
        catch
        {
            print("nope");
        }
        
    }
    void OnJump()
    {
        if (isGrounded)
        {

            isGrounded = false;
            print("Jjasdifjaighigaehgiuahgiouha");
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

}