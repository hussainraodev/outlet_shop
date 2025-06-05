using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Items_Container_Shelf : MonoBehaviour
{

    [Header("ID FOR USING WITH EASY SAVE")]
    public string ID;


    public enum Shelf_Type
    {
        Storage,
        Display
    }
    public enum Item_Type_to_Store
    {
        Shoes,
        Shirt,
        Cap,
        Ball
    }

    public Shelf_Type TypeOfShelf;
    public Item_Type_to_Store TypeOfItem;
    public int Shelf_Capacity=12;
    public List<Transform> Objects_Placement_Point;
    public GameObject[] Customer_Standing_Point;

    public bool[] Customer_Standing_Point_In_use;
    public GameObject Worker_Standing_Point;

    public  List<GameObject> Items_On_Shelf=new List<GameObject>();
    public int Items_Count;
    public bool Customer_is_Using;

    public GameObject Shadow_Sprite;
    Vector3 Default_Shadow_Scale;
    Vector3 Target_Shadow_Scale;
    // Start is called before the first frame update
    void Start()
    {
       
        if (TypeOfShelf == Shelf_Type.Storage)
        {
            Invoke(nameof(FillRackOnStart), 0.5f);
            //   FillRackOnStart();
        }
       else
            if (TypeOfShelf == Shelf_Type.Display)
        {
            if (Items_Count > 0)
            {
                FillRackItemsSaved();
            }


           
        }

        if (Shadow_Sprite)
        {
            Default_Shadow_Scale = Shadow_Sprite.transform.localScale;
            Target_Shadow_Scale = Default_Shadow_Scale;
            Target_Shadow_Scale.x += 0.2f;
            Target_Shadow_Scale.y += 0.2f;
        }

    }

    public void TriggerShadow(bool ScaleUp)
    {
        if (Shadow_Sprite)
        {
        if(ScaleUp)
        Shadow_Sprite.transform.DOScale(Target_Shadow_Scale, 0.25f);
        else
            Shadow_Sprite.transform.DOScale(Default_Shadow_Scale, 0.25f);

        }
    }
    public void AddShelfToGameManager()
    {
        if (TypeOfShelf == Shelf_Type.Display)
            GameManager.Instance.Add_New_Unlocked_Shelf(this);
    }
   
    void FillRackItemsSaved()
    {
        count = 0;
        while (Items_On_Shelf.Count < Items_Count)
        {
            // Debug.LogError(count);
            if (Objects_Placement_Point[count].transform.childCount == 0)
            {
                GameObject go=null;

                if (TypeOfItem == Item_Type_to_Store.Shoes)
                     go = Instantiate(refiller.Shoes[Random.Range(0, refiller.Folded_Shirts.Count)], refiller.transform.position, Quaternion.identity);
                else
              if (TypeOfItem == Item_Type_to_Store.Shirt)
                     go = Instantiate(refiller.Folded_Shirts[Random.Range(0, refiller.Folded_Shirts.Count)], refiller.transform.position, Quaternion.identity);
                if (TypeOfItem == Item_Type_to_Store.Cap)
                    go = Instantiate(refiller.Caps[Random.Range(0, refiller.Caps.Count)], refiller.transform.position, Quaternion.identity);
                if (TypeOfItem == Item_Type_to_Store.Ball)
                    go = Instantiate(refiller.Balls[Random.Range(0, refiller.Balls.Count)], refiller.transform.position, Quaternion.identity);



                if(go.GetComponent<Item_Script>())
                    go.GetComponent<Item_Script>().Custom_Event_Place_on_Rack();
                go.transform.SetParent(Objects_Placement_Point[count].transform);
                //  Cube.transform.DOLocalMove(new Vector3(0,0,0),1f);

                go.transform.localPosition = Vector3.zero;
                go.transform.DOScale(Vector3.one, 1f);
                go.transform.localEulerAngles = Vector3.zero;
                

                Items_On_Shelf.Add(go);
                count++;
               
            }
            else
            {
                count++;
               
            }

        }
       
        //InvokeRepeating(nameof(Refill_Rack), 2f, 20f);
    }

    void CustomfunctionSavedObject()
    {

    }

    void FillRackOnStart()
    {
        count = 0;
        while (Items_On_Shelf.Count < Shelf_Capacity)
        {
            // Debug.LogError(count);
            if (Objects_Placement_Point[count].transform.childCount == 0)
            {
                GameObject go = null;

                if (TypeOfItem == Item_Type_to_Store.Shoes)
                    go = Instantiate(refiller.Shoes[Random.Range(0, refiller.Folded_Shirts.Count)], refiller.transform.position, Quaternion.identity);
                else
              if (TypeOfItem == Item_Type_to_Store.Shirt)
                    go = Instantiate(refiller.Folded_Shirts[Random.Range(0, refiller.Folded_Shirts.Count)], refiller.transform.position, Quaternion.identity);
                if (TypeOfItem == Item_Type_to_Store.Cap)
                    go = Instantiate(refiller.Caps[Random.Range(0, refiller.Caps.Count)], refiller.transform.position, Quaternion.identity);
                if (TypeOfItem == Item_Type_to_Store.Ball)
                    go = Instantiate(refiller.Balls[Random.Range(0, refiller.Balls.Count)], refiller.transform.position, Quaternion.identity);


                go.transform.SetParent(Objects_Placement_Point[count].transform);
                //  Cube.transform.DOLocalMove(new Vector3(0,0,0),1f);

                go.transform.localPosition = Vector3.zero;
                go.transform.DOScale(Vector3.one, 1f);

                Items_On_Shelf.Add(go);
                count++;

            }
            else
            {
                count++;

            }

        }

        //InvokeRepeating(nameof(Refill_Rack), 2f, 20f);
    }

    public void Pick_Objects_From_Rack(Player_Controller player)
    {
        int count = player.Player_Object_Holder_Hands.transform.childCount - player.Holding_Capacity;
    }

    public Item_Stock_Refiller refiller;
    int count;
    public void Refill_Rack()
    {
       
        if(TypeOfItem==Item_Type_to_Store.Shoes  || TypeOfItem == Item_Type_to_Store.Cap || TypeOfItem == Item_Type_to_Store.Ball)
        StartCoroutine(Refill_Rack_Corot());
        else
              if (TypeOfItem == Item_Type_to_Store.Shirt)
            StartCoroutine(Refill_Shirts_Rack_Corot());
    }


    IEnumerator Refill_Rack_Corot()
    {

        count = 0;
        while (Items_On_Shelf.Count < Shelf_Capacity)
        {
           // Debug.LogError(count);
            if (Objects_Placement_Point[count].transform.childCount == 0)
            {
                GameObject go=null;
                if (TypeOfItem == Item_Type_to_Store.Shoes)
                    go = Instantiate(refiller.Shoes[Random.Range(0, refiller.Shoes.Count)], refiller.transform.position, Quaternion.identity);
                else
                    if (TypeOfItem == Item_Type_to_Store.Ball)
                    go = Instantiate(refiller.Balls[Random.Range(0, refiller.Balls.Count)], refiller.transform.position, Quaternion.identity);
                else
                    if (TypeOfItem == Item_Type_to_Store.Cap)
                    go = Instantiate(refiller.Caps[Random.Range(0, refiller.Caps.Count)], refiller.transform.position, Quaternion.identity);

                go.transform.SetParent(Objects_Placement_Point[count].transform);
                //  Cube.transform.DOLocalMove(new Vector3(0,0,0),1f);
                go.transform.DOLocalJump(new Vector3(0, 0, 0), 2f, 1, 1f);
                go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);

                Items_On_Shelf.Add(go);
                count++;
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                count++;
                yield return new WaitForSeconds(0f);
            }
                
        }
    }

    IEnumerator Refill_Shirts_Rack_Corot()
    {

        count = 0;
        while (Items_On_Shelf.Count < Shelf_Capacity)
        {
            // Debug.LogError(count);
            if (Objects_Placement_Point[count].transform.childCount == 0)
            {

                GameObject go = Instantiate(refiller.Folded_Shirts[Random.Range(0, refiller.Folded_Shirts.Count)], refiller.transform.position, Quaternion.identity);

                go.transform.SetParent(Objects_Placement_Point[count].transform);
                //  Cube.transform.DOLocalMove(new Vector3(0,0,0),1f);
                go.transform.DOLocalJump(new Vector3(0, 0, 0), 2f, 1, 1f);
                go.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);

                Items_On_Shelf.Add(go);
                count++;
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                count++;
                yield return new WaitForSeconds(0f);
            }

        }
    }

    GameObject AvailablePoint=null;
  public  GameObject Available_Standing_Point()
  {
        for (int i = 0; i < Customer_Standing_Point.Length; i++)
        {
            if (!Customer_Standing_Point_In_use[i])
            {
                AvailablePoint = Customer_Standing_Point[i];

               
                break;
            }
            else
                AvailablePoint = null;
        }

        return AvailablePoint;
  }
    public GameObject Cam_point;
    public void CheckIfCustomersAreWaiting()
    {
        if (Available_Standing_Point() == null && Items_On_Shelf.Count == 0)
        {
         
            MoveCamera cam = GameObject.FindObjectOfType<MoveCamera>();
            if (cam != null && Cam_point)
            {
                cam.MoveToPos_Rot(Cam_point);
            }
        }
        
    }


    public void CustomerWaiting_checker()
    {
        Invoke(nameof(CheckIfCustomersAreWaiting),15f);
    }
}
