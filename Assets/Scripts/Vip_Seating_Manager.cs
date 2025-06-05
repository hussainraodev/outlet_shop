using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Vip_Seating_Manager : MonoBehaviour
{



    //public Transform[] Waiting_Points;

    public Vip_Seat[] Seats_Vip;

    public List<GameObject> Customer_Queue_Positions = new List<GameObject>();
    public List<AI_Customer> Customers_In_Queue;

    public GameObject Customers_VIP;// customers that are using seat now
    bool seat_available;
    int Queue_Limit = 4;

    public Cash_Counter CashReward;
    // Start is called before the first frame update

    #region VIP_QUEUE_WORK
    public bool CanAddtoQueue()
    {
        return Customers_In_Queue.Count < Queue_Limit;
    }
    public void AddCustomer(AI_Customer customer)//thiss functions send customer to the waiting queue if seat is not available 
    {
        if (CanAddtoQueue())
        {
            Customers_In_Queue.Add(customer);
            Customers_In_Queue[Customers_In_Queue.Count - 1].SetDestination(Customer_Queue_Positions[Customers_In_Queue.Count - 1]);
            //Invoke(nameof(Start_Billing), 3f);

        }
    }

    public void MoveCustomersQueue()
    {
        if (Customers_In_Queue.Count > 0)
        {
            Add_Customer_To_VIP(Customers_In_Queue[0]);
            Customers_In_Queue.RemoveAt(0);
            for (int i = 0; i < Customers_In_Queue.Count; i++)
            {
               
                Customers_In_Queue[i].SetDestination(Customer_Queue_Positions[i]);
            }
        }
    }
    #endregion
    public void Add_Customer_To_VIP(AI_Customer customer)// directly send customer to seat if its available
    {
        
      // if(Customers_In_Queue.co)
        for (int i = 0; i < Seats_Vip.Length; i++)
        {
           
            if(!Seats_Vip[i].Customer_Assigned_for_Seat && Seats_Vip[i].Trash_Collected)
            {
              //  Debug.LogError(i);
                customer.slot_number = i;// seat number assigned to this customer
                Customers_VIP=customer.gameObject;
             
                Seats_Vip[i].Customer_Assigned_for_Seat = customer.gameObject;
                customer.SetDestination(Seats_Vip[i].Seat_Sitting_Point.gameObject);
                Customers_VIP = customer.gameObject;
                seat_available = true;
                break;
             
            }
            else { seat_available = false; }

        }
        if(!seat_available)
        {
            AddCustomer(customer); //thiss functions send customer to the waiting queue if seat is not available 
        }
    }

    public bool CheckIfAllSeatsAreOccupied()
    {

        for (int i = 0; i < Seats_Vip.Length; i++)
        {

        }
        return true;
    }
    AI_Customer customer;
    public void Customer_Seating_Scenario(GameObject obj)
    {
        for (int i = 0; i < Seats_Vip.Length; i++)
        {
            if (Seats_Vip[i].Customer_Assigned_for_Seat == obj)
            { 
              customer = new AI_Customer();
              customer = obj.GetComponent<AI_Customer>();
              StartCoroutine(Start_Seating_Scneario1());
            }
        }
       
    }

    IEnumerator Start_Seating_Scneario1()
    {
        Debug.Log("1");
        GameObject go;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < Seats_Vip.Length; i++)
        {
            if (Seats_Vip[i].Customer_Assigned_for_Seat && !Seats_Vip[i].Seat_In_Use)
            {
                
               // if (Seats_Vip[i].Customer_Assigned_for_Seat == customer.gameObject)
              //  {

                    // customer = obj.GetComponent<AI_Customer>();
                    AI_Customer customer_ai= Seats_Vip[i].Customer_Assigned_for_Seat.GetComponent<AI_Customer>();
                    Seats_Vip[i].Seat_In_Use = true;

                    go = customer_ai.Player_Object_Holder_Hands.transform.GetChild(0).gameObject;

                    GameManager.Instance.PickAndDropAnimation(go, Seats_Vip[customer_ai.slot_number].Shoes_Placement_Point.gameObject, 5f,0f);
                    //go.transform.SetParent(Seats_Vip[customer_ai.vip_seat_number].Shoes_Placement_Point);
                    //go.transform.DOLocalJump(new Vector3(0, 0, 0), 5f, 1, 0.5f);
                    //go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                    yield return new WaitForSeconds(1f);
                    customer_ai.Sit_Customer(true);
                    yield return new WaitForSeconds(4f);
                    customer_ai.Sit_Customer(false);
                    yield return new WaitForSeconds(2f);
                    GameManager.Instance.PickAndDropAnimation(go, customer_ai.Player_Object_Holder_Hands, 5f,0f);
                    //go.transform.SetParent(customer_ai.Player_Object_Holder_Hands.transform);
                    //go.transform.DOLocalJump(new Vector3(0, 0, 0), 10f, 1, 0.5f);
                    //go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                    yield return new WaitForSeconds(2f);
                    customer_ai.ExitCustomer();
                    Seats_Vip[i].Trash_Collected = false;
                    Create_Trash(Seats_Vip[i].Trash_Placement_point, Seats_Vip[i]);

                    Seats_Vip[i].Customer_Assigned_for_Seat = null;
                    Seats_Vip[i].Seat_In_Use = false;
                    CashReward.AddObjects();
                    break;
              //  }


              
            }
        }
        yield return null;
    }

    public GameObject Trash_obj;

    void Create_Trash(GameObject point,Vip_Seat seat)
    {
       GameObject OB= Instantiate(Trash_obj, point.transform.position, Quaternion.identity);
        OB.transform.SetParent(point.transform);
        seat.Trash_On_Seat = OB;
    }
    public void Collect_trash()
    {

        Invoke(nameof(CleanTrash), 0.1f);
    }

    void CleanTrash()
    {
        for (int i = 0; i < Seats_Vip.Length; i++)
        {


            if (!Seats_Vip[i].Trash_Collected)// this bool checks if trash needs to be collected frimm this seat
            {
            
              
                if (Seats_Vip[i].Trash_Placement_point.transform.childCount == 0)// this bool if var is null trash was removed from this seat
                {
               
                    Seats_Vip[i].Trash_Collected = true;
                    Seats_Vip[i].Trash_On_Seat = null;
                    // Seats_Vip[i].Trash_Placement_point.SetActive(false);
                    break;
                }

            }

        }

        Invoke(nameof(MoveCustomersQueue), 1f);
    }    



}

[System.Serializable]
public class Vip_Seat
{
    public GameObject Customer_Assigned_for_Seat;
    public Transform Seat_Sitting_Point;
    public Transform Shoes_Placement_Point;
    public bool Seat_In_Use;
    public GameObject Trash_Placement_point;
   

    public bool Trash_Collected=true;
   public GameObject Trash_On_Seat;
}
