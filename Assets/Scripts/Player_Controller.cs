using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Invector.vCharacterController;
public class Player_Controller : MonoBehaviour
{

    vThirdPersonController controller;
    public List<GameObject> Player_Item_Holder = new List<GameObject>();
    public GameObject Player_Object_Holder_Hands;
    public GameObject Player_Object_Holder_Cash;

    public List<GameObject> Player_Cash_Objects = new List<GameObject>();


    public int Holding_Capacity=4;
    Animator anim;

    GameObject cash;

    public GameObject Arrow;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Dev");
        anim=GetComponent<Animator>();
        controller = gameObject.GetComponent<vThirdPersonController>();
        for (int i = 0; i < GameManager.Instance.Cash_Player; i++)
        {
            cash = GameManager.Instance.Stores[GameManager.Instance.Curren_Store - 1].GetPooledCash();
            cash.transform.SetParent(Player_Object_Holder_Cash.transform);
            cash.transform.localScale = Vector3.zero;
            Player_Cash_Objects.Add(cash);
        }

        if (GameManager.Instance.CanSpawnCustomersNow())
            GameManager.Instance.Start_ShowingCutomers_for_Store();
        //Invoke(nameof(DoTween), 1f);
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.Billing_Tut )
        {
            Arrow.SetActive(true);
            Arrow.transform.LookAt(GameManager.Instance.CurrentActiveBuyArea().transform.position, Vector3.up);
        }
        else
            Arrow.SetActive(false);
    }

    // Update is called once per frame
    public void UpgradeSpeedOfPlayer()
    {
        if (controller.freeSpeed.runningSpeed < 6)
            controller.freeSpeed.runningSpeed += 1;
    }
    public void UpgradeCapacityOfPlayer()
    {
        if (Holding_Capacity<6)
        Holding_Capacity += 1;
    }

    public void AddCashtoPlayer(GameObject cashObj)
    {
        Player_Cash_Objects.Add(cashObj);
        cashObj.transform.SetParent(Player_Object_Holder_Cash.transform);
        cashObj.transform.DOLocalJump(new Vector3(0,-1.5f, 0), 3.5f, 1, 0.25f);
        cashObj.transform.DOScale(Vector3.zero, 2f);
        GameManager.Instance.Add_Cash();
    }

    public void DeductCashFromPlayer(GameObject target)
    {
        if (Player_Cash_Objects.Count > 0)
        {
            if (target.GetComponent<New_Area>()) // if buying new area add cash objects to the area list and set parent of cash object
            {
                target.GetComponent<New_Area>().Cash_Taken.Add(Player_Cash_Objects[0]);
                Player_Cash_Objects[0].transform.SetParent(target.GetComponent<New_Area>().buying_area_placeHolder.transform);
            }
            Player_Cash_Objects[0].transform.localScale = new Vector3(0.8f, .8f, .8f);
            
            Player_Cash_Objects[0].transform.DOLocalJump(new Vector3(0, -1f, 0), 3f, 1,0.5f);
            //   Player_Cash_Objects[0].transform.DOScale(Vector3.zero, 2f);
           
            
            Player_Cash_Objects.RemoveAt(0);
            
            //Invoke(nameof(RemoveFromList), 3f);

        }
    }
    

   

    Items_Container_Shelf Interacted_Shelf;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Objects_Pick_Trigger"))
        {
            if (CanPick)
            {
                Interacted_Shelf = other.gameObject.GetComponent<Items_Container_Shelf>();
                PickObjectsFrom_Rack();
                //Debug.LogError("in object pick");
                CanPick = false;
                
            }
            if (other.gameObject.GetComponent<Items_Container_Shelf>())
                other.gameObject.GetComponent<Items_Container_Shelf>().TriggerShadow(true);
            //Debug.Log()
        }
        else
            if (other.gameObject.CompareTag("Objects_Drop_Trigger"))
        {

            Interacted_Shelf = other.gameObject.GetComponent<Items_Container_Shelf>();
            PlaceObjectsIn_Rack();
            if (other.gameObject.GetComponent<Items_Container_Shelf>())
                other.gameObject.GetComponent<Items_Container_Shelf>().TriggerShadow(true);
            //Debug.Log()
        }

        if (other.gameObject.CompareTag("Trash"))
        {
            if (Player_Object_Holder_Hands.transform.childCount < Holding_Capacity)// check if it can pickup Trash
            {
              
                Pick_Trash(other.gameObject);
                
                GameObject.FindObjectOfType<Vip_Seating_Manager>().Collect_trash();

                //Debug.Log()
            }

        }
        if (other.gameObject.CompareTag("Trash2"))
        {
            if (Player_Object_Holder_Hands.transform.childCount < Holding_Capacity)// check if it can pickup Trash
            {
            
                Pick_Trash(other.gameObject);
              
                GameObject.FindObjectOfType<Changing_Room_Manager>().Collect_trash();

             
            }

        }

        if (other.CompareTag("TrashBin"))
        {

            DropTrash(other.gameObject);

        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Objects_Pick_Trigger") || other.gameObject.CompareTag("Objects_Drop_Trigger"))
        {
            
            if (other.gameObject.GetComponent<Items_Container_Shelf>())
                other.gameObject.GetComponent<Items_Container_Shelf>().TriggerShadow(false);
            //Debug.Log()
        }
    }


    void OnTriggerStay(Collider other)
    {
        // Check if the triggering GameObject has the specified tag
        if (other.CompareTag("BuyArea"))
        {

      //   other.GetComponent<New_Area>().collect_Cash();
        }
    }

    #region Picking_placing_Items

public    bool CanPick=true;
    void PickObjectsFrom_Rack()
    {
       
        count = Player_Object_Holder_Hands.transform.childCount;
        // StartCoroutine(Pick_Objs_Rack_Corot());
        Pick_Objs_Rack_Corot();
           
      
    }
    int count;
    GameObject go;
    void Pick_Objs_Rack_Corot()
    {


        if (count < Holding_Capacity && Interacted_Shelf.Items_On_Shelf.Count > 0)
        {
        //    Debug.Log(count + "===" + Holding_Capacity + "===" + Interacted_Shelf.Items_On_Shelf.Count);


          //  go = Interacted_Shelf.Items_On_Shelf[0];

            Interacted_Shelf.Items_On_Shelf[0].transform.SetParent(Player_Object_Holder_Hands.transform);


         
            Interacted_Shelf.Items_On_Shelf[0].transform.DOLocalJump(new Vector3(0, 0.25f * count, 0), 1.5f, 1, 0.5f);
            Interacted_Shelf.Items_On_Shelf[0].transform.DOLocalRotate(new Vector3(0, 0, 0), 1);


            GameManager.Instance.ItemPick_DropSfx();
            if (!GameManager.Instance.Shoes_Tut && Player_Object_Holder_Hands.transform.childCount == Holding_Capacity)
            {
                Debug.Log("WTF");
                GameManager.Instance.DisplayTaskNotification();
            }
            Debug.Log("before remoce" + Interacted_Shelf.Items_On_Shelf.Count);
            Interacted_Shelf.Items_On_Shelf.Remove(Interacted_Shelf.Items_On_Shelf[0]);
            Interacted_Shelf.Items_On_Shelf.Capacity=Interacted_Shelf.Items_On_Shelf.Count;
            Debug.Log("after remoce" + Interacted_Shelf.Items_On_Shelf.Count);

            //yield return new WaitForSeconds(0.5f);
            
            count++;

            Invoke(nameof(Pick_Objs_Rack_Corot), 0.1f);

        }
        else
            CanPick = true;


       Interacted_Shelf.Items_Count = Interacted_Shelf.Items_On_Shelf.Count;
        anim.SetLayerWeight(3, 1f);

        
    }

    //IEnumerator Pick_Objs_Rack_Corot()
    //{


    //    while (count < Holding_Capacity && Interacted_Shelf.Items_On_Shelf.Count > 0)
    //    {
    //        Debug.Log(count + "===" + Holding_Capacity + "===" + Interacted_Shelf.Items_On_Shelf.Count);


    //        GameObject go = Interacted_Shelf.Items_On_Shelf[0];
    //        Interacted_Shelf.Items_On_Shelf.RemoveAt(0);
    //        go.transform.SetParent(Player_Object_Holder_Hands.transform);


    //        //  Cube.transform.DOLocalMove(new Vector3(0,0,0),1f);
    //        go.transform.DOLocalJump(new Vector3(0, 0.25f * count, 0), 1.5f, 1, 0.5f);
    //        go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
    //        GameManager.Instance.ItemPick_DropSfx();


    //        if (!GameManager.Instance.Shoes_Tut && Player_Object_Holder_Hands.transform.childCount == Holding_Capacity)
    //        {
    //            Debug.Log("WTF");
    //            GameManager.Instance.DisplayTaskNotification();
    //        }
    //        yield return new WaitForSeconds(0.1f);
    //        count++;
    //    }

    //    anim.SetLayerWeight(3, 1f);
    //}

    void PlaceObjectsIn_Rack()
    {
        StartCoroutine(PlaceObjectsIn_Rack_Corot());
    }
    int count2;
    IEnumerator PlaceObjectsIn_Rack_Corot()
    {
        Debug.Log("Llalalaalaallalaalaalalalalall");
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
                        go.transform.DOLocalJump(new Vector3(0, count2, 0), 1.5f, 1, 0.5f);
                        go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                        go.GetComponent<Item_Script>().Custom_Event_Place_on_Rack();

                        GameManager.Instance.ItemPick_DropSfx();
                        break;
                    }
                }
                
            }





            //  Cube.transform.DOLocalMove(new Vector3(0,0,0),1f);


            //  Items_On_Shelf.Add(go);
            //  count++;

            if (!GameManager.Instance.Shoes_Tut)
            {
                GameManager.Instance.Shoes_Tut = true;
                GameManager.Instance.DisplayTaskNotification();
                
                //transform.position.x= new Vector3(10,0,0);
            }

            yield return new WaitForSeconds(0.05f);
            if (!GameManager.Instance.CanSpawnCustomersNow())
                GameManager.Instance.Start_ShowingCutomers_for_Store();
        }
        if(Player_Object_Holder_Hands.transform.childCount==0)
        anim.SetLayerWeight(3, 0f);

        Interacted_Shelf.Items_Count = Interacted_Shelf.Items_On_Shelf.Count;

        if (Player_Object_Holder_Hands.transform.childCount<Holding_Capacity)// force can pick
        CanPick = true;



    }

    bool HasTrash;
    public void Pick_Trash(GameObject obj)
    {
        
        GameManager.Instance.PickAndDropAnimation(obj, Player_Object_Holder_Hands, 3f, 0.2f*Player_Object_Holder_Hands.transform.childCount);
        anim.SetLayerWeight(3, 1f);
        HasTrash = true;
        obj.GetComponent<BoxCollider>().enabled = false;
    }
    public void DropTrash(GameObject bin)
    {
        int c= Player_Object_Holder_Hands.transform.childCount;
        for (int i = 0; i <c; i++)
        {
            Debug.Log(Player_Object_Holder_Hands.transform.childCount + "dddddddddddddddddddddddd");
            GameManager.Instance.PickAndDropAnimation(Player_Object_Holder_Hands.transform.GetChild(0).gameObject,
                bin, 3f, 0f);
        }
        anim.SetLayerWeight(3, 0f);
        HasTrash = false;
    }

    #endregion

}
