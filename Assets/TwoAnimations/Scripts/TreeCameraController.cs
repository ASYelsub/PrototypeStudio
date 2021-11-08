using UnityEngine;
public class TreeCameraController : MonoBehaviour
{
    float yaw;
    float pitch;

    [Header("Settings")]
    public float mouseSensitivity = 5f;
    public float colliderSize = 5f;

    [Header("Other Objects")]
    public GameObject myCollider;

    void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update(){

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch -= mouseY;
        yaw += mouseX;
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0.0f);

        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(inputDirection);
    }
    bool col = false;
    public void RecieveCollision(){
        col = true;
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
            myCollider.transform.Translate(rl);
        }
        else if(input.x < 0){
            rl = Vector3.left;
            myCollider.transform.Translate(rl);
        }
        if(input.y > 0){
            fb = Vector3.forward;
            myCollider.transform.Translate(fb);
        }
        else if(input.y < 0){
            fb = Vector3.back;
            myCollider.transform.Translate(fb);
        }
        if(!col){
            transform.Translate((rl));
            transform.Translate((fb));
        }
    }

    

}