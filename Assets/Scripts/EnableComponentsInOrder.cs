 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsInOrder : MonoBehaviour
{
    [SerializeField] private float delayValue = 0.02f;
    [SerializeField] private List<GameObject> componentsToEnableWithDelay;
    WaitForSecondsRealtime delay;
    // Start is called before the first frame update
    void Start()
    {
        delay = new WaitForSecondsRealtime(delayValue);
        StartCoroutine(EnableComponentsInOrderWithDelays());
    }
    IEnumerator EnableComponentsInOrderWithDelays()
    {
        yield return delay;
        foreach (GameObject v in componentsToEnableWithDelay)
        {
            if(v)
            v.SetActive(true);
            yield return delay;
        }
        yield return null;

       // Debug.Log("corot eneded boysssssssssssssss");
        //Destroy(gameObject);
    }
}
