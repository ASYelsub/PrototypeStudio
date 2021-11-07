using UnityEngine;
public class TreeCameraController : MonoBehaviour
{
    float x;
    float y;
    float z;
    float yaw;
    float pitch;
    float roll;       

    [Header("Settings")]
    public float mouseSensitivity = 5f;
    public float moveSpeed = 2f;
    public float colliderSize = 5f;

    [Header("Other Objects")]
    public GameObject myCollider;

    void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update(){

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // yaw input
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // pitch input
        pitch -= mouseY;
        yaw += mouseX;
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0.0f);
        myCollider.transform.localRotation = Quaternion.Euler(pitch, yaw, 0.0f);


        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDirection.Normalize();
        Move(inputDirection);
    }
    bool col = false;
    public void RecieveCollision(){
        col = true;
        Debug.Log("here");
    }
    public void LeaveCollision(){
        col = false;
    }
    void Move(Vector2 input)
    {
        myCollider.transform.position = transform.position;
        Vector3 rl = Vector3.zero;
        Vector3 fb = Vector3.zero;
        
        if(input.x > 0){
            rl = Vector3.right;
        }
        else if(input.x < 0){
            rl = Vector3.left;
        }
        if(input.y > 0){
            fb = Vector3.forward;
        }
        else if(input.y < 0){
            fb = Vector3.back;
        }
        myCollider.transform.Translate(fb);
        myCollider.transform.Translate(rl);
        if(!col){
            transform.Translate((rl));
            transform.Translate((fb));
        }
    }

    

}