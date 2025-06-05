using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMoneyReward : MonoBehaviour
{
    // Start is called before the first frame update
    public BoxCollider collider;
    public GameObject Money_Box,UI_Panel;
    public float time;
    private void Start()
    {
        Invoke(nameof(Activate_Reward_Box), time);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            UI_Panel.SetActive(true);
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UI_Panel.SetActive(false);
        }
    }

    public void ShowRewarded()
    {
        //MyAdsManager.instance.ShowRewardedVideo(GiveReward);//ads commented by hussain
    }
    public void HidePanel()
    {
        DeActivate_Reward_Box();
    }
    void GiveReward()
    {

      //  GameManager.Instance.Add_Cash_Reward((GameManager.Instance.Stores[GameManager.Instance.Curren_Store].Areas_Bought/2)*10);
        GameManager.Instance.Add_Cash_Reward(200);
        DeActivate_Reward_Box();
    }
    public void Activate_Reward_Box()
    {
        Money_Box.SetActive(true);
        collider.enabled = true;
    }

    public void DeActivate_Reward_Box()
    {
        Money_Box.SetActive(false);
        collider.enabled = false;
        UI_Panel.SetActive(false);
        Invoke(nameof(Activate_Reward_Box),time);
    }
}
