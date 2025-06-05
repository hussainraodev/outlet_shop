using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public bool ClearData;
    public GameObject LoadingScreen;
    public SoundManager Sfx_Manager;
    public GameObject Cash_bar_ui;
    TextMeshProUGUI CashBarText;
    public GameObject Notification_Bar;
    TextMeshProUGUI text_field;
    Animator anim;
    public GameObject Player;
    public Player_Controller player_controller;
    public MoveCamera player_Cam_Mover;
    public int Cash_Player;
    public Store[] Stores;
    public int Curren_Store;
    public GameObject[] Customers_AI;
    private static GameManager _instance;
    public bool Shoes_Tut;
    public bool Wait_Tut;
    public bool Billing_Tut;
    // Public property to access the instance
    public static GameManager Instance
    {
        get
        {
            // If the instance doesn't exist, create it
            if (_instance == null)
            {
                // Create a new GameObject to hold the Singleton script
                GameObject singletonObject = new GameObject("GameManager");

                // Attach the Singleton script to the new GameObject
                _instance = singletonObject.AddComponent<GameManager>();

                // Ensure that the GameObject persists between scenes
                DontDestroyOnLoad(singletonObject);
            }

            return _instance;
        }
    }

    private void Awake()
    {

        //if(ClearData)
        //    ES3AutoSaveMgr.Current.SA
        
        ES3AutoSaveMgr.Current.Load();
        _instance = this;

#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }

    private void Start()
    {
        if (Notification_Bar)
        {
            anim = Notification_Bar.GetComponent<Animator>();
            text_field = Notification_Bar.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }

        if (Cash_bar_ui)
        {
            CashBarText = Cash_bar_ui.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        //Stores[Curren_Store-1].InitializeCashesPool();
        //Stores[Curren_Store - 1].InitializeCustomerPool();
        StartCoroutine(PoolingDelay());



        if (!Stores[Curren_Store - 1].InitialCashTaken)
        New_Area_Bought();
        else
            DisplayTaskNotification_SavedIndex();

        Invoke(nameof(LoadingDOne), 6f);
        Invoke(nameof(ShowAds), 120f); //commented by hussain
        UpdateCash();
        
    }

    IEnumerator PoolingDelay()
    {
        yield return new WaitForSeconds(1f);
        Stores[Curren_Store - 1].InitializeCashesPool();
        yield return new WaitForSeconds(1f);
        Stores[Curren_Store - 1].InitializeCustomerPool();
    }

    int count=5;
    public TextMeshProUGUI AdsDelayText;
    void ShowAds()
    {
        StartCoroutine(ShowAds_Delay());
        AdsDelayText.gameObject.SetActive(true);
    }
    IEnumerator ShowAds_Delay()
    {
        if (count == 0)
        {
           
            
            Invoke(nameof(ShowAds), 180f);
            count = 5;
            AdsDelayText.gameObject.SetActive(false);
            //MyAdsManager.instance.ShowInterstitialAds();//ads commented by hussain
            yield return null;
        }
        else
        {
            AdsDelayText.text = "Showing Ad in :" + count;
            yield return new WaitForSeconds(1f);
            count--;
            StartCoroutine(ShowAds_Delay());
        }
    }

    void LoadingDOne()
    {
        LoadingScreen.SetActive(false);
    }
    public void ItemPick_DropSfx()
    {
        Sfx_Manager.Item_Picked_Droped();
    }

    public void NewArea_Unlocked_Sfx()
    {
        Sfx_Manager.NewAreaUnlocked();
    }

    public void Billing_Sfx()
    {
        Sfx_Manager.CustomerBillingMachine();
    }

    public void PickMoney_Sfx()
    {
        Sfx_Manager.PickMoney();
    }
    public int Task_Count;
    public void DisplayTaskNotification()
    {
        if (Notification_Bar )
        {
            if (text_field && Task_Count < Stores[Curren_Store - 1].Tasks_Notifications_text.Length) {
                text_field.text = Stores[Curren_Store - 1].Tasks_Notifications_text[Task_Count];
                anim.Play("NewTask");
                Debug.Log(Task_Count);
                Task_Count ++;
                
            }
        }
    }

    public void DisplayTaskNotification_SavedIndex()
    {
        if (Notification_Bar)
        {
            if (text_field && Task_Count < Stores[Curren_Store - 1].Tasks_Notifications_text.Length)
            {
                text_field.text = Stores[Curren_Store - 1].Tasks_Notifications_text[Task_Count-1];
                anim.Play("NewTask");
               
            //    Task_Count++;

            }
        }
    }

    public void Add_Cash()
    {
        Cash_Player += 2;
        CashBarText.text = Cash_Player.ToString();
    }
    public void Add_Cash_Reward(int amount)
    {
        Cash_Player += amount;
        CashBarText.text = Cash_Player.ToString();
    }

    public void Deduct_Cash()
    {
       
        Cash_Player -= 1;
        CashBarText.text = Cash_Player.ToString();
    }
    public void UpdateCash()
    {
        
        CashBarText.text = Cash_Player.ToString();
    }
    public void LerpCashValueUI(int val) {
      CashBarText.text = val.ToString();
    }

    public void BuyWithCash(int amount)
    {
        Cash_Player -= amount;
        CashBarText.text = Cash_Player.ToString();
    }
    // Optional: Add OnDestroy to nullify the instance when the application is closed
    private void OnDestroy()
    {
        _instance = null;
    }

    public void Add_New_Unlocked_Shelf(Items_Container_Shelf shelf)
    {
        Stores[Curren_Store - 1].Add_Shelves(shelf);
    }
    public void New_Area_Bought()
    {
        //Invoke(nameof(DisplayTaskNotification), 0f);
        DisplayTaskNotification();
        Stores[Curren_Store - 1].ActivateNewBuyArea();
        Invoke(nameof(SaveGame), 1f);

        // DisplayTaskNotification();
    }

    public GameObject CurrentActiveBuyArea()
    {
      return  Stores[Curren_Store - 1].BuyAreasStore[Stores[Curren_Store - 1].Areas_Bought-1];
            
    }
    void SaveGame()
    {
        ES3AutoSaveMgr.Current.Save();
    }
    public void Upgrade_Player_Capacity(int amount)
    {
        player_controller.UpgradeCapacityOfPlayer();
        BuyWithCash(amount);
    }
    public void Upgrade_Player_Speed(int amount)
    {
        player_controller.UpgradeSpeedOfPlayer();
        BuyWithCash(amount);
    }
    public void AddCashToPlayer(GameObject cash)
    {
        player_controller.AddCashtoPlayer(cash);
        //GameManager.Instance.ItemPick_DropSfx();

    }

    public void DeductCashFromPlayer(GameObject target)
    {
        player_controller.DeductCashFromPlayer(target);
        GameManager.Instance.ItemPick_DropSfx();
    }

    public void Pool_Acquired_Cash(List<GameObject> cash_list)
    {
        Stores[Curren_Store-1].Return_Acquired_Cash_List_To_Poll(cash_list);
    }

    public void Start_ShowingCutomers_for_Store()
    {
      
        Invoke(nameof(CustomerEntry), 1.5f);
        Stores[Curren_Store - 1].Activate_Customers = true;
        InvokeRepeating(nameof(Spawn_Customers), 1.6f,20f);
    }

    void CustomerEntry()
    {
        GameManager.Instance.player_Cam_Mover.ShowCustomer();
    }

    public void SpawnDelay(GameObject obj)
    {

    }
    //IEnumerator


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Spawn_Customers();
        }
    }
    public void Spawn_Customers()
    {
       // Stores[Curren_Store-1].Spawn_Customers_For_Store();
        StartCoroutine(Stores[Curren_Store - 1].Spawn_Customers_For_Store());
        //  Instantiate(Customers_AI[Random.Range(0, Customers_AI.Length)], Stores[0].StoreEntrancePoint.position, Stores[0].StoreEntrancePoint.rotation);
    }

    public void AddWorker(Transform spawn, int price, bool REWARDED_uNLOCK)
    {
        Debug.Log("WTF"+ Cash_Player.ToString()+ price.ToString());
        if (Cash_Player > price || REWARDED_uNLOCK)
        {
            Debug.Log("WTF");
            Stores[Curren_Store - 1].SpawnWorker_AI(spawn);
            if(!REWARDED_uNLOCK)
            BuyWithCash(price);
        }
    }
    public void UpgradeWorkersSpeed(int price, bool REWARDED_uNLOCK)
    {
        if (Cash_Player > price || REWARDED_uNLOCK)
        {
            Stores[Curren_Store - 1].UpgradeWorkersSpeed();
            if (!REWARDED_uNLOCK)
                BuyWithCash(price);
        }
     
    } 
    public void UpgradeWorkersCapacity(int price, bool REWARDED_uNLOCK)
    {
        if (Cash_Player > price || REWARDED_uNLOCK)
        {
            Stores[Curren_Store - 1].UpgradeWorkersCapacity();
            if (!REWARDED_uNLOCK)
                BuyWithCash(price);
        }
    }
    public int GetWorkersCount()
    {
        return Stores[Curren_Store - 1].Workers_spawned;
    }

    public void PickAndDropAnimation(GameObject object_toAnimate, GameObject ParentObj,float jumpVal,float customYvalue)
    {
        object_toAnimate.transform.SetParent(ParentObj.transform);
        object_toAnimate.transform.DOLocalJump(new Vector3(0, customYvalue, 0), jumpVal, 1, 0.5f);
        object_toAnimate.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
    }

    public bool CanSpawnCustomersNow()
    {
        return Stores[Curren_Store - 1].Activate_Customers;
    }
    [System.Serializable]
    public class Store
    {
    
        public bool Activate_Customers;// bool sto check if can spawn customers
        public GameObject Initial_Cash_Stack;
        public bool InitialCashTaken;
        public int Areas_Bought;
        public GameObject[] BuyAreasStore;
        public string[] Tasks_Notifications_text;
        public Transform StoreExitPoint;
        public Transform StoreEntrancePoint;
        public GameObject Worker_AI;
        public GameObject Worker_Spawn_Point;
        public List<Worker_AI> Workers_Bought;
        public int Workers_spawned;
        float WorkerSpeed=1.5f;
        int WorkerCapacity=1;
        
        //public List<GameObject> Customers=new List<GameObject>();
        List<Items_Container_Shelf> Item_shelves=new List<Items_Container_Shelf>();



        public void Add_Shelves(Items_Container_Shelf shelve)
        {
            Item_shelves.Add(shelve);
        }

        public void Spawn_Customers_For_Store1()
        {
           
            
        }
     
      public IEnumerator Spawn_Customers_For_Store()
        {

            for (int i = 0; i < Item_shelves.Count; i++)
            {
               
                for (int j= 0; j < Item_shelves[i].Customer_Standing_Point.Length; j++)
                {
                    yield return new WaitForSeconds(1f);
                    if (!Item_shelves[i].Customer_Standing_Point_In_use[j])
                    {
                    //  GameObject obj =Instantiate(Customer_Prefabs[Random.Range(0, Customer_Prefabs.Length)], StoreEntrancePoint.position, StoreEntrancePoint.rotation);
                        GameObject obj = GetPooledCustomer();
                        //  obj.SetActive(true);
                         obj.GetComponent<AI_Customer>().Shelf_Assigned_GameManager= Item_shelves[i];
                       // obj.GetComponent<AI_Customer>().AssignShelf(Item_shelves[i]);
                        obj.SetActive(true);

                        // obj.GetComponent<AI_Customer>().AssignShelf(Item_shelves[i]);

                        //obj = null;
                        break;
                    }
                }
            
            }
           
        }
         public void ActivateNewBuyArea()
        {

            if(Areas_Bought< BuyAreasStore.Length)
            BuyAreasStore[Areas_Bought].SetActive(true);
            Areas_Bought++;
        }

        public void SpawnWorker_AI(Transform SpawnPoint)
        {
            if (Workers_spawned < 3)
            {
                GameObject obj = Instantiate(Worker_AI,SpawnPoint.position, Quaternion.identity);
               
                obj.GetComponent<Worker_AI>().AI_Speed = WorkerSpeed;
                obj.GetComponent<Worker_AI>().Holding_Capacity = WorkerCapacity;
                Workers_Bought.Add(obj.GetComponent<Worker_AI>());
                Workers_spawned++;
            }
          
        }

        public void UpgradeWorkersCapacity()
        {
            for (int i = 0; i < Workers_Bought.Count; i++)
            {
                Workers_Bought[i].UpdateCapacity();
                
            }
            WorkerCapacity += 1;
        }

        public void UpgradeWorkersSpeed()
        {
            for (int i = 0; i < Workers_Bought.Count; i++)
            {
                Workers_Bought[i].UpdateSpeed();
                 
            }
            WorkerSpeed += 0.5f;
        
        }

        #region PollingForBoxes
        [Header("BOX POOLER")]
        public GameObject BoxPrefab;  // The prefab to pool
        public int Boxes_poolSize = 10;        // The number of bullets to initially instantiate

        List<GameObject> Box_Pool = new List<GameObject>();

        public GameObject Box_Placement_Point;

        public void InitializeBoxesPool()
        {
            for (int i = 0; i < Boxes_poolSize; i++)
            {
                GameObject box = Instantiate(BoxPrefab, Box_Placement_Point.transform.position,Quaternion.identity);
                box.transform.SetParent(Box_Placement_Point.transform);
                box.SetActive(false); // Deactivate the bullet initially
                Box_Pool.Add(box);
            }
        }
        public void Return_Box_to_Pool(GameObject box)
        {
            box.transform.SetParent(Box_Placement_Point.transform);
            box.SetActive(false);
            box.transform.GetComponentInChildren<Item_Script>().gameObject.SetActive(false);
            box.transform.GetChild(0).gameObject.SetActive(false);
        }
        // Method to get a bullet from the pool
        public GameObject GetPooledBox()
        {
            for (int i = 0; i < Box_Pool.Count; i++)
            {
                if (!Box_Pool[i].activeInHierarchy)
                {
                    return Box_Pool[i];
                }
            }

            // If no inactive bullet is found, create a new one and add it to the pool
            GameObject newbox = Instantiate(BoxPrefab);
            newbox.SetActive(false);
            Box_Pool.Add(newbox);
            return newbox;
        }
        #endregion


        #region PollingForCash
        [Header("CASH POOLER")]
        public GameObject CashPrefab;  // The prefab to pool
        public int Cashes_poolSize = 10;        // The number of bullets to initially instantiate


         List<GameObject> Cash_Pool = new List<GameObject>();

        public GameObject Cash_Placement_Point;

        public void InitializeCashesPool()
        {
            for (int i = 0; i < Cashes_poolSize; i++)
            {
                GameObject Cash = Instantiate(CashPrefab, Cash_Placement_Point.transform.position, CashPrefab.transform.rotation);
                Cash.transform.SetParent(Cash_Placement_Point.transform);
                Cash.SetActive(false); // Deactivate the bullet initially
                Cash_Pool.Add(Cash);
            }

       //    Initial_Cash_Stack.SetActive(true);
        }
      public void Return_Acquired_Cash_List_To_Poll(List<GameObject> cash_list)
        {
            for (int i = 0; i < cash_list.Count; i++)
            {
                cash_list[i].transform.SetParent(Cash_Placement_Point.transform);
                cash_list[i].SetActive(false);
                Cash_Pool.Add(cash_list[i]);
            }
        }
        public void Return_Cash_to_Pool(GameObject Cash)
        {
            Cash.transform.SetParent(Cash_Placement_Point.transform);
            Cash.SetActive(false);
            Cash.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Cash.transform.eulerAngles = new Vector3(0, 90, 0);
            //Cash.transform.GetComponentInChildren<Item_Script>().gameObject.SetActive(false);
           // Cash.transform.GetChild(0).gameObject.SetActive(false);
        }
        // Method to get a bullet from the pool
        int i;
        public GameObject GetPooledCash()
        {
            for ( i = 0; i < Cash_Pool.Count; i++)
            {
                if (!Cash_Pool[i].activeInHierarchy)
                {
                  //  Debug.Log("LOLOLO");
                    Cash_Pool[i].SetActive(true);
                    GameObject obj = Cash_Pool[i];
                    Cash_Pool.RemoveAt(i);
                    return obj;
                }

            }

            if (i == Cash_Pool.Count )
            {

                // If no inactive bullet is found, create a new one and add it to the pool
                GameObject newCash = Instantiate(CashPrefab);
                newCash.SetActive(false);
                Cash_Pool.Add(newCash);
                return newCash;
            }
            else
                return null;
            
        }
        #endregion

        #region PollingForCustomer
        [Header("CASH POOLER")]
        public GameObject[] Customer_Prefabs;  // The prefab to pool
        public int Customer_poolSize = 10;        // The number of bullets to initially instantiate


        List<GameObject> Customer_Pool = new List<GameObject>();

        public GameObject Customer_Placement_Point;

        public void InitializeCustomerPool()
        {
            for (int i = 0; i < Customer_poolSize; i++)
            {
                
                GameObject Customer = Instantiate(Customer_Prefabs[Random.Range(0, Customer_Prefabs.Length)], StoreEntrancePoint.transform.position, StoreEntrancePoint.transform.rotation);
                //Customer.transform.SetParent(Cash_Placement_Point.transform);
                Customer.SetActive(false); // Deactivate the bullet initially
                Customer_Pool.Add(Customer);
            }

            //    Initial_Cash_Stack.SetActive(true);
        }
        //public void Return_Acquired_Customer_List_To_Poll(List<GameObject> cash_list)
        //{
        //    for (int i = 0; i < cash_list.Count; i++)
        //    {
        //        cash_list[i].transform.SetParent(Cash_Placement_Point.transform);
        //        cash_list[i].SetActive(false);
        //        Cash_Pool.Add(cash_list[i]);
        //    }
        //}
        public void Return_Customer_to_Pool(GameObject Customer)
        {
           // Customer.transform.SetParent(Customer_Placement_Point.transform);
            
            
            Customer.GetComponent<Animator>().SetLayerWeight(1, 0f);
            Customer.SetActive(false);
            Customer_Pool.Add(Customer);
            Customer.transform.position = StoreEntrancePoint.transform.position;
           
        }
        // Method to get a bullet from the pool
        int j;
        public GameObject GetPooledCustomer()
        {
            for (j = 0; j < Customer_Pool.Count; j++)
            {
                if (!Customer_Pool[j].activeInHierarchy)
                {
                    //  Debug.Log("LOLOLO");
                    //Customer_Pool[j].SetActive(true);
                    GameObject obj = Customer_Pool[j];
                    Customer_Pool.RemoveAt(j);
                    return obj;
                }

            }

            if (i == Customer_Pool.Count)
            {

                // If no inactive bullet is found, create a new one and add it to the pool
                GameObject NewCustomer = Instantiate(Customer_Prefabs[Random.Range(0, Customer_Prefabs.Length)]);
                NewCustomer.SetActive(false);
                Customer_Pool.Add(NewCustomer);
                return NewCustomer;
            }
            else
                return null;

        }
        #endregion
    }


    private void OnApplicationQuit()
    {
       // ES3AutoSaveMgr.Current.Save();
    }
}
