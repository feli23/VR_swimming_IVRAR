using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_PlugScript : MonoBehaviour
{
    [Tooltip("Feature for one using only")]
    public bool OneTime = false;
    [Tooltip("Plug follow this local EmptyObject")]
    public Transform HeroHandsPosition;
    [Tooltip("SocketObject with collider(shpere, box etc.) (is trigger = true)")]
    public Collider Socket; // need Trigger
    public AN_DoorScript DoorObject;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    bool follow = false, isConnected = false, followFlag = false, youCan = true;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
       

        // frozen if it is connected to PowerOut
        if (isConnected)
        {
            gameObject.transform.position = Socket.transform.position;
            gameObject.transform.rotation = Socket.transform.rotation;
            DoorObject.isOpened = true;
        }
        else
        {
            DoorObject.isOpened = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("plug"))
        {
            
        }
        if (other == Socket)
        {
            isConnected = true;
            follow = false;
            DoorObject.rbDoor.AddRelativeTorque(new Vector3(0, 0, 80f));
        }
        if (OneTime) youCan = false;
    }
}
