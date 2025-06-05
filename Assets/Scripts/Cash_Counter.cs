using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Cash_Counter : MonoBehaviour
{

    public GameObject cubePrefab;
     float spawnDelay = 0.1f;
  
     Vector3 startingPosition;
    public float spacing;
    public bool Award_Starting_money;
    
    public List<GameObject> Money_Spawned=new List<GameObject>();
    private void Start()
    {
        //  StartCoroutine(SpawnCubes());
        startingPosition = transform.localPosition;
        if(/*PlayerPrefs.GetInt("Starting_Money")==0*/ Award_Starting_money)
        Starting_Money();
    }
    Vector3 position;
   public void Starting_Money()
    {
        for (int i = 0; i < 50; i++)
        {
            AddObjects();
        }
        PlayerPrefs.SetInt("Starting_Money", 1);
    }
   

    Player_Controller player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Player_Controller>();
            Pick_Money();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }


    public void Pick_Money()
    {
        if(Money_Spawned.Count > 0)
        GameManager.Instance.PickMoney_Sfx();
        StartCoroutine(Add_Money_To_Player());
    }

    IEnumerator Add_Money_To_Player()
    {
        while (Money_Spawned.Count > 0)
        {
            if (Money_Spawned.Count < 11)
            {
                GameManager.Instance.AddCashToPlayer(Money_Spawned[0]);
                Money_Spawned.RemoveAt(0);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.AddCashToPlayer(Money_Spawned[0]);
                    Money_Spawned.RemoveAt(0);
                }
            }
            
           // Money_Spawned[0].transform.SetParent(player.Player_Object_Holder_Cash.transform);
            //Money_Spawned[0].transform.DOLocalJump(new Vector3(0, 0, 0), 3f, 1, 0.5f);
            //Money_Spawned[0].transform.DOScale(Vector3.zero, 2f);
          //  player.Player_Cash_Objects.Add(Money_Spawned[0].gameObject);
            
            
           
            yield return new WaitForSeconds(0f);
            
        }
        if (!GameManager.Instance.Stores[GameManager.Instance.Curren_Store-1].InitialCashTaken)
        {

            Debug.Log("HAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            GameManager.Instance.New_Area_Bought();
            GameManager.Instance.Stores[GameManager.Instance.Curren_Store-1].InitialCashTaken = true;
            gameObject.SetActive(false);

        }
        i = 0;
        j = 0;
        c = 0;
        yAxis = 0;
            



    }
    float yAxis;
    int i=0, j=0;

    int c=0;
    public void AddObjects()
    {
        for ( i=c; i < 5; i++)
        {
            for ( j=0; j < 5; j++)
            {
                if (Money_Spawned.Count == 75 && Award_Starting_money)
                    break;
                GameObject cube =GameManager.Instance.Stores[GameManager.Instance.Curren_Store-1].GetPooledCash();
                cube.transform.SetParent(this.transform);
                cube.SetActive(true);
                Vector3 position = startingPosition + new Vector3(j * 0.37f, yAxis, i * 0.21f);
                cube.transform.localPosition = position;
                cube.transform.eulerAngles = new Vector3(0, 90, 0);
                cube.transform.localScale= new Vector3(0.8f, 0.8f, 0.8f);
                Money_Spawned.Add(cube);
           
               
               
            }
            c ++ ;
            break;
        }

        if(c==5 )
        {
            yAxis += 0.1f;
           c = 0;
            j = 0;
        }
       
    }
}