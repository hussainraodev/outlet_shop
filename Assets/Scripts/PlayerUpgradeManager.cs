using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUpgradeManager : MonoBehaviour
{


   
    public GameObject Upgrade_UI;
    public GameObject Upgrade_Speed_Progress;
    public GameObject Upgrade_Capacity_Progress;
    public Button UpgradeSpeed, UpgradeCapacity;
    public int[] SpeedPrice, CapacityPrice;
    public int Speed_Upgrades_Bought, Capacity_Upgrades_Bought;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Refresh_UI()
    {
      
        UpgradeCapacity.transform.GetComponentInChildren<Text>().text = CapacityPrice[Capacity_Upgrades_Bought].ToString();
        UpgradeSpeed.transform.GetComponentInChildren<Text>().text = SpeedPrice[Speed_Upgrades_Bought].ToString();
        //if (GameManager.Instance.Cash_Player > Worker_Price)
        //{
        //    HireWorker.interactable = true;
        //
        //}

        if (GameManager.Instance.Cash_Player > SpeedPrice[Speed_Upgrades_Bought])
        {
            UpgradeSpeed.interactable = true;

        }
        else
            UpgradeSpeed.interactable = false;



        if (GameManager.Instance.Cash_Player > CapacityPrice[Capacity_Upgrades_Bought] )
        {
            UpgradeCapacity.interactable = true;

        }
        else
            UpgradeCapacity.interactable = false;

        ProgressBar();

    }

    void ProgressBar()
    {
        for (int i = 0; i < Speed_Upgrades_Bought; i++)
        {
            Upgrade_Speed_Progress.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }

        for (int i = 0; i < Capacity_Upgrades_Bought; i++)
        {
            Upgrade_Capacity_Progress.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
    }
   
    public void UpgradeSpeedOfPlayer()
    {
        GameManager.Instance.Upgrade_Player_Speed(SpeedPrice[Speed_Upgrades_Bought]);
        Refresh_UI();
    }
    public void UpgradeCapacityPlayer()
    {
        GameManager.Instance.Upgrade_Player_Capacity(CapacityPrice[Capacity_Upgrades_Bought]);
        Refresh_UI();
    }
    public void Toggle_HR_Panel(bool toggle)
    {
        Upgrade_UI.SetActive(toggle);

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
