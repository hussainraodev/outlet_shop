using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Worker_AI : MonoBehaviour
{
    public enum Customer_State
    {

    }




    GameObject destination_obj;
    public Transform Store_exit_point;
    
    NavMeshAgent navMeshAgent;


    List<GameObject> Player_Item_Holder = new List<GameObject>();
    public GameObject Player_Object_Holder_Hands;
    public GameObject AI_Detection_Trigger;
    bool Object_Picked;

    public Items_Container_Shelf Storage_Shelf_Destination;
    public Items_Container_Shelf Display_Shelf_Destination;


    Animator anim;


    public int Holding_Capacity = 1;
    public float AI_Speed = 1.25f;




    List<Items_Container_Shelf> Items_Shelves = new List<Items_Container_Shelf>();

    List<Items_Container_Shelf> Storage_Items_Shelves = new List<Items_Container_Shelf>();
    List<Items_Container_Shelf> Display_Items_Shelves = new List<Items_Container_Shelf>();
    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Objects_Drop_Trigger");
       
      
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = AI_Speed;
        //if (destination)
        //    SetDestination(destination.position);

        //   billing_Counter.AddCustomer(this);
        GetAvailable_Items_Shelves();
    }
    private void Update()
    {
        // Check if the agent has reached the destination
        if (HasReachedDestination())
        {
     //       Debug.Log("ssssssssssssssssssssssssss");
            // Face the destination point
            SetRotation();
        }
    }

    public void UpdateSpeed()
    {
        AI_Speed += 0.1f;
        navMeshAgent.speed =AI_Speed;
    }

    public void UpdateCapacity()
    {
        Holding_Capacity += 1;
    }
    void GetAvailable_Items_Shelves()
    {

        Storage_Items_Shelves.Clear();
        Display_Items_Shelves.Clear();
        Items_Container_Shelf[] Shelves = GameObject.FindObjectsOfType<Items_Container_Shelf>();

        

        for (int i = 0; i < Shelves.Length; i++)
        {
            if (Shelves[i].TypeOfShelf == Items_Container_Shelf.Shelf_Type.Storage)
            {
                Storage_Items_Shelves.Add(Shelves[i]);
            }
            else
            if (Shelves[i].TypeOfShelf == Items_Container_Shelf.Shelf_Type.Display)
            {
                Display_Items_Shelves.Add(Shelves[i]);
            }
        }

        CheckDisplay_ShelveToRestock();
    }

    void CheckDisplay_ShelveToRestock()// find the shelve thats needs to restock 
    {
        int i = Random.Range(0, Display_Items_Shelves.Count);
            if (Display_Items_Shelves[i].Items_On_Shelf.Count < Display_Items_Shelves[i].Shelf_Capacity)
            {
                Display_Shelf_Destination = Display_Items_Shelves[i];
                GetRespective_StorageShelf();
         
            }
            else
            {
            CheckDisplay_ShelveToRestock();
            }
       

        
        //for (int i = 0; i < Display_Items_Shelves.Count; i++)
        //{
        //    if(Display_Items_Shelves[i].Items_On_Shelf.Count< Display_Items_Shelves[i].Shelf_Capacity)
        //    {
        //        Display_Shelf_Destination = Display_Items_Shelves[i];
        //        break;
        //    }
        //}

        //GetRespective_StorageShelf();
    }

    int storage_shelf_point_number;
    void GetRespective_StorageShelf()// get storage shelf respective to display shlef type
    {
        for (int i = 0; i < Storage_Items_Shelves.Count; i++)
        {
            if (Storage_Items_Shelves[i].TypeOfItem==Display_Shelf_Destination.TypeOfItem)
            {
                Storage_Shelf_Destination = Storage_Items_Shelves[i];
                break;
            }
        }
        for (int i = 0; i < Storage_Shelf_Destination.Customer_Standing_Point.Length; i++)
        {
            if (!Storage_Shelf_Destination.Customer_Standing_Point_In_use[i])
            {
                
                Storage_Shelf_Destination.Customer_Standing_Point_In_use[i] = true;
                SetDestination(Storage_Shelf_Destination.Customer_Standing_Point[i]);
                storage_shelf_point_number = i;
                break;

            }
        }
       // Worker_Storage_Destination();
    }

    void Worker_Storage_Destination()
    {

        SetDestination(Storage_Shelf_Destination.Customer_Standing_Point[0]);
    }
   
    Items_Container_Shelf Interacted_Shelf;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Objects_Pick_Trigger") && !Object_Picked)
        {

            Interacted_Shelf = other.gameObject.GetComponent<Items_Container_Shelf>();
            Invoke(nameof(PickObjectsFrom_Rack), 0.5f);
            Object_Picked = true;
            //Debug.Log()
        }


        if (other.gameObject.CompareTag("Objects_Drop_Trigger") )
        {
            Debug.Log("HASEEB KELA");
            Interacted_Shelf = other.gameObject.GetComponent<Items_Container_Shelf>();
            Invoke(nameof(PlaceObjectsIn_Rack), 0.5f);
            Object_Picked = false;
            //Debug.Log()
        }


        if (other.CompareTag("TrashBin"))
        {
            if(Can_Trash)
            DropTrash(other.gameObject);

        }




    }

    GameObject targetRot;
    void SetRotation()
    {
        this.gameObject.transform.rotation = destination_obj.transform.rotation;
        AI_Detection_Trigger.SetActive(true);
        anim.SetBool("Walking", false);
    }
    public void SetDestination(GameObject destination_point)
    {
        AI_Detection_Trigger.SetActive(false);
        destination_obj = destination_point;
        // Check if the NavMeshAgent component exists
        if (navMeshAgent != null)
        {
            // Set the destination for the NavMeshAgent
            navMeshAgent.SetDestination(destination_point.transform.position);
            anim.SetBool("Walking", true);
        }
    }

    private bool HasReachedDestination()
    {
        return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }
    

     #region Picking_placing_Items
    void PickObjectsFrom_Rack()
    {
        if (Interacted_Shelf.Items_On_Shelf.Count > 0)
        {
            StartCoroutine(Pick_Objs_Rack_Corot());
        }
        else
        {
            Invoke(nameof(PickObjectsFrom_Rack), 5f);
        }
        //StartCoroutine(Pick_Objs_Rack_Corot());
    }
    int count;
    IEnumerator Pick_Objs_Rack_Corot()
    {

        count = Player_Object_Holder_Hands.transform.childCount;
        while (Player_Object_Holder_Hands.transform.childCount < Holding_Capacity && Interacted_Shelf.Items_On_Shelf.Count>0)
        {


        
                GameObject go = Interacted_Shelf.Items_On_Shelf[0];
        
                Interacted_Shelf.Items_On_Shelf[0].transform.SetParent(Player_Object_Holder_Hands.transform);
                 Interacted_Shelf.Items_On_Shelf.RemoveAt(0);
            
                //  Cube.transform.DOLocalMove(new Vector3(0,0,0),1f);
                go.transform.DOLocalJump(new Vector3(0, 1f*count, 0), 1.5f, 1, 0.5f);
                go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);

            
                count++;
            
            yield return new WaitForSeconds(0.05f);
        }

        anim.SetLayerWeight(1, 1f);
        yield return new WaitForSeconds(1f);
        SetDestination(Display_Shelf_Destination.Worker_Standing_Point);
        Interacted_Shelf.Customer_Standing_Point_In_use[storage_shelf_point_number] = false;
        Interacted_Shelf.Items_Count = Interacted_Shelf.Items_On_Shelf.Count;
    }

    void PlaceObjectsIn_Rack()
    {
        StartCoroutine(PlaceObjectsIn_Rack_Corot());
    }
    int count2;
    IEnumerator PlaceObjectsIn_Rack_Corot()
    {
     if(Interacted_Shelf.Items_On_Shelf.Count < Interacted_Shelf.Shelf_Capacity)
     {

        count2 = 0;
        while (Player_Object_Holder_Hands.transform.childCount>0 && Interacted_Shelf.Items_On_Shelf.Count<Interacted_Shelf.Shelf_Capacity)
        {

            int ChildCount = Player_Object_Holder_Hands.transform.childCount;
            //   GameObject go = Instantiate(refiller.Shoes[Random.Range(0, refiller.Shoes.Count)], refiller.transform.position, Quaternion.identity);

            GameObject go = Player_Object_Holder_Hands.transform.GetChild(ChildCount-1).gameObject;
            for (int i = 0; i < Interacted_Shelf.Objects_Placement_Point.Count; i++)
            {
                if (Interacted_Shelf.TypeOfItem.ToString() == go.GetComponent<Item_Script>().TypeOfItem.ToString())//checking if the item in hand is same as the shelf interacted
                {
                    if (Interacted_Shelf.Objects_Placement_Point[i].transform.childCount == 0)
                    {
                        go.transform.SetParent(Interacted_Shelf.Objects_Placement_Point[i].transform);
                        Interacted_Shelf.Items_On_Shelf.Add(go);
                        go.transform.DOLocalJump(new Vector3(0, count2, 0), 1.5f, 1, 1f);
                        go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                        go.GetComponent<Item_Script>().Custom_Event_Place_on_Rack();
                        break;
                    }
                }
                    else
                    {
                        sendToOTherShelf();
                        break;
                    }
                
            }

            yield return new WaitForSeconds(0.2f);
            if (!GameManager.Instance.CanSpawnCustomersNow())
                GameManager.Instance.Start_ShowingCutomers_for_Store();
        }
            //if (Player_Object_Holder_Hands.transform.childCount > 0)
            //{
            //    AI_Detection_Trigger.SetActive(false);
            //    sendToOTherShelf();
            //    AI_Detection_Trigger.SetActive(false);
            //}
            //else
            //{
                

                yield return new WaitForSeconds(1f);
            if (Player_Object_Holder_Hands.transform.childCount == 0)
            {

                anim.SetLayerWeight(1, 0f);
            GetAvailable_Items_Shelves();

            }
            else
            {
                sendToOTherShelf();
                yield return new WaitForSeconds(0.1f);
                if (!OtherShelfAssigned)
                    GoToTrash();
            }
                Interacted_Shelf.Items_Count = Interacted_Shelf.Items_On_Shelf.Count;
          //  }
    
     }
      else
      {
            
            
            if (Interacted_Shelf.TypeOfItem.ToString() == Player_Object_Holder_Hands.transform.GetChild(0).gameObject.GetComponent<Item_Script>().TypeOfItem.ToString())
                Invoke(nameof(PlaceObjectsIn_Rack),5f);
            else
            {
                sendToOTherShelf();
            }
            


      }

    }
    bool OtherShelfAssigned;
    #endregion
    void sendToOTherShelf()
    {
        if (Player_Object_Holder_Hands.transform.GetChild(Player_Object_Holder_Hands.transform.childCount - 1).gameObject.GetComponent<Item_Script>().TypeOfItem == Item_Script.Itemp_Type.Shirt)
        {
            GetShirtShelf();
        }
        else
                   if (Player_Object_Holder_Hands.transform.GetChild(Player_Object_Holder_Hands.transform.childCount - 1).gameObject.GetComponent<Item_Script>().TypeOfItem == Item_Script.Itemp_Type.Shoes)
        {
            GetShoeShelf();
        }
        else
                   if (Player_Object_Holder_Hands.transform.GetChild(Player_Object_Holder_Hands.transform.childCount-1).gameObject.GetComponent<Item_Script>().TypeOfItem == Item_Script.Itemp_Type.Ball)
        {
            GetBallShelf();
        }
        else
                   if (Player_Object_Holder_Hands.transform.GetChild(Player_Object_Holder_Hands.transform.childCount - 1).gameObject.GetComponent<Item_Script>().TypeOfItem == Item_Script.Itemp_Type.Cap)
        {
            GetCapShelf();
        }
    }

    void GetShirtShelf()
    {
        

        for (int i = 0; i < Display_Items_Shelves.Count; i++)
        {
            if(Display_Items_Shelves[i].TypeOfItem == Items_Container_Shelf.Item_Type_to_Store.Shirt)
            {

               if (Display_Items_Shelves[i].Items_On_Shelf.Count < Display_Items_Shelves[i].Shelf_Capacity)
               {
                Display_Shelf_Destination = Display_Items_Shelves[i];
                SetDestination(Display_Shelf_Destination.Worker_Standing_Point);
                OtherShelfAssigned = true;
                break;
               }
               else
                    OtherShelfAssigned = false;
            }
        }
        
        //else
        //{
        //    CheckDisplay_ShelveToRestock();
        //}
    }

    void GetShoeShelf()
    {


        for (int i = 0; i < Display_Items_Shelves.Count; i++)
        {
            if (Display_Items_Shelves[i].TypeOfItem == Items_Container_Shelf.Item_Type_to_Store.Shoes)
            {

                if (Display_Items_Shelves[i].Items_On_Shelf.Count < Display_Items_Shelves[i].Shelf_Capacity)
                {
                    Display_Shelf_Destination = Display_Items_Shelves[i];
                    SetDestination(Display_Shelf_Destination.Worker_Standing_Point);
                    OtherShelfAssigned = true;
                    break;
                }
                else
                    OtherShelfAssigned = false;
            }
        }

        //else
        //{
        //    CheckDisplay_ShelveToRestock();
        //}
    }

    void GetCapShelf()
    {


        for (int i = 0; i < Display_Items_Shelves.Count; i++)
        {
            if (Display_Items_Shelves[i].TypeOfItem == Items_Container_Shelf.Item_Type_to_Store.Cap)
            {

                if (Display_Items_Shelves[i].Items_On_Shelf.Count < Display_Items_Shelves[i].Shelf_Capacity)
                {
                    Display_Shelf_Destination = Display_Items_Shelves[i];
                    SetDestination(Display_Shelf_Destination.Worker_Standing_Point);
                    OtherShelfAssigned = true;
                    break;
                }
                else
                    OtherShelfAssigned = false;
            }
        }

        //else
        //{
        //    CheckDisplay_ShelveToRestock();
        //}
    }

    void GetBallShelf()
    {


        for (int i = 0; i < Display_Items_Shelves.Count; i++)
        {
            if (Display_Items_Shelves[i].TypeOfItem == Items_Container_Shelf.Item_Type_to_Store.Ball)
            {

                if (Display_Items_Shelves[i].Items_On_Shelf.Count < Display_Items_Shelves[i].Shelf_Capacity)
                {
                    Display_Shelf_Destination = Display_Items_Shelves[i];
                    SetDestination(Display_Shelf_Destination.Worker_Standing_Point);
                    OtherShelfAssigned = true;
                    break;
                }
                else
                    OtherShelfAssigned = false;
            }
        }

        //else
        //{
        //    CheckDisplay_ShelveToRestock();
        //}
    }

    bool Can_Trash;
    void GoToTrash()
    {
        GameObject obj=GameObject.FindGameObjectWithTag("TrashBin");
        if (obj)
        {
            Can_Trash = true;
            SetDestination(obj);
        }
    }

    public void DropTrash(GameObject bin)
    {
        int c = Player_Object_Holder_Hands.transform.childCount;
        for (int i = 0; i < c; i++)
        {
            
            GameManager.Instance.PickAndDropAnimation(Player_Object_Holder_Hands.transform.GetChild(0).gameObject,
                bin, 3f, 0f);
        }
        anim.SetLayerWeight(1, 0f);
        GetAvailable_Items_Shelves();
        Can_Trash = false;

    }
}
