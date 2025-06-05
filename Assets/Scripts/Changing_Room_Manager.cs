using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Changing_Room_Manager : MonoBehaviour
{



    //public Transform[] Waiting_Points;
    public bool ChangingRoomsUnlocked;
    public Changing_Rooms[] ChangingRoom;

    public List<GameObject> Customer_Queue_Positions = new List<GameObject>();
    public List<AI_Customer> Customers_In_Queue;

    public GameObject Customers_VIP;// customers that are using seat now
    bool NoRoomAvailable;
    int Queue_Limit = 4;
    // Start is called before the first frame update


  public  void UnlockRoom(int room)
    {
        ChangingRoom[room].Unlocked = true;
        ChangingRoomsUnlocked=true;
    }
    #region VIP_QUEUE_WORK
    public bool CanAddtoQueue()
    {
        return Customers_In_Queue.Count < Queue_Limit;
    }
    public void AddCustomer(AI_Customer customer)//thiss functions send customer to the waiting queue if seat is not available 
    {
        Debug.Log("GOOOOOOOOOOOOOOO");
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

       
        for (int i = 0; i < ChangingRoom.Length; i++)
        {
         //   Debug.Log(ChangingRoom[i].Unlocked);
            if (ChangingRoom[i].Unlocked && !ChangingRoom[i].Room_In_Use)
            {

                if (!ChangingRoom[i].Room_In_Use && ChangingRoom[i].Trash_Collected)
                {
                    if (!ChangingRoom[i].Customer_Assigned_for_Room )
                    {
                        //  Debug.LogError(i);
                        customer.slot_number = i;// seat number assigned to this customer
                        Customers_VIP = customer.gameObject;

                        ChangingRoom[i].Customer_Assigned_for_Room = customer.gameObject;
                        customer.SetDestination(ChangingRoom[i].Room_Standing_Point.gameObject);
                        Customers_VIP = customer.gameObject;
                        ChangingRoom[i].Room_In_Use = true;
                        NoRoomAvailable = false;
                        break;

                    }
                }
                else
                {
                    NoRoomAvailable = true;
                }
          

                
                
            }
            else
            {
                NoRoomAvailable = true;
            }



        }
        //for (int i = 0; i < ChangingRoom.Length; i++)
        //{
        //    if (ChangingRoom[i].Room_In_Use)
        //    {

        //    }
        //}



        if (NoRoomAvailable)
        {
            AddCustomer(customer); //thiss functions send customer to the waiting queue if seat is not available 

        }
    }

    public bool CheckIfAllSeatsAreOccupied()
    {

        for (int i = 0; i < ChangingRoom.Length; i++)
        {

        }
        return true;
    }
    AI_Customer customer;

    int room_to_use;
    public void Customer_Seating_Scenario(GameObject obj)
    {

        for (int i = 0; i < ChangingRoom.Length; i++)
        {
            if (ChangingRoom[i].Customer_Assigned_for_Room == obj)
            {

        customer = new AI_Customer();
        customer = obj.GetComponent<AI_Customer>();
                room_to_use = i;
                ChangingRoom[i].Room_In_Use = true;
                StartCoroutine(Start_Seating_Scneario1());
             
                break;
            }
        }


    }

    //public void Customer_Seating_Scenario(GameObject obj)
    //{
    //    for (int i = 0; i < Seats_Vip.Length; i++)
    //    {
    //        if (Seats_Vip[i].Customer_Assigned_for_Seat == obj)
    //        {
    //            customer = new AI_Customer();
    //            customer = obj.GetComponent<AI_Customer>();
    //            StartCoroutine(Start_Seating_Scneario1());
    //        }
    //    }

    //}

    IEnumerator Start_Seating_Scneario1()
    {
        int i = room_to_use;
        GameObject go = new GameObject();
        yield return new WaitForSeconds(2f);
       
           

                    ChangingRoom[i].Room_In_Use = true;
                   
                    ChangingRoom[i].Curtain.Play("Curtain_Close");
                    // customer = obj.GetComponent<AI_Customer>();
                    AI_Customer customer_ai = ChangingRoom[i].Customer_Assigned_for_Room.GetComponent<AI_Customer>();

                    go = customer_ai.Player_Object_Holder_Hands.transform.GetChild(0).gameObject;

                    GameManager.Instance.PickAndDropAnimation(go, ChangingRoom[customer_ai.slot_number].Clothes_Placement_Point.gameObject, 5f, 0f);
                    //go.transform.SetParent(ChangingRoom[customer_ai.Changing_Rooms_number].Clothes_Placement_Point);
                    //go.transform.DOLocalJump(new Vector3(0, 0, 0), 5f, 1, 0.5f);
                    //go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                    yield return new WaitForSeconds(1f);
                    customer_ai.Sit_Customer(true);
                    yield return new WaitForSeconds(4f);
                    customer_ai.Sit_Customer(false);
                    yield return new WaitForSeconds(2f);
                    GameManager.Instance.PickAndDropAnimation(go, customer_ai.Player_Object_Holder_Hands, 5f, 0f);
                    //go.transform.SetParent(customer_ai.Player_Object_Holder_Hands.transform);
                    //go.transform.DOLocalJump(new Vector3(0, 0, 0), 10f, 1, 0.5f);
                    //go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                    yield return new WaitForSeconds(2f);
                    ChangingRoom[i].Curtain.Play("Curtain_Open");
                    yield return new WaitForSeconds(1f);
                    customer_ai.ExitCustomer();
                    ChangingRoom[i].Trash_Collected = false;
                    Create_Trash(ChangingRoom[i].Trash_Placement_point, ChangingRoom[i]);
                    Customers_VIP = null;
                    ChangingRoom[i].Customer_Assigned_for_Room = null;
                    ChangingRoom[i].Room_In_Use = false;

                   
                



            
        
        yield return null;
    }

    public GameObject Trash_obj;

    void Create_Trash(GameObject point, Changing_Rooms seat)
    {
        GameObject OB = Instantiate(Trash_obj, point.transform.position, Quaternion.identity);
        OB.transform.SetParent(point.transform);
        point.SetActive(true);
        seat.Trash_On_Seat = OB;
    }
    public void Collect_trash()
    {

        Invoke(nameof(CleanTrash), 0.1f);
    }

    void CleanTrash()
    {
        for (int i = 0; i < ChangingRoom.Length; i++)
        {


            if (!ChangingRoom[i].Trash_Collected)// this bool checks if trash needs to be collected frimm this seat
            {


                if (ChangingRoom[i].Trash_Placement_point.transform.childCount == 0)// this bool if var is null trash was removed from this seat
                {

                    ChangingRoom[i].Trash_Collected = true;
                    ChangingRoom[i].Trash_Placement_point.SetActive(false);
                    break;
                }

            }

        }

        Invoke(nameof(MoveCustomersQueue), 1f);
    }



}

[System.Serializable]
public class Changing_Rooms
{
    public GameObject Customer_Assigned_for_Room;
    public Transform Room_Standing_Point;
    public Transform Clothes_Placement_Point;
    public bool Room_In_Use;
    public GameObject Trash_Placement_point;
    public Animator Curtain;


    public bool Trash_Collected = true;
    public GameObject Trash_On_Seat;
    public bool Unlocked;
}
