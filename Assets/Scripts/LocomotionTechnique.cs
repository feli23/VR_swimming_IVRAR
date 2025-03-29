using System;
using UnityEngine;

public class LocomotionTechnique : MonoBehaviour
{
    // Please implement your locomotion technique in this script. 
    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;
    [Range(0, 10)] public float translationGain = 0.5f;
    [Range(0, 1)] public float moveTolerance = 0.05f;
    public Rigidbody player;

    public GameObject hmd;
    private float leftTriggerValue;
    private float rightTriggerValue;
    private Vector3 velocityL = Vector3.zero;
    private Vector3 velocityR = Vector3.zero;
    private Vector3 conPosL;
    private Vector3 conPosR;
    private Vector3 startPos;
    private Vector3 offset;
    private bool isIndexTriggerDown;
    private Vector3 furthest;
    private double furthestDistance;
    private float distController = 0;
    private bool distChange = false;
    private Vector3 positionHead_initial;
    private Vector3 positionHead;

    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public string stage;
    public SelectionTaskMeasure selectionTaskMeasure;
    private double resetDist = 2;
    public float speed;


    void Start()
    {
        player = GetComponent<Rigidbody>();
        positionHead_initial = transform.position;
    }

    void Update()
    {
        if (player.transform.position.y > 17)
        {
            Vector3 pos = player.transform.position;
            pos.y = 17;
            player.transform.position = pos;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D

        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController);
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);

        velocityL = velocityL + OVRInput.GetLocalControllerVelocity(leftController);
        velocityR = velocityR + OVRInput.GetLocalControllerVelocity(rightController);

        conPosL = OVRInput.GetLocalControllerPosition(leftController);
        conPosR = OVRInput.GetLocalControllerPosition(rightController);
        //Debug.Log("Controler y: " + conPosR.y);

        positionHead = transform.position;
        //Debug.Log(positionHead.y);

        float distTemp = (conPosL - conPosR).magnitude;
        //Debug.Log("Controller Dist:" + distTemp);
        //Debug.Log("Dist Change:" + (distTemp - distController));

        if (distTemp - distController > 0.5)
        {
            distController = distTemp;
            distChange = true;

        }
        else
        {
            distController = 0;
            distChange = false;
        }


        ////////////////////////////////////////////////////////////////////////////////
        // These are for the game mechanism.
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                //hmd.addForce(offset);
            }
        }
    }

    private void FixedUpdate()
    {
        //my version

        Vector3 velocity = OVRInput.GetLocalControllerVelocity(rightController);
        Debug.Log(velocity);
        Vector3 playerPos = Vector3.zero;

        if (leftTriggerValue > 0.95f && rightTriggerValue > 0.95f)
        {
            isIndexTriggerDown = true;

            double diL = Dist(playerPos, (conPosL + conPosR) / 2);
            if (diL > furthestDistance)
            {
                furthestDistance = diL;
                furthest = (conPosL + conPosR) / 2;
            }
            else if (diL < resetDist)
            {
                furthest = Vector3.zero;
                furthestDistance = 0;
            }


            Vector3 dVec = furthest - playerPos;

            float distL = PointDist(new Ray(playerPos, dVec), conPosL);
            float distR = PointDist(new Ray(playerPos, dVec), conPosR);

            bool doMove = (Math.Abs(distL - distR) < moveTolerance && distChange);

            if (doMove)
            {
                offset.x = -(velocityL.x + velocityR.x) / 2;
                offset.y = (-(velocityL.y + velocityR.y) / 2);
                offset.z = -(velocityL.z + velocityR.z) / 2;

                //Debug.Log("offsetY: " + offset.y);
                player.AddForce(offset * speed, ForceMode.VelocityChange);
            }

        }
        else
        {
            isIndexTriggerDown = false;
            offset = Vector3.zero;
        }

        velocityL = Vector3.zero;
        velocityR = Vector3.zero;
        distChange = false;
    }

    private float PointDist(Ray ray, Vector3 point)
    {

        return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    }

    private double Dist(Vector3 playerPos, Vector3 conPosL)
    {
        return Math.Sqrt(Math.Pow(playerPos.x - conPosL.x, 2) + Math.Pow(playerPos.z - conPosL.z, 2));
    }

    void OnTriggerEnter(Collider other)
    {

        // These are for the game mechanism.
        if (other.CompareTag("banner"))
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }
        else if (other.CompareTag("objectInteractionTask"))
        {
            selectionTaskMeasure.isTaskStart = true;
            selectionTaskMeasure.scoreText.text = "";
            selectionTaskMeasure.partSumErr = 0f;
            selectionTaskMeasure.partSumTime = 0f;
            // rotation: facing the user's entering direction
            float tempValueY = other.transform.position.y > 0 ? 12 : 0;
            Vector3 tmpTarget = new(hmd.transform.position.x, tempValueY, hmd.transform.position.z);
            selectionTaskMeasure.taskUI.transform.LookAt(tmpTarget);
            selectionTaskMeasure.taskUI.transform.Rotate(new Vector3(0, 180f, 0));
            selectionTaskMeasure.taskStartPanel.SetActive(true);
        }
        else if (other.CompareTag("coin"))
        {
            if (other.gameObject.activeSelf)
            {
                other.gameObject.SetActive(false);
                parkourCounter.coinCount += 1;
                GetComponent<AudioSource>().Play();
            }
        }
        // These are for the game mechanism.
    }
}