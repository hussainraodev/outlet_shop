//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds.Ump;
//using GoogleMobileAds.Ump.Api;
//using UnityEngine.SceneManagement;

//public class GDPRConsent : MonoBehaviour
//{
//    public GameObject localConsent;
//    public string privacyUrl, termsAndConditionsUrl;
//    //public string SplashScreen = "SplashScreen";
//  //  public Scenes NextScene;
//    ConsentForm _consentForm;

//    public bool IsInternetAvailable()
//    {
//        return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork |
//            Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
//    }
//    // Start is called before the first frame update
//    void Start()
//    {
     
//        if (!IsInternetAvailable())
//        {
//            Invoke(nameof(ShowLocalConsent),0.1f);
//            return;
//        }


//        //DontDestroyOnLoad(this.gameObject);
//        //var debugSettings = new ConsentDebugSettings
//        //{
//        //    // Geography appears as in EEA for debug devices.
//        //    DebugGeography = DebugGeography.EEA,
//        //    TestDeviceHashedIds = new List<string>
//        //    {
//        //        "BDAC3ABA273B957575889F626918A0A8"
//        //    }
//        //};
//        ConsentRequestParameters request = new ConsentRequestParameters
//        {
//            TagForUnderAgeOfConsent = false,
//           // ConsentDebugSettings = debugSettings,
//        };
//        if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
//        {

//            SceneManager.LoadSceneAsync(1);
//            return;
//        }
//        if (ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
//        {

//            Invoke(nameof(ShowLocalConsent), 0.1f);
//            return;
//        }
//        ConsentInformation.Update(request, OnConsentInfoUpdated);
//    }

//    void OnConsentInfoUpdated(FormError error)
//    {

//        if (error != null)
//        {
//            //Debug.LogError(error);
//            SceneManager.LoadSceneAsync(1);
//            return;
//        }
//        if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
//        {

//            SceneManager.LoadSceneAsync(1);
//            return;
//        }
//        if (ConsentInformation.IsConsentFormAvailable())
//        {
//            LoadConsentForm();
//        }
//        else
//        {
//            print("1");
//            Invoke(nameof(ShowLocalConsent), 0.1f);
//        }
//    }

//    void LoadConsentForm()
//    {
//        ConsentForm.Load(OnLoadConsentForm);
//    }

//    void OnLoadConsentForm(ConsentForm consentForm, FormError error)
//    {
//        print("loaded");
//        if (error != null)
//        {
//            print("A1");
//            //Debug.LogError(error);
//            Invoke(nameof(ShowLocalConsent), 0.1f);
//            //SceneManager.LoadSceneAsync(SplashScreen);
//            return;
//        }

//        // The consent form was loaded.
//        // Save the consent form for future requests.
//        _consentForm = consentForm;
//        if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
//        {

//            SceneManager.LoadSceneAsync(1);
//            return;
//        }
//        // You are now ready to show the form.
//        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
//        {
//            _consentForm.Show(OnShowForm);
//        }
//        else
//        {
//            Invoke(nameof(ShowLocalConsent), 0.1f);
//        }
//    }


//    void OnShowForm(FormError error)
//    {
//        if (error != null)
//        {
//            //Debug.LogError(error);
//            SceneManager.LoadSceneAsync(1);
//            return;
//        }
//        else
//        {
//            print("3");
//            //localConsent.SetActive(true);
//        }
//        if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
//        {

//            SceneManager.LoadSceneAsync(1);
//            return;
//        }
//        else
//        {
//            Invoke(nameof(ShowLocalConsent), 0.1f);
//        }
//        LoadConsentForm();
//    }


//    public void ShowLocalConsent()
//    {
//        if (PlayerPrefs.GetInt("Consent", 0) == 0)
//        {
//            localConsent.SetActive(true); 
//        }
//        else
//            Accept();
//    }

//    public void Accept()
//    {
//        PlayerPrefs.SetInt("Consent", 1);
//        localConsent.SetActive(false);
//        SceneManager.LoadSceneAsync(1);
//    }


//    public void PrivacyPolicy()
//    {
//        Application.OpenURL(privacyUrl);
//    }

//    public void TermsAndConditions()
//    {
//        Application.OpenURL(termsAndConditionsUrl);
//    }
     
//}
