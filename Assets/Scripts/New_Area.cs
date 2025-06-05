using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
public class New_Area : MonoBehaviour
{

    [Header("ID FOR USING WITH EASY SAVE")]
    public string ID;
    public GameObject CameraFocusPoint;
   public enum Area_type
    {
        CashCounter,
        ShoeStorage,
        ShoesStand,
        ShirtStorage,
        ShirtStand,
        ChangingRoom,
        CapsStand,
        HROffice,
        Cashier
    }

    public Area_type Area_to_Unlock;

    public GameObject Area_to_spawn;
    public GameObject buying_area_placeHolder;
    public GameObject Spawn_Point;

    public int CashRequired;
    public int CashCollected;
    public int CashCollected_SavedValue;
    public Image Fill_Image;

    public float FillImage_SavedValue;
    public TextMeshProUGUI PRice_text;
    public bool IsUnlocked;
    public UnityEvent Event;
    
    public List<GameObject> Cash_Taken = new List<GameObject>();
    public GameObject Arrow;

    public float startTime;
    // Start is called before the first frame update
    private void Start()
    {
        PRice_text.text = (CashRequired - CashCollected).ToString();
        if (!IsUnlocked)
        {
            Area_to_spawn.SetActive(false);

            if(CameraFocusPoint)
                if(GameObject.FindObjectOfType<MoveCamera>())
            GameObject.FindObjectOfType<MoveCamera>().MoveToPos(CameraFocusPoint);

        }
        else
        {
            Unlocked();
         //   Area_to_spawn.SetActive(true);
        }
      

        CameraPantoArea();
        

        if (CashCollected > 0)
            CashCollected_SavedValue=CashCollected;

        if (Fill_Image.fillAmount > 0)
            FillImage_SavedValue = Fill_Image.fillAmount;

        //int result = Mathf.RoundToInt(Mathf.Lerp(0, 100, 1f*Time.deltaTime));
        //Debug.Log(result);
        // c = Mathf.Lerp(CashCollected, CashRequired, 1f*Time.deltaTime);
        //    Debug.Log((int)c);
    }
    float transitionDuration = 2f;

     public int cash;

   public int Last_Cash;


    bool islerping;
   public float timeElapsed;
    
    private void Update()
    {
      
     if (this.PlayerTriggered)
     {

       if (GameManager.Instance.Cash_Player > 0)
       {

            if (islerping )
            {
                float progress = Mathf.Clamp01((Time.time - startTime) / transitionDuration);
                //currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, progress));
                Last_Cash = CashCollected;
                Fill_Image.fillAmount = Mathf.Lerp(FillImage_SavedValue, 1, progress);
                CashCollected = Mathf.RoundToInt(Mathf.Lerp(CashCollected_SavedValue, CashRequired, progress));
                PRice_text.text = (CashRequired - CashCollected).ToString();


                if (CashCollected > Last_Cash)
                {
                    GameManager.Instance.Cash_Player -= (CashCollected - Last_Cash);
                    GameManager.Instance.UpdateCash();
                }
                //cash = Mathf.RoundToInt(Mathf.Lerp(GameManager.Instance.Cash_Player, (GameManager.Instance.Cash_Player - CashRequired), progress));
              

                if (progress >= 1.0f  || CashCollected==CashRequired)
                {
                
                    islerping = false;
                    
                    if (!IsUnlocked)
                    this.AreaUnlocked();
                   
                }
                
               
            }
       }
       else
       {
         PlayerTriggered = false;
         islerping = false;
         timeElapsed = Time.time - startTime;
       }
          

           
     }
            
       

     
    }

   





   

    IEnumerator CashAnimation()
    {
        while (islerping && GameManager.Instance.Cash_Player > 0)
        {
            yield return new WaitForSeconds(0.2f);
            GameManager.Instance.DeductCashFromPlayer(this.gameObject);
        }
    }
    void AreaUnlocked()
    {
        GameManager.Instance.NewArea_Unlocked_Sfx();
        //    Instantiate(Area_to_spawn, Spawn_Point.transform.position, Spawn_Point.transform.rotation);
        IsUnlocked = true;
        Area_to_spawn.SetActive(true);
        if (Area_to_spawn.GetComponent<DOTweenAnimation>())
            Area_to_spawn.GetComponent<DOTweenAnimation>().DOPlay() ;
        buying_area_placeHolder.SetActive(false);
        GameManager.Instance.New_Area_Bought();
        GetComponent<BoxCollider>().enabled = false;
        GameManager.Instance.Pool_Acquired_Cash(Cash_Taken);
        Cash_Taken.Clear();
        Event.Invoke();
        Arrow.SetActive(false);
        if (Area_to_spawn.GetComponent<Items_Container_Shelf>())
            Area_to_spawn.GetComponent<Items_Container_Shelf>().AddShelfToGameManager();

        PlayerTriggered = false;
        enabled = false;
        //FirebaseAnalytics.Event("gameplay","unlocked_"+Area_to_Unlock, "unlocked_" + Area_to_Unlock);
        FirebaseAnalytics.Event("new_area_" + Area_to_Unlock.ToString().ToLower() + "_unlocked", "new_area_" + Area_to_Unlock.ToString().ToLower() + "_unlocked", "_unlocked");
        //gameObject.SetActive(false);
    }

    
    void CameraPantoArea()
    {
        if (Area_to_Unlock == Area_type.HROffice)
            GameManager.Instance.player_Cam_Mover.ShowHRRoom();
       
        
    }

   public  bool PlayerTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.Instance.Cash_Player>0)
        {
            if (!islerping)
            {
                islerping = true;
                startTime = Time.time - timeElapsed;
                StartCoroutine(CashAnimation());
            }
          //  startTime = Time.time - remainingTime;
            PlayerTriggered = true;
         //   filval=Fill_Image.fillAmount;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && islerping )
        {
            //GameManager.Instance.UpdateCash(cash);
            PlayerTriggered = false;
            islerping = false;
            timeElapsed = Time.time - startTime;
            //GameManager.Instance.BuyWithCash(CashCollected);
        }
    }


    void Unlocked()
    {
            //    Instantiate(Area_to_spawn, Spawn_Point.transform.position, Spawn_Point.transform.rotation);
            IsUnlocked = true;
            Area_to_spawn.SetActive(true);
            if (Area_to_spawn.GetComponent<DOTweenAnimation>())
                Area_to_spawn.GetComponent<DOTweenAnimation>().DOPlay();
            buying_area_placeHolder.SetActive(false);
         
            GetComponent<BoxCollider>().enabled = false;
          
            Event.Invoke();
            Arrow.SetActive(false);
            if (Area_to_spawn.GetComponent<Items_Container_Shelf>())
                Area_to_spawn.GetComponent<Items_Container_Shelf>().AddShelfToGameManager();

            PlayerTriggered = false;
            enabled = false;

            //gameObject.SetActive(false);
    }
}

