using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordsNewAI : MonoBehaviour
{
    private Lord lord;
    private Rigidbody2D rb;

    
    public float enemyrange;
    public float villagerange;
    public float castlerange;

    void Start()
    {
        lord=GetComponent<Lord>();
        rb=GetComponent<Rigidbody2D>();
    }

    public List<Lord> CheckForTargetInLineOfSight()
    {
        List<Lord> lords = new List<Lord>();
        Collider2D[] ObjectsInRange = Physics2D.OverlapCircleAll(transform.position, enemyrange);
        foreach (Collider2D item in ObjectsInRange)
        {
            if (item.gameObject.CompareTag("Lord") && item.gameObject.name != this.name)
            {
                lords.Add(item.GetComponent<Lord>());
            }

        }
        return lords;
    }
    public Lord DeterminingClosestLord()
    {
        if (CheckForTargetInLineOfSight().Count>0)
        {
            Lord willReturn=CheckForTargetInLineOfSight()[0];
        float tempdistance = 999999;
        for (int i = 0; i < CheckForTargetInLineOfSight().Count; i++)
        {

            float distance = Vector3.Distance(this.transform.position, CheckForTargetInLineOfSight()[i].transform.position);
            if (distance < tempdistance)
            {
                willReturn=CheckForTargetInLineOfSight()[i];
                tempdistance=distance;
            }
        }
        return willReturn;
        
        }
        else
        {
            return null;
        }
    }

    
    public List<Village> CheckForVillageOfSight()
    {
        List<Village> Villages = new List<Village>();
        Collider2D[] ObjectsInRange = Physics2D.OverlapCircleAll(transform.position, villagerange);
        foreach (Collider2D item in ObjectsInRange)
        {
            if (item.gameObject.CompareTag("Village"))
            {
                if (item.GetComponent<Village>().canTakeSoldier()) 
                {
                    Villages.Add(item.GetComponent<Village>());
                }
            }
        }
        return Villages;
    }
    public Village DeterminingClosestVillage()
    {
        if (CheckForVillageOfSight().Count>0)
        {
            Village willReturn=CheckForVillageOfSight()[0];
        float tempdistance = 999999;
        for (int i = 0; i < CheckForVillageOfSight().Count; i++)
        {

            float distance = Vector3.Distance(this.transform.position, CheckForVillageOfSight()[i].transform.position);
            if (distance < tempdistance)
            {
                willReturn=CheckForVillageOfSight()[i];
                tempdistance=distance;
            }
        }
        return willReturn;
        
        }
        else
        {
            return null;
        }
    }
    
    public List<Castle> CheckForCastleOfSight()
    {
        List<Castle> Castles = new List<Castle>();
        Collider2D[] ObjectsInRange = Physics2D.OverlapCircleAll(transform.position, castlerange);
        foreach (Collider2D item in ObjectsInRange)
        {
            if (item.gameObject.CompareTag("Castle"))
            {
                Castles.Add(item.GetComponent<Castle>());
            }
        }
        return Castles;
    }
    public Castle DeterminingClosestCastle()
    {
        if (CheckForCastleOfSight().Count>0)
        {
            Castle willReturn=CheckForCastleOfSight()[0];
        float tempdistance = 999999;
        for (int i = 0; i < CheckForCastleOfSight().Count; i++)
        {

            float distance = Vector3.Distance(this.transform.position, CheckForCastleOfSight()[i].transform.position);
            if (distance < tempdistance)
            {
                willReturn=CheckForCastleOfSight()[i];
                tempdistance=distance;
            }
        }
        return willReturn;
        
        }
        else
        {
            return null;
        }
    }
    public Castle DeterminingClosestConqurableCastle()
    {
        if (CheckForCastleOfSight().Count>0)
        {
            Castle willReturn=CheckForCastleOfSight()[0];
        float tempdistance = 999999;
        for (int i = 0; i < CheckForCastleOfSight().Count; i++)
        {
            if (lord.canConquer(CheckForCastleOfSight()[i]))
            {
                
            float distance = Vector3.Distance(this.transform.position, CheckForCastleOfSight()[i].transform.position);
            if (distance < tempdistance)
            {
                willReturn=CheckForCastleOfSight()[i];
                tempdistance=distance;
            }
            
            }
        }
        return willReturn;
        
        }
        else
        {
            return null;
        }
    }
    private void MovementToTarget(Vector3 target,int reverse){
        Vector3 difference = target - this.transform.position;
                    difference = difference.normalized;
                    rb.velocity = new Vector2(difference.x, difference.y) * Time.deltaTime * lord.speed * 100*reverse;
    }

    private Castle CheckUnderAttack(){
        foreach (Castle castle in lord.castles)
        {
            if (castle.underAttack)
            {
                return castle;
            }
        }
        return null;
    }
    private void AI(){
        if (CheckUnderAttack()!=null) // SALDIRIDA MIYIZ
        {
            if (!CheckForCastleOfSight().Contains(CheckUnderAttack())) // görüşünde kale yoksa kaleyi görene kadar ilerle
            {
            MovementToTarget(CheckUnderAttack().transform.position,1); // kaleye dogru hareket
            }
            else // kaleyi gördün
            {
                if (CheckUnderAttack().Conquerer!=null)
                {
            Lord invader=CheckUnderAttack().Conquerer;
             if (invader.soldierCount>lord.soldierCount)
             {
                if (DeterminingClosestVillage()!=null)
                {
                    MovementToTarget(DeterminingClosestVillage().transform.position,1);
                }
             }else
             {
                MovementToTarget(invader.transform.position,1);
             }   
                    
                }
            }
        }else if (DeterminingClosestLord()!=null)
        {
            if (DeterminingClosestLord().soldierCount>lord.soldierCount)
            {
                    // can change
            MovementToTarget(DeterminingClosestVillage().transform.position,-1);

            }else
            {
                MovementToTarget(DeterminingClosestLord().transform.position,1);
            }
        }else if (DeterminingClosestConqurableCastle()!=null)
        {
            
            MovementToTarget(DeterminingClosestConqurableCastle().transform.position,1);

        }else if(DeterminingClosestVillage()!=null){
        MovementToTarget(DeterminingClosestVillage().transform.position,1);

        }else
        {
            rb.velocity=Vector2.zero;
        }
    }
    void Update()
    {
        if (!lord.onConquest)
        {
        AI();
        }
    }



    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyrange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, villagerange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, castlerange);
    }
}
