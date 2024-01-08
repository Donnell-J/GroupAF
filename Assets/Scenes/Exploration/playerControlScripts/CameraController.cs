using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraController : MonoBehaviour
{

    [SerializeField]
    private int speed = 5;
    [SerializeField]
    private int rotSpeed = 180;
    [SerializeField]
    private InputActionReference movement, mousePos, rotate;
    private Vector3 velocity = Vector3.zero;
    private Vector3 Avelocity = Vector3.zero;

    private Vector2 rotStartPos;
    private Vector3 rotVelocity = Vector3.zero;
    private Vector3 internalRotVelocity = Vector3.zero;

    private float minFOV = 15f;
    private float maxFOV = 90f;
    private float sens = 10f;
    
    public Transform followObject;
    public Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = MovingScenes.instance.getPreCombatPosition();
        transform.rotation = MovingScenes.instance.getCameraRotation();
    }

    // Update is called once per frame
    void Update()
    {
        //height();
        parseMovement();
        parseRotation();
        parseZoom();
        // transform.position = followObject.position + offset;
    }
    
    void parseMovement(){
        Vector2 moveInput = movement.action.ReadValue<Vector2>(); //Get wasd input, project it so that it is in terms of the direction the camera is facing, and apply a lerp effect to the movement

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 relativeForward = moveInput.y * forward;
        Vector3 relativeRight = moveInput.x * right;
        Vector3 targetVelocity = (relativeForward + relativeRight).normalized;

        velocity = Vector3.SmoothDamp(velocity,targetVelocity, ref Avelocity, 0.15f);
        transform.position+= velocity *speed * Time.deltaTime;
    }

    void parseRotation(){
        Vector3 targetRotVelocity = Vector3.zero;
        if(rotate.action.WasPressedThisFrame()){
            rotStartPos = mousePos.action.ReadValue<Vector2>();
        }else if(rotate.action.IsPressed()){
            targetRotVelocity =  new Vector3(0,((mousePos.action.ReadValue<Vector2>().x - rotStartPos.x)/Screen.width)*4,0);

            //float rotDistance = mousePos.action.ReadValue<Vector2>().x - rotStartPos.x;
        }
        rotVelocity = Vector3.SmoothDamp(rotVelocity,targetRotVelocity, ref internalRotVelocity, 0.15f);
        transform.rotation *= Quaternion.Euler(rotVelocity* rotSpeed * Time.deltaTime);
        
    }

    void parseZoom(){
        float FOV = Camera.main.fieldOfView;
        FOV += Input.GetAxis("Mouse ScrollWheel") * sens; //change FOV between min-max based on scroll wheel to emulate a camera zoom effect 
        FOV = Mathf.Clamp(FOV, minFOV, maxFOV);
        Camera.main.fieldOfView = FOV;
    }

    void height(){ //Currently inactive function that is meant to find the distance tothe ground from the camera so that it maintains a certain height above it 
        Vector3 cameraPosition = transform.position;
        Ray ray = new Ray(cameraPosition, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            float ground = hit.point.y;
            cameraPosition.y = Mathf.Lerp(cameraPosition.y,  ground + 55, Time.deltaTime * 10);
            transform.position = cameraPosition;
        }
    }
    
    

    
}
