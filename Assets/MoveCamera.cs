using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCamera : MonoBehaviour
{


    public Transform DefaultPoint;
    public Transform ExitPoint,HRRoomPoint,WorkerPoint;
    // Start is called before the first frame update
    void Start()
    {
    
       // Invoke(nameof(ShowExit),5f);
    }

   public void ShowCustomer()
    {
      //  Start_Position = transform;

        transform.DOMove(ExitPoint.position, 1f);
        transform.DORotate(ExitPoint.eulerAngles, 1f);
        Invoke(nameof(Return_to_Default_Pos), 3f);

    }

    public void ShowHRRoom()
    {
        //  Start_Position = transform;

        transform.DOMove(HRRoomPoint.position, 1f);
        transform.DORotate(HRRoomPoint.eulerAngles, 1f);
        Invoke(nameof(Return_to_Default_Pos), 3f);

    }
    public void ShowWorker()
    {
        //  Start_Position = transform;

        transform.DOMove(WorkerPoint.position, 1f);
        transform.DORotate(WorkerPoint.eulerAngles, 1f);
        Invoke(nameof(Return_to_Default_Pos), 3f);

    }

    public void MoveToPos(GameObject pos)
    {
        //Vector3 tmp = pos.transform.position;
        //tmp.y = 10f;
        transform.DOMove(pos.transform.position,1f);
        //transform.DORotate(pos.transform.eulerAngles, 1f);
        Invoke(nameof(Return_to_Default_Pos), 3f);
    }

    public void MoveToPos_Rot(GameObject pos)
    {
        //Vector3 tmp = pos.transform.position;
        //tmp.y = 10f;
        transform.DOMove(pos.transform.position, 1f);
        transform.DORotate(pos.transform.eulerAngles, 1f);
        Invoke(nameof(Return_to_Default_Pos), 3f);
    }
    void Return_to_Default_Pos()
    {
        transform.DOMove(DefaultPoint.position, 1f);
        transform.DORotate(DefaultPoint.eulerAngles, 1f);
    }
}
