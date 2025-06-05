using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class AI_Customer : MonoBehaviour
{
    public enum Customer_State {
    
    }

  


     GameObject destination_obj;
    public Transform Store_exit_point;
     NavMeshAgent navMeshAgent;


     List<GameObject> Player_Item_Holder = new List<GameObject>();
    public GameObject Player_Object_Holder_Hands;
    public GameObject AI_Detection_Trigger;
    bool Object_Picked;


    Animator anim;


    public int Holding_Capacity = 1;
 

    Customer_Billing_Manager[] billing_Counter;

 List<Items_Container_Shelf>Items_Shelves=new List<Items_Container_Shelf>();

    public Items_Container_Shelf Shelf_Assigned_GameManager;
    public bool Vip_Customer;
    Vip_Seating_Manager vip_manager;
    Changing_Room_Manager Changing_Room_manager;
    [HideInInspector]
    public int slot_number;

    private void OnEnable()
    {
        //ChangeMats();
        ChangeBodyVariation();
        anim = GetComponent<Animator>();
        
        billing_Counter = GameObject.FindObjectsOfType<Customer_Billing_Manager>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        //Invoke(nameof(GetAvailable_Items_Shelves), 1f);

        if (Vip_Customer)
        {
            vip_manager = GameObject.FindObjectOfType<Vip_Seating_Manager>();
        }

        GetAvailable_Items_Shelves();
        InvokeRepeating(nameof(CheckDestination_Reached), 1f, 0.5f);
    }
     
   
    // Start is called before the first frame update
    void Start()
    {

        
        //anim = GetComponent<Animator>();
        //GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Objects_Drop_Trigger");
        //billing_Counter = GameObject.FindObjectsOfType<Customer_Billing_Manager>();
      
        //navMeshAgent = GetComponent<NavMeshAgent>();

       
        //GetAvailable_Items_Shelves();

        //if (Vip_Customer)
        //{
        //    vip_manager = GameObject.FindObjectOfType<Vip_Seating_Manager>();
        //}

        //InvokeRepeating(nameof(CheckDestination_Reached), 1f, 0.5f);

        
    }

    void CheckDestination_Reached()
    {
        if (gameObject.activeInHierarchy)
        {

        if (HasReachedDestination())
        {
            Debug.Log("ssssssssssssssssssssssssss");
            // Face the destination point
            SetRotation();
        }
        }
    }
   
    void GetAvailable_Items_Shelves()
    {
        Items_Container_Shelf[]  Shelves = GameObject.FindObjectsOfType<Items_Container_Shelf>();

        for (int i = 0; i < Shelves.Length; i++)
        {
            if (Shelves[i].TypeOfShelf == Items_Container_Shelf.Shelf_Type.Display)
            {
                Items_Shelves.Add(Shelves[i]);
            }
        }

        Customer_Initital_Destination();
    }
    int assigned_shelf_Point;
    bool Dest_Assigned;
    void Customer_Initital_Destination()
    {
        //for (int i = 0; i < Items_Shelves.Count; i++)
        //{
        //    if (!Items_Shelves[i].Customer_is_Using)
        //    {
        //        SetDestination(Items_Shelves[i].Customer_Standing_Point[0]);
        //        Items_Shelves[i].Customer_is_Using = true;
        //        assigned_shelf = i;
        //        break;
        //    }
        //}
    
            for (int j = 0; j < Shelf_Assigned_GameManager.Customer_Standing_Point.Length; j++)
            {
                if (!Shelf_Assigned_GameManager.Customer_Standing_Point_In_use[j])
                {
                    SetDestination(Shelf_Assigned_GameManager.Customer_Standing_Point[j]);
                    Shelf_Assigned_GameManager.Customer_Standing_Point_In_use[j] = true;
                    assigned_shelf_Point = j;
                    Shelf_Assigned_GameManager.CustomerWaiting_checker();
                    Dest_Assigned = true;
                    break;
                }
            }
        
            
        
            
    }
    public void ExitCustomer()
    {
        if (navMeshAgent != null)
        {
            // Set the destination for the NavMeshAgent
            SetDestination(GameManager.Instance.Stores[0].StoreExitPoint.gameObject);
            if (!GameManager.Instance.Billing_Tut)
            {
                GameManager.Instance.DisplayTaskNotification();
                GameManager.Instance.Billing_Tut = true;
            }
        }
    }
    Items_Container_Shelf Interacted_Shelf;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Objects_Drop_Trigger") && !Object_Picked)
        {

            Interacted_Shelf = other.gameObject.GetComponent<Items_Container_Shelf>();
            Invoke(nameof(PickObjectsFrom_Rack), 0.5f);
            Object_Picked = true;
            //Debug.Log()
        }
        
        if (other.gameObject.CompareTag("AI_Standing_Point"))
        {
            //if (destination == other.gameObject.transform.position)
            //{
            //    targetRot = other.gameObject;
            //    Invoke(nameof(SetRotation), 0.5f);
            //}
        }

        if (other.gameObject.CompareTag("Destroy_Customer"))
        {
            navMeshAgent.Stop();
            Object_Picked = false;
            GameManager.Instance.Stores[0].Return_Customer_to_Pool(this.gameObject);
            GameManager.Instance.Stores[0].Return_Box_to_Pool(Player_Object_Holder_Hands.transform.GetChild(0).gameObject);
            
        }


        if (other.gameObject.CompareTag("Vip_Seating") && vip_manager)
        {
            vip_manager.Customer_Seating_Scenario(this.gameObject);
        }

        if (other.gameObject.CompareTag("InsideChangingRoom") && Changing_Room_manager)
        {
            Changing_Room_manager.Customer_Seating_Scenario(this.gameObject);
        }
    }

    GameObject targetRot;

    public void Sit_Customer(bool toggle)
    {
        anim.SetBool("Sit", toggle);
        if(toggle)
        anim.SetLayerWeight(1, 0f);
        else
            anim.SetLayerWeight(1, 1f);
    }
    void SetRotation()
    {
        this.gameObject.transform.rotation = destination_obj.transform.rotation;
        AI_Detection_Trigger.SetActive(true);
        anim.SetBool("Walking", false);


    }
    public void SetDestination(GameObject destination_point)
    {
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
    void PickObjectsFrom_Rack()
    {
       // Debug.Log(this.name);
        if (Interacted_Shelf.Items_On_Shelf.Count > 0)
        {
            StartCoroutine(Pick_Objs_Rack_Corot());
        }
        else
        {
            Invoke(nameof(PickObjectsFrom_Rack), 5f);
        }
        
    }
    int count;
    IEnumerator Pick_Objs_Rack_Corot()
    {
        
        count = Player_Object_Holder_Hands.transform.childCount;
        GameObject go=null;
        while (Player_Object_Holder_Hands.transform.childCount < Holding_Capacity)
        {


        
           // Debug.Log(count + "dddddd");
           go = Interacted_Shelf.Items_On_Shelf[count];
           
            Interacted_Shelf.Items_On_Shelf[count].transform.SetParent(Player_Object_Holder_Hands.transform);
            Interacted_Shelf.Items_On_Shelf.RemoveAt(count);
            go.GetComponent<Item_Script>().Custom_Event_Pick_from_DisplayRack();

            go.transform.DOLocalJump(new Vector3(0, count, 0), 10f, 1, 0.5f);
            go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);

        
            count++;

            yield return new WaitForSeconds(0.05f);
        }
        if (!GameManager.Instance.Wait_Tut)
        {
            GameManager.Instance.DisplayTaskNotification();
            GameManager.Instance.Wait_Tut = true;
          //  GameManager.Instance.player_Cam_Mover.ShowCustomer();
            
        }
        Interacted_Shelf.Items_Count = Interacted_Shelf.Items_On_Shelf.Count;
        anim.SetLayerWeight(1, 1f);
        yield return new WaitForSeconds(0.5f);
        if (Vip_Customer )
        {
            if( go.GetComponent<Item_Script>().TypeOfItem == Item_Script.Itemp_Type.Shoes)
            {
                vip_manager = GameObject.FindObjectOfType<Vip_Seating_Manager>();


                if (vip_manager)
                {
                    if (vip_manager.CanAddtoQueue())
                    {
                        Interacted_Shelf.Customer_is_Using = false;
                        vip_manager.Add_Customer_To_VIP(this);
                        Interacted_Shelf.Customer_Standing_Point_In_use[assigned_shelf_Point] = false;
                    }
                    else
                        SendCustomerForBilling();
                }
                else
                    SendCustomerForBilling();
            }
            else
                if (go.GetComponent<Item_Script>().TypeOfItem == Item_Script.Itemp_Type.Shirt)
            {
              
                Changing_Room_manager = GameObject.FindObjectOfType<Changing_Room_Manager>();


                if (Changing_Room_manager && Changing_Room_manager.ChangingRoomsUnlocked)
                {
                    if (Changing_Room_manager.CanAddtoQueue())
                    {
                        Interacted_Shelf.Customer_is_Using = false;
                    Changing_Room_manager.Add_Customer_To_VIP(this);
                        Interacted_Shelf.Customer_Standing_Point_In_use[assigned_shelf_Point] = false;
                    }
                    else
                        SendCustomerForBilling();
                }
                else
                    SendCustomerForBilling();
            }
            else { SendCustomerForBilling(); }


        }
        
        else
        {
            SendCustomerForBilling();
        }
        AI_Detection_Trigger.SetActive(false);
       
    }


    void SendCustomerForBilling()
    {
        int rand = Random.Range(0, billing_Counter.Length);
        if(billing_Counter[rand].CanAddCutomers())
        {
            billing_Counter[rand].AddCustomer(this);
            Interacted_Shelf.Customer_Standing_Point_In_use[assigned_shelf_Point] = false;
        }
        else
        {
            Invoke(nameof(SendCustomerForBilling), 5f);
        }
    }


    #region BODY_COLOR 
    public SkinnedMeshRenderer rend;
    public Color[] AvailableShirtColors;
    public Material[] MaterialsOnCharacter;



    void ChangeMats()
    {
        MaterialsOnCharacter = rend.materials;

        for (int i = 0; i < MaterialsOnCharacter.Length; i++)
        {
            if (MaterialsOnCharacter[i].name.Contains("Shirt"))
            {
                MaterialsOnCharacter[i].color = AvailableShirtColors[Random.Range(0, AvailableShirtColors.Length)] ;
            }

        }

    }
    #endregion

    #region Change MAts
    public Material[] Mat_Variations;
  


    void ChangeBodyVariation()
    {
        rend.material = Mat_Variations[Random.Range(0, Mat_Variations.Length)];
    }
    #endregion
}
