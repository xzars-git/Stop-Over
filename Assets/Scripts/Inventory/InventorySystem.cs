using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [System.Serializable]
    //inventory Item Class
    
    public class InventoryItem
    {
        public GameObject obj;

        public int stack=1;

        public InventoryItem(GameObject o, int s=1)
        {
            obj = o;
            stack = s;
        }
    }
    [Header("General Fields")]
    //List of items picked up
    public List<InventoryItem> items= new List<InventoryItem>();
    //flag indicates if the inventory is open or not
    public bool isOpen;

    [Header("UI Items Section")]
    ////Inventory System Window
    public GameObject ui_Window;
    public Image[] items_images;

    [Header("UI Item Description")]
    public GameObject ui_Description_Window;
    public Image description_Image;
    public Text description_Title;
    public Text description_Text;


    [Header("Detection Fields")]
    //Detection Point
    public Transform detectionPointkey;
    //Detection Radius
    private const float detectionRadiuskey = 0.2f;
    //Detection Layer
    public LayerMask detectionLayerkey;
    //Cached Trigger Object
    public GameObject detectedObjectkey;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ToggleInventory();
        }
        DetectObject1();
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        ui_Window.SetActive(isOpen);

        Update_UI();
    }

    //Add the item to the items list
    public void PickUp(GameObject item)
    {
        //if item is stackable
        if (item.GetComponent<Item>().stackable)
        {
            //Check if we have an existing item in our inventroy
            InventoryItem existingItem = items.Find(x=>x.obj.name==item.name);
            //if yes, stack it
            if (existingItem!=null)
            {
                existingItem.stack++;
            }
            else
            {
                InventoryItem i = new InventoryItem(item);
                items.Add(i);
            }
        }
        //if not stackable
        else
        {
            InventoryItem i = new InventoryItem(item);
            items.Add(i);
        }
        
        Update_UI();
    }

    public bool CanPickUp()
    {
        if (items.Count >= items_images.Length)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //Refresh the UI elements in the inventory window    
    void Update_UI()
    {
        HideAll();
        //For each item in the "items" list 
        //Show it in the respective slot in the "items_images"
        for (int i = 0; i < items.Count; i++)
        {
            items_images[i].sprite = items[i].obj.GetComponent<SpriteRenderer>().sprite;
            items_images[i].gameObject.SetActive(true);
        }
    }

    //Hide all the items ui images
    void HideAll()
    {
        foreach (var i in items_images) { i.gameObject.SetActive(false); }

        HideDescription();
    }

    public void ShowDescription(int id)
    {
        //Set the Image
        description_Image.sprite = items_images[id].sprite;
        //Set the Title
        //if stack sama dengan 1 , just name
        if(items[id].stack==1)
            description_Title.text = items[id].obj.name;
        //if stack >1  name + x jumlah stack
        else
            description_Title.text = items[id].obj.name+" x"+items[id].stack;
        
        //Show the description
        description_Text.text = items[id].obj.GetComponent<Item>().descriptionText;
        //Show the elements
        description_Image.gameObject.SetActive(true);
        description_Title.gameObject.SetActive(true);
        description_Text.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        description_Image.gameObject.SetActive(false);
        description_Title.gameObject.SetActive(false);
        description_Text.gameObject.SetActive(false);
    }



    bool DetectObject1()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPointkey.position, detectionRadiuskey, detectionLayerkey);

        if (obj == null)
        {
            
            detectedObjectkey = null;
            return false;
        }
        else
        {
            
            detectedObjectkey = obj.gameObject;
            
            return true;

        }
    }



    public void Consume(int id)
    {
        if (items[id].obj.GetComponent<Item>().type == Item.ItemType.Consumables)
        {
            Debug.Log($"CONSUMED {items[id].obj.name}");
            //Invoke the cunsume custome event
            items[id].obj.GetComponent<Item>().consumeEvent.Invoke();
            //Reduce the stack Number
            items[id].stack--;
            //if the stack is zero
            if (items[id].stack == 0)
            {
                //Destroy the item in very tiny time
                Destroy(items[id].obj, 0.1f);
                //Clear the item from the list
                items.RemoveAt(id);
            }

            //Update UI
            Update_UI();
        }
        if (items[id].obj.GetComponent<Item>().type == Item.ItemType.Unlocker)
        {
            if(DetectObject1() != false)
            {
                Debug.Log($"UNLOCKED WITH {items[id].obj.name}");

                //Invoke the cunsume custome event
                items[id].obj.GetComponent<Item>().consumeEvent.Invoke();
                //Reduce the stack Number
                items[id].stack--;
                //if the stack is zero
                if (items[id].stack == 0)
                {
                    //Destroy the item in very tiny time
                    Destroy(items[id].obj, 0.1f);
                    //Clear the item from the list
                    items.RemoveAt(id);
                }

                //Update UI
                Update_UI();
            }
            
        }
    }
}