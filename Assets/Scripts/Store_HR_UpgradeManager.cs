using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Store_HR_UpgradeManager : MonoBehaviour
{


    public Transform Worker_SpawnPoint;
    public GameObject HR_UI;
    public GameObject Upgrade_Speed_Progress;
    public GameObject Upgrade_Capacity_Progress;
    public GameObject Upgrade_Workers_Progress;

    public Button HireWorker, HireWorker_Rewarded, HireWorker_Max,
        UpgradeSpeed, UpgradeSpeed_Rewarded, UpgradeSpeed_Max,
        UpgradeCapacity, UpgradeCapacity_Rewraded, UpgradeCapacity_Max;


    public int[] Worker_Price, SpeedPrice, CapacityPrice;

    public int Workers_count;
    public int Speed_Upgrades_Count;
    public int Capacity_Upgrades_Count;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Refresh_UI()
    {
        HireWorker.transform.GetComponentInChildren<Text>().text = Worker_Price[Workers_count].ToString();
        UpgradeCapacity.transform.GetComponentInChildren<Text>().text = CapacityPrice[Capacity_Upgrades_Count].ToString();
        UpgradeSpeed.transform.GetComponentInChildren<Text>().text = SpeedPrice[Speed_Upgrades_Count].ToString();
        if (Workers_count < 3)
        {

           if (GameManager.Instance.Cash_Player > Worker_Price[Workers_count] )
           {
           
                HireWorker.interactable = true;
         
           }
           else
            HireWorker.interactable = false;
        }
        else
        {
            HireWorker.gameObject.SetActive(false);
            HireWorker_Rewarded.gameObject.SetActive(false);
            HireWorker_Max.gameObject.SetActive(true);
        }

        if (Speed_Upgrades_Count < 5)
        {
            if (GameManager.Instance.Cash_Player > SpeedPrice[Speed_Upgrades_Count] && GameManager.Instance.GetWorkersCount() > 0)
            {
                UpgradeSpeed.interactable = true;

            }
            else
                UpgradeSpeed.interactable = false;

        }
        else
        {
            UpgradeSpeed.gameObject.SetActive(false);
            UpgradeSpeed_Rewarded.gameObject.SetActive(false);
            UpgradeSpeed_Max.gameObject.SetActive(true);
        }

        if (Capacity_Upgrades_Count < 5)
        {
            if (GameManager.Instance.Cash_Player > CapacityPrice[Capacity_Upgrades_Count] && GameManager.Instance.GetWorkersCount() > 0)
            {
                UpgradeCapacity.interactable = true;

            }
            else
                UpgradeCapacity.interactable = false;

        }
        else
        {
            UpgradeCapacity.gameObject.SetActive(false);
            UpgradeCapacity_Rewraded.gameObject.SetActive(false);
            UpgradeCapacity_Max.gameObject.SetActive(true);
        }




        ProgressBar();

    }
    void ProgressBar()
    {
        for (int i = 0; i < Speed_Upgrades_Count; i++)
        {
            Upgrade_Speed_Progress.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }

        for (int i = 0; i < Capacity_Upgrades_Count; i++)
        {
            Upgrade_Capacity_Progress.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }

        for (int i = 0; i < Workers_count; i++)
        {
            Upgrade_Workers_Progress.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    bool IsRewarded;
    public void ShowRewarded(int x)
    {
        //ads commented by hussain
        //IsRewarded = true;
        ////blow code block commented by hussain
        //if (x == 1)
        //    MyAdsManager.instance.ShowRewardedVideo(AddWorker);
        //else
        //if (x == 2)
        //    MyAdsManager.instance.ShowRewardedVideo(UpgradeSpeedOfWorkers);
        //else
        //if (x == 3)
        //    MyAdsManager.instance.ShowRewardedVideo(UpgradeCapacityOfWorkers);



    }
    public void AddWorker()
    {
        if (IsRewarded)
        {
            StartCoroutine(SpawnDelay());
           
        }
        else
        if (GameManager.Instance.Cash_Player > Worker_Price[Workers_count])
        {
            StartCoroutine(SpawnDelay());
           
        }
    }
    IEnumerator SpawnDelay()
    {
        GameManager.Instance.player_Cam_Mover.ShowWorker();
        Debug.Log("spawnign worker now");
        yield return new WaitForSeconds(1f);
        GameManager.Instance.AddWorker(Worker_SpawnPoint,Worker_Price[Workers_count],IsRewarded);
        IsRewarded = false;
        Debug.Log("spawnign worker now 2222");
        Workers_count++;
        Refresh_UI();
    }
    public void UpgradeSpeedOfWorkers()
    {
        if (IsRewarded)
        {
            GameManager.Instance.UpgradeWorkersSpeed(SpeedPrice[Speed_Upgrades_Count],IsRewarded);
            Speed_Upgrades_Count++;
            Refresh_UI();
            IsRewarded = false;
        }
        else
        if (GameManager.Instance.Cash_Player> SpeedPrice[Speed_Upgrades_Count])
        {
            GameManager.Instance.UpgradeWorkersSpeed(SpeedPrice[Speed_Upgrades_Count],IsRewarded);
            Speed_Upgrades_Count++;
            Refresh_UI();
        }
    }
    public void UpgradeCapacityOfWorkers()
    {
        if (IsRewarded)
        {
            GameManager.Instance.UpgradeWorkersCapacity(CapacityPrice[Capacity_Upgrades_Count], IsRewarded);
            Capacity_Upgrades_Count++;
            Refresh_UI();
            IsRewarded = false;
        }
        else
       if (GameManager.Instance.Cash_Player > CapacityPrice[Capacity_Upgrades_Count])
        {
            GameManager.Instance.UpgradeWorkersCapacity(CapacityPrice[Capacity_Upgrades_Count], IsRewarded);
            Capacity_Upgrades_Count++;
            Refresh_UI();
        }
     }
    public void Toggle_HR_Panel(bool toggle)
    {
        HR_UI.SetActive(toggle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Toggle_HR_Panel(true);
            Refresh_UI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Toggle_HR_Panel(false);
            Refresh_UI();
        }
    }
}
