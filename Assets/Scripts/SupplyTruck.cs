using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;
public class SupplyTruck : MonoBehaviour
{

    public GameObject Boy;
    public splineMove boySpline;
    public Animator SlidingDoor;

    public Items_Container_Shelf[] storage_Shelves;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DeliveryBoyArrived), 15f);
    }

   void DeliveryBoyArrived()
    {
        StartCoroutine(deliveryboySceneario());
    }

    IEnumerator deliveryboySceneario()
    {
        Boy.SetActive(true);
        SlidingDoor.Play("Open");
        yield return new WaitForSeconds(1f);
      
        boySpline.StartMove();
        yield return new WaitForSeconds(12f);
        ReachedEnd();
    }
    public void ReachedEnd()
    {
        Debug.Log("END");
        boySpline.Stop();
        SlidingDoor.Play("Close");
        Boy.SetActive(false);
        Invoke(nameof(DeliveryBoyArrived), 15f);
    }
    public void FillRacks()
    {
        for (int i = 0; i < storage_Shelves.Length; i++)
        {
            if (storage_Shelves[i].gameObject.activeInHierarchy)
            {
                storage_Shelves[i].Refill_Rack();
            }
        }
    }
}
