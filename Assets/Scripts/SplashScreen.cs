using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour
{


    public GameObject AdsManager, Firebase;
    // Start is called before the first frame update
    void Start()
    {
		SetQualitySettingByDefault();
        StartCoroutine(delay());

	}

    // Update is called once per frame
   

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.5f);
        AdsManager.SetActive(true);
        yield return new WaitForSeconds(3f);
        Firebase.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
	private void SetQualitySettingByDefault()
	{
		if (SystemInfo.systemMemorySize < 4096)
		{
			// Low setting
			QualitySettings.SetQualityLevel(0);
		}
		else if (SystemInfo.systemMemorySize < 6144)
		{
			// Medium setting
			QualitySettings.antiAliasing = 2;
			QualitySettings.SetQualityLevel(1);
		}
		else
		{
			// High setting
			QualitySettings.antiAliasing = 2;
			QualitySettings.SetQualityLevel(2);
		}
	}

}
