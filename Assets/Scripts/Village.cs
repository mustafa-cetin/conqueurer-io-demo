using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Village : MonoBehaviour
{
    public int soldierCount=0;
    private float timer;
    public float delayCount=5f;
    
    public TextMeshProUGUI textComp;
    
    public GameObject plusSign;
    void Start()
    {
        UpdateSoldierCount(0);
        delayCount=Random.Range(.5f,2f);
    }

    // Update is called once per frame
    void Update()
    {
        TimerStuff();
        if (soldierCount>=10)
        {
            plusSign.SetActive(true);
        }else
        {
            plusSign.SetActive(false);
        }
    }
    public bool canTakeSoldier(){
        if (soldierCount>=10)
        {
            return true;
        }
        return false;
    }
    
    private void TimerStuff(){
        timer+=Time.deltaTime;
        if(timer>=delayCount){
            timer=0f;
            if (soldierCount <29){

            UpdateSoldierCount(soldierCount+1);
            }
        }
    }
    public void UpdateSoldierCount(int number){
        soldierCount=number;
        textComp.text=soldierCount.ToString();
    }
    
}
