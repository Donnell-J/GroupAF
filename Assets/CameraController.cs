using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraController : MonoBehaviour
{

    [SerializeField]
    private int speed = 15;
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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        height();
        parseMovement();
        parseRotation();
        parseZoom();

    }
    
    void parseMovement(){
        Vector2 moveInput = movement.action.ReadValue<Vector2>();

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
        FOV += Input.GetAxis("Mouse ScrollWheel") * sens;
        FOV = Mathf.Clamp(FOV, minFOV, maxFOV);
        Camera.main.fieldOfView = FOV;
    }

    void height(){
        Vector3 cameraPosition = transform.position;
        float ground = Terrain.activeTerrain.SampleHeight(cameraPosition);
        cameraPosition.y = Mathf.Lerp(cameraPosition.y,  ground + 50, Time.deltaTime * 10);
        transform.position = cameraPosition;
    }


}
