using UnityEngine;

public class MyGrab : MonoBehaviour
{
    public OVRInput.Controller controller;
    private float triggerValue;
    private bool isInCollider;
    private bool isSelected;
    private GameObject selectedObj;
    public SelectionTaskMeasure selectionTaskMeasure;
    private string selectedTag;

    void Update()
    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        

        if (isInCollider)
        {
            if (!isSelected && triggerValue > 0.95f)
            {
                isSelected = true;
                //Vector2 triggerVal = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);
                if (selectedTag == "plug")
                {
                    selectedObj.transform.parent = transform;
                }
                else { 
                selectedObj.transform.parent.transform.parent = transform;
                }
            }
            else if (isSelected && triggerValue < 0.95f)
            {
                isSelected = false;
                if (selectedTag == "plug")
                {
                    selectedObj.transform.parent = null;
                }
                else
                {
                    selectedObj.transform.parent.transform.parent = null;
                        }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        selectedTag = other.tag;

        if (other.gameObject.CompareTag("plug"))
        {
            isInCollider = true;
            selectedObj = other.gameObject;
        }
        else if (other.gameObject.CompareTag("objectT"))
        {
            isInCollider = true;
            selectedObj = other.gameObject;
        }
        else if (other.gameObject.CompareTag("selectionTaskStart"))
        {
            if (!selectionTaskMeasure.isCountdown)
            {
                selectionTaskMeasure.isTaskStart = true;
                selectionTaskMeasure.StartOneTask();
            }
        }
        else if (other.gameObject.CompareTag("done"))
        {
            selectionTaskMeasure.isTaskStart = false;
            selectionTaskMeasure.EndOneTask();
        }
    }

    void OnTriggerExit(Collider other)
    {
        selectedTag = null;
        if (other.gameObject.CompareTag("plug"))
        {
            isInCollider = false;
            selectedObj = null;
        }
        else if (other.gameObject.CompareTag("objectT"))
        {
            isInCollider = false;
            selectedObj = null;
        }
    }
}