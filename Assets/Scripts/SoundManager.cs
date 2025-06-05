using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource Sfx_Source;
    public AudioClip clip_pick_drop_Items;
    public AudioClip NewArea;
    public AudioClip BillingMachine;
    public AudioClip PickMoneySfx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Item_Picked_Droped()
    {
        Sfx_Source.PlayOneShot(clip_pick_drop_Items);
    }
    public void NewAreaUnlocked()
    {
        Sfx_Source.PlayOneShot(NewArea);
    }

    public void CustomerBillingMachine()
    {
        Sfx_Source.PlayOneShot(BillingMachine);
    }
    public void PickMoney()
    {
        Sfx_Source.PlayOneShot(PickMoneySfx);
    }
}
