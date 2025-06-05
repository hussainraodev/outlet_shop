using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Customer_Billing_Manager : MonoBehaviour
{
    public GameObject CustomerStandingPoint;// point ehre the first customer stands
    public List<GameObject> Customer_Queue_Positions = new List<GameObject>();
    public List<AI_Customer> Customers_In_Queue;
  
    public int Queue_Limit;
    public GameObject Temp_obj;
    public GameObject Box__Spawn_Point;
    public Cash_Counter Cash_Handler;
    public bool HasCashierAI;
     bool PlayerCashier;

    public GameObject BuyCashierArea;
    public GameObject Cashier;
    public SpriteRenderer Billing_Indicator;
    // Start is called before the first frame update
    void Awake()
    {
        CreateQueuePositons();
    }


    private void Start()
    {
        GameManager.Instance.Stores[GameManager.Instance.Curren_Store-1].InitializeBoxesPool();
        //GameManager.Instance.Stores[GameManager.Instance.Curren_Store - 1].InitializeCashesPool();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {

     //   Debug.Log(other.gameObject.name);
        
        if(other.gameObject.CompareTag("Player") && !HasCashierAI)
        {
            PlayerCashier = true;
            Start_Billing();
            Billing_Indicator.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider other)
    {

      //  Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player") && !HasCashierAI)
        {
            PlayerCashier = false;
            Billing_Indicator.color = Color.red;
            // Start_Billing();
        }
    }
    public void ActivateCashier()
    {
        HasCashierAI = true;
        Billing_Indicator.color = Color.green;
        Start_Billing();
    }
    #region Billing Scenario
    void Start_Billing()
    {
        if (Customers_In_Queue.Count > 0 && (HasCashierAI || PlayerCashier) )
        {
            if (Customers_In_Queue[0] != null)
            {

                Debug.Log(IsCorotRunning);
                if(!IsCorotRunning)
                StartCoroutine(Billing_Corot());
            }
        }
    }

    GameObject Current_Box;

   public  bool IsCorotRunning;// using this bool to check if the corot is alreadyy runnning before calling it on trigger enter
    IEnumerator Billing_Corot()
    {

        IsCorotRunning = true;
        yield return new WaitForSeconds(1f);
        if (!PlayerCashier && !HasCashierAI)// exit corotune if player exited the trigger before 1 second stay
            yield return null;
        Get_Box();
        
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.Billing_Sfx();
        if (Customers_In_Queue[0].Player_Object_Holder_Hands.transform.childCount > 0)
        {
            GameObject go = Customers_In_Queue[0].Player_Object_Holder_Hands.transform.GetChild(0).gameObject;
            go.transform.SetParent(Current_Box.transform);
            go.transform.DOLocalJump(new Vector3(0, 0, 0), 100f, 1, 0.5f);
            go.transform.DOScale(new Vector3(go.transform.localScale.x/2, go.transform.localScale.y / 2, go.transform.localScale.z / 2),1f);
            go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
            yield return new WaitForSeconds(1f);
            Current_Box.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            Current_Box.transform.SetParent(Customers_In_Queue[0].Player_Object_Holder_Hands.transform);
            Current_Box.transform.DOLocalJump(new Vector3(0, 0, 0), 10f, 1, 0.5f);
            Current_Box.transform.DOLocalRotate(new Vector3(0, 90, 0), 1);
            // go.transform.DOScale(new Vector3(go.transform.localScale.x / 2, go.transform.localScale.y / 2, go.transform.localScale.z / 2), 1f);
            go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
            yield return new WaitForSeconds(1f);
            ProcessCustomerBill();
            
        }
        IsCorotRunning = false;
        Start_Billing();
    }
    void Get_Box()
    {
        Current_Box = GameManager.Instance.Stores[0].GetPooledBox();
        Current_Box.transform.SetParent(Box__Spawn_Point.transform);
        Current_Box.transform.localPosition = Vector3.zero;
        Current_Box.transform.localScale = Vector3.zero;
        Current_Box.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Current_Box.SetActive(true);
        Current_Box.transform.DOScale(new Vector3(0.009f, 0.009f, 0.009f), 0.5f);
    }
    #endregion
    void CreateQueuePositons()
    {
       //Vector3 te=new Vector3();
       // Transform pos = new Transform();
        for (int i = 0; i < Queue_Limit; i++)
        {
            GameObject cube = Instantiate(Temp_obj);
            cube.GetComponent<BoxCollider>().enabled = false;
            cube.GetComponent<MeshRenderer>().enabled=false;
            cube.transform.SetParent(this.gameObject.transform);
            //  cube.transform.position = cube.transform.position.x* i;
            
            cube.transform.localPosition = new Vector3(CustomerStandingPoint.transform.localPosition.x + i, CustomerStandingPoint.transform.localPosition.y,
                CustomerStandingPoint.transform.localPosition.z);
            cube.transform.localRotation = CustomerStandingPoint.transform.localRotation;
            Customer_Queue_Positions.Add(cube);
            //Vector3 temp = Customer_Queue_Positions[i].transform.position;
            //temp.z = i + 1;
            //Customer_Queue_Positions[i].transform.position = temp;
     //       Debug.Log(Customer_Queue_Positions[i].transform.localPosition);

        }
    }


    public bool CanAddCutomers()
    {
        return Customers_In_Queue.Count < Queue_Limit;
    }


    public void AddCustomer(AI_Customer customer)
    {
        if (CanAddCutomers())
        {
            Customers_In_Queue.Add(customer);
            Customers_In_Queue[Customers_In_Queue.Count - 1].SetDestination(Customer_Queue_Positions[Customers_In_Queue.Count - 1]);
            Invoke(nameof(Start_Billing), 3f);

        }
    }

    public void ProcessCustomerBill()
    {
        if (Customers_In_Queue.Count > 0)
        {
            Customers_In_Queue[0].ExitCustomer();
            Customers_In_Queue.RemoveAt(0);
            MoveCustomersQueue();
            Cash_Handler.AddObjects();
        }
    }

    public void MoveCustomersQueue()
    {
        if (Customers_In_Queue.Count > 0)
        {
            for (int i = 0; i < Customers_In_Queue.Count; i++)
            {
                Customers_In_Queue[i].SetDestination(Customer_Queue_Positions[i]);
            }
        }
    }



    private void OnDrawGizmos()
    {
        for (int i = 0; i < Customer_Queue_Positions.Count; i++)
        {
           // Debug.Log("QTF"+i);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Customer_Queue_Positions[i].transform.position, 0.2f);
        }

    }



    #region PollingForBoxes
  
    #endregion
}
