using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lord : MonoBehaviour
{
    public int startMinSoldierCount, startMaxSoldierCount;
    public List<Castle> castles;
    public int soldierCount;
    public Color lordsColor;
    private SpriteRenderer spriteRenderer;

    public TextMeshProUGUI textComp;
    public bool onFight=false;
    public float fightCooldown=.5f;
    public LordManager lordManager;
    public float speed;
    public float conquestCooldownNumber;
    public float conquestCooldown;
    public bool onConquest;
    public Castle whatIConquering;


    void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        lordsColor=Random.ColorHSV(1,0,0,1,1,.5f);
        spriteRenderer.color=lordsColor;
        SoldierCountUpdate(Random.Range(startMinSoldierCount,startMaxSoldierCount));
    }

    private void Update() {
        FightCooldownSetting();
        SpeedoSystem();
        ConquestCooldownSetting();
    }

    private void FightCooldownSetting(){
        
        if (fightCooldown<=0)
        {
            onFight=false;
            
        }else
        {
            onFight=true;
            fightCooldown-=Time.deltaTime;
            
        }
    }
    private void ConquestCooldownSetting()
    {
        if (conquestCooldown <= 0)
        {
            onConquest = false;
        }
        else
        {
            onConquest = true;
            conquestCooldown -= Time.deltaTime;
        }
    }
    public void SoldierCountUpdate(int number){
        soldierCount=number;
        textComp.text=soldierCount.ToString();
    }

    private void OnCollisionStay2D(Collision2D other) {

        if (other.gameObject.CompareTag("Lord") )
        {   
            lordManager.OnFight(other.gameObject.GetComponent<Lord>(),this);

        }
        
        if (other.gameObject.CompareTag("Village") )
        {   
            lordManager.GetSoldierFromVillage(this,other.gameObject.GetComponent<Village>());
        }
        
        if (other.gameObject.CompareTag("Castle") )
        {   
            
            lordManager.ConqueuerTheCastle(this,other.gameObject.GetComponent<Castle>());
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
         if (other.gameObject.CompareTag("Castle") )
        {   
            Castle castle=other.gameObject.GetComponent<Castle>();
            
            if (!castles.Contains(castle)&&!castle.underAttack)
            {
            whatIConquering=castle;
            castle.Conquerer=this;
            castle.underAttack=true;
            }
        
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        
        if (other.gameObject.CompareTag("Castle") )
        {   
            Castle castle=other.gameObject.GetComponent<Castle>();

        if (!castles.Contains(castle)&&castle.underAttack)
        {
            if (canConquer(castle))
            {
            conquestCooldown=conquestCooldownNumber;
            }
            whatIConquering=null;
            castle.Conquerer=null;
            castle.underAttack=false;
        }
        
        }
    }
    private void SpeedoSystem(){ // should update
        speed=50-(float)soldierCount/5;
        if (speed<=10)
        {
            speed=10;
        }
        speed=speed*5;
    }
    

    public bool canConquer(Castle castle){
        if (soldierCount>=10 && !castles.Contains(castle))
        {
            return true;
        }
        return false;
    }
}
