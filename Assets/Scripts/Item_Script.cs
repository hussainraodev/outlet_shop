using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Script : MonoBehaviour
{


    public enum Itemp_Type
    {
        Shoes,
        Shirt,
        Cap,
        Ball
    }

    public Itemp_Type TypeOfItem;
    // Start is called before the first frame update
    void Awake()
    {
        if (TypeOfItem ==  Itemp_Type.Shirt)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

  public void Custom_Event_Place_on_Rack()
    {
        if (TypeOfItem == Itemp_Type.Shirt)
        {
            Debug.Log("wtffffffffffffffffffffffffffffffffffffffffffff");
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Custom_Event_Pick_from_DisplayRack()
    {
        if (TypeOfItem == Itemp_Type.Shirt)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
