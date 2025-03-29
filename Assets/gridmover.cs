using UnityEngine;
using static OVRInput;

public class gridmover : MonoBehaviour
{
    public Transform GridObject;
    public Transform Ramp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Debug.Log(input.x);

        //if (Ramp.rotation.x > -0.005f)
        //{
            Vector3 newPos = new Vector3(GridObject.position.x, GridObject.position.y + (input.y * 0.25f), GridObject.position.z);
            Debug.Log(newPos);
            if (newPos.y < 25f && newPos.y >= 11f) 
                GridObject.position = newPos;
        //}

    }
}
