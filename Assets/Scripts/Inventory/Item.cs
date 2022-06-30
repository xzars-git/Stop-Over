using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    
    public enum InteractionType { NONE, PickUp, Examine, GrabDrop, FloorChanger} //,OpenAble }
    public enum ItemType { Static, Consumables, Unlocker}
    [Header("Attributes")]
    public InteractionType interactType;

    public ItemType type;
    public bool stackable = true;

    [Header("Exmaine")]
    public string descriptionText;

    [Header("Custom Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

/*    [Header("Change Scene")]
    public Object scene;*/

    [Header("Room Changer")]
    public UIFloorChanger FloorChangerUI;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 7;
    }

    public void Interact()
    {
        switch(interactType)
        {
            case InteractionType.PickUp:
                if (!FindObjectOfType<InventorySystem>().CanPickUp())
                    return;
                //Add the object to the PickedUpItems list
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                //Disable
                gameObject.SetActive(false);
                break;
            case InteractionType.FloorChanger:
                Time.timeScale = 0f;
                FloorChangerUI.Show();
                break;
/*                SceneManager.LoadScene(scene.name);*/
            case InteractionType.Examine:
                ////Call the Examine item in the interaction system
                FindObjectOfType<InteractionSystem>().ExamineItem(this);  
                break;
            case InteractionType.GrabDrop:
                //Grab interaction
                FindObjectOfType<InteractionSystem>().GrabDrop();
                break;
            //case InteractionType.OpenAble:
            //    //OpenAble interaction
            //    FindObjectOfType<OpenAble>().OpenAbleObj();
            //    break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        //Invoke (call) the custom event(s)
        customEvent.Invoke();
    }

   



}
