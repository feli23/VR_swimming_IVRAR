using UnityEngine;
using static OVRInput;

public class rampmover : MonoBehaviour
{

    public OVRInput.Controller leftController;
    public Transform RampObject;
    Quaternion startQuat, rampQuat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rampQuat = RampObject.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        
        Quaternion rampRot = RampObject.rotation;
        rampRot = rampRot * Quaternion.Euler(-(input.y * 0.75f), 0f, 0f);

        Debug.Log("" + rampRot.eulerAngles.x + " ; " + rampRot.x);
        //if (rampRot.x >= -0.182f && rampRot.x < 0f){
            RampObject.rotation = rampRot;
      //}
    }
}
