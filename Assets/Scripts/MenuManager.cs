using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public Text Cash;
    public GameObject Loading;
    // Start is called before the first frame update
    void Start()
    {
        //MyAdsManager.instance.ShowBanner();
    }

    IEnumerator delay()
    {
        Loading.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }
    public void LoadGamePlay()
    {
        StartCoroutine(delay());
    }

    public void MoreFunBtn()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Arkane+Games");
    }

    public void ShowRateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }
}
