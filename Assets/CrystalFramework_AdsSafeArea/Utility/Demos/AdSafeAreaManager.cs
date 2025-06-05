using UnityEngine;
using System;

namespace Crystal
{
    public class AdSafeAreaManager : MonoBehaviour
    {
        public GameObject adSpaceUI;
        public static AdSafeAreaManager Instance;
        //[SerializeField] KeyCode KeySafeArea = KeyCode.A;
        AdSafeArea.SimDevice[] Sims;
        int SimIdx;

        void Awake ()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            Sims = (AdSafeArea.SimDevice[])Enum.GetValues (typeof (AdSafeArea.SimDevice));

            if (!(Application.internetReachability == NetworkReachability.NotReachable)) // Internet Available
            {
                SimIdx = 1;
                ToggleSafeArea();
                if (adSpaceUI) adSpaceUI.SetActive(true);
            }
            else
            {
                SimIdx = 0;
                if (adSpaceUI) adSpaceUI.SetActive(false);
            }
        }

        //void Update ()
        //{
        //    if (Input.GetKeyDown (KeySafeArea))
        //        ToggleSafeArea ();
        //}

        void ToggleSafeArea ()
        {
            //SimIdx++;

            //if (SimIdx >= Sims.Length)
            //    SimIdx = 0;

            AdSafeArea.Sim = Sims[SimIdx];
            //Debug.LogFormat ("Switched to sim device {0} with debug key '{1}'", Sims[SimIdx], KeySafeArea);
        }

        //public void SetAdSpace()
        //{
        //    SimIdx++;
        //    if (SimIdx >= Sims.Length)
        //        SimIdx = 0;
        //    ToggleSafeArea();
        //}
    }
}
