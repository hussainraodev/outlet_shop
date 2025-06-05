using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{


    public Animator[] Doors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenDoor()
    {
        for (int i = 0; i < Doors.Length; i++)
        {
         //   Debug.LogError("open");
            Doors[i].Play("DoorOpen");
        }
    }
    void CloseDoor()
    {
        for (int i = 0; i < Doors.Length; i++)
        {
            Doors[i].Play("DoorClose");
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("CompanionAI"))
            OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("CompanionAI"))
            CloseDoor();
    }
}
