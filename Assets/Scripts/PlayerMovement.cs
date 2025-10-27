using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 5f, jumpPower = 3f;
    Rigidbody rb;
    InputAction moveAction;
    Vector3 startLocation;
    Quaternion startRotation;
   void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        startLocation = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //MOUSE CONTROLS
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }

        //
        Vector2 inputDir = moveAction.ReadValue<Vector2>();
        //Vector3 moveDir = new Vector3(inputDir.x, 0f, inputDir.y) * Time.deltaTime * speed;
        Vector3 moveDir = (transform.forward * inputDir.y + transform.right * inputDir.x) * Time.deltaTime * speed;


        rb.AddForce(moveDir, ForceMode.Impulse);

        if (transform.position.y < -2)
        {
            die();
        }

    }
    void OnJump()
    {
        print("Jjasdifjaighigaehgiuahgiouha");
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    void die()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startLocation;
        transform.rotation = startRotation;

    }

}