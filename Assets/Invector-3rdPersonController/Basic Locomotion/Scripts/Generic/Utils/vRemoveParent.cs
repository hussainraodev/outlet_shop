using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vRemoveParent : MonoBehaviour
{

    public bool removeOnStart = true;

    private void Awake()
    {
        if (removeOnStart)
        {
           // RemoveParent();
            Invoke(nameof(RemoveParent), 5f);
        }
    }

    public void RemoveParentOfOtherTransform(Transform target)
    {
        target.SetParent(null);
    }
    public void RemoveParent()
    {
        transform.SetParent(null);
        transform.localScale = Vector3.one;
    }
}