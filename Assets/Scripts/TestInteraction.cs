using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ControllerScript : MonoBehaviour
{
    public Camera sceneCamera;

    public float rotationSpeed = 70.0f;
    public float x = 0.01f;
    private Vector3 scaleVector;

    public Vector3 minScale = Vector3.zero;
    public Vector3 maxScale = new(10f, 10f, 10f);

    // Start is called before the first frame update
    void Start()
    {
        scaleVector = new(x, x, x);
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation (right controller, thumbstick)
        Vector2 rightStickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (rightStickInput != null)
        {
            RotateObject(rightStickInput);
        }
        // Scale (right controller, A/B)
        if(OVRInput.Get(OVRInput.Button.One) ^ OVRInput.Get(OVRInput.Button.Two))
        {
            if(OVRInput.Get(OVRInput.Button.One))
            {
                ScaleObject(false);
            }
            else
            {
                ScaleObject(true);
            }
        }
        


        // If user has just released Button A of right controller in this frame
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            // Play short haptic on right controller
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        }



        // While user holds the left hand trigger
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.0f)
        {
            // Assign left controller's position and rotation to cube
            transform.SetPositionAndRotation(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch));
        }
    }

    void ResetPosition()
    {

    }

    // ???????
    void RotateObject(Vector2 rightStickInput)
    {
        float t = rightStickInput.magnitude;

        Vector3 cameraRight = sceneCamera.transform.right;
        Vector3 cameraUp = sceneCamera.transform.up;
        transform.RotateAround(transform.position, cameraUp, -rightStickInput.x * rotationSpeed * Time.deltaTime * t);
        transform.RotateAround(transform.position, cameraRight, rightStickInput.y * rotationSpeed * Time.deltaTime * t);
    }
    
    // ??????
    void ScaleObject(bool b)
    {
        if (b)
        {
            if(transform.localScale.x < 3f)
            transform.localScale += scaleVector;
        }
        else
        {
            if(transform.localScale.x > 0.08f)
            transform.localScale -= scaleVector;
        }
    }


}
