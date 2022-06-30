using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField]
    private Text pickUpText;

    [Header("Detection Fields")]
    //Detection Point
    public Transform detectionPoint;
    //Detection Radius
    private const float detectionRadius = 0.2f;
    //Detection Layer
    public LayerMask detectionLayer;
    //Cached Trigger Object
    public GameObject detectedObject;
    [Header("Examine Fields")]
    //Examine window object//
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public bool isExamining;
    [Header("Grabbing")]
    public GameObject grabbedObject;
    public float grabbedObjectYValue;
    public Transform grabPoint;
    public bool isGrabbing;


    //[Header("Unlocked field")]
    //public bool isUnlocked;


    private void Start()
    {
        pickUpText.gameObject.SetActive(false);
    }

    void Update()
    {
        if(DetectObject())
        {

            if(InteractInput())
            {
                
                //Debug.Log("Berinteraksi");
                //If we are grabbing something don't interact with other items, drop the grabbed item first
                if (isGrabbing)
                {
                    
                    GrabDrop();
                    
                    return;
                }

                detectedObject.GetComponent<Item>().Interact();
            }
        }
        if (Canshow() == false)
            return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        if (obj == null)
        {
            pickUpText.gameObject.SetActive(false);
            detectedObject = null;
            return false;
        }
        else
        {
            pickUpText.gameObject.SetActive(true);
            detectedObject = obj.gameObject;
            if (isGrabbing || isExamining)
            {

                pickUpText.gameObject.SetActive(false);

            }
            return true;
            
        }
    }

    bool Canshow()
    {

        //can move when Opened Inventory
        if (FindObjectOfType<InventorySystem>().isOpen)

            pickUpText.gameObject.SetActive(false);
        return false;

    }

    public void ExamineItem(Item item)
    {
        if (isExamining)
        {
            //Hide the Examine Window
            examineWindow.SetActive(false);
            //disable the boolean
            isExamining = false;
        }
        else
        {
            //Show the item's image in the middle
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            //Write description text underneath the image
            examineText.text = item.descriptionText;
            //Display an Examine Window
            examineWindow.SetActive(true);
            //enable the boolean
            isExamining = true;
        }
    }

    public void GrabDrop()
    {
        //Check if we do have a grabbed object => drop it
        if (isGrabbing)
        {

            //make isGrabbing false
            isGrabbing = false;
            //unparent the grabbed object
            grabbedObject.transform.parent = null;
            //set the y position to its origin
            grabbedObject.transform.position = new Vector3(grabbedObject.transform.position.x, grabbedObjectYValue, grabbedObject.transform.position.z);
            //null the grabbed object reference
            grabbedObject = null;
        }
        //Check if we have nothing grabbed grab the detected item
        else
        {
            //Enable the isGrabbing bool
            isGrabbing = true;
            //assign the grabbed object to the object itself
            grabbedObject = detectedObject;
            //Parent the grabbed object to the player
            grabbedObject.transform.parent = transform;
            //Cache the y value of the object
            grabbedObjectYValue = grabbedObject.transform.position.y;
            //Adjust the position of the grabbed object to be closer to hands                        
            grabbedObject.transform.localPosition = grabPoint.localPosition;
        }
    }


    


}