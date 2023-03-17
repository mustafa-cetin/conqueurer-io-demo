using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LordsAI : MonoBehaviour
{
    bool IsCastleOccuppied = false;
    bool SoldiersEneough = false;
    public float speed;

    public float enemyrange;
    public float villagerange;
    public float castlerange;

    private Lord lord;
    private Rigidbody2D rb;

    public Castle DeterminingClosestUnderAttackCastle()
    {
        foreach (Castle item in lord.castles)
        {
            if (item.underAttack == true)
            {
                return item;
            }
        }
        return null;
    }
    public List<Collider2D> CheckForTargetInLineOfSight()
    {
        List<Collider2D> Enemies = new List<Collider2D>();
        Collider2D[] ObjectsInRange = Physics2D.OverlapCircleAll(transform.position, enemyrange*lord.soldierCount/30);
        foreach (Collider2D item in ObjectsInRange)
        {
            if (item.gameObject.CompareTag("Lord") && item.gameObject.name != this.name)
            {
                Enemies.Add(item);
            }

        }
        return Enemies;
    }

    public Collider2D DeterminingClosestEnemy()
    {
        if (CheckForTargetInLineOfSight().Count>0)
        {
            Collider2D willReturn=willReturn=CheckForTargetInLineOfSight()[0];
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

    public List<Collider2D> CheckForVillageOfSight()
    {
        List<Collider2D> Villages = new List<Collider2D>();
        Collider2D[] ObjectsInRange = Physics2D.OverlapCircleAll(transform.position, villagerange*lord.soldierCount/30);
        foreach (Collider2D item in ObjectsInRange)
        {
            if (item.gameObject.CompareTag("Village"))
            {
                if (item.GetComponent<Village>().canTakeSoldier()) 
                {
                    Villages.Add(item);
                }
            }
        }
        return Villages;
    }

    public List<Collider2D> CheckForCastleOfSight()
    {
        List<Collider2D> Castles = new List<Collider2D>();
        Collider2D[] ObjectsInRange = Physics2D.OverlapCircleAll(transform.position, castlerange*lord.soldierCount/30);
        foreach (Collider2D item in ObjectsInRange)
        {
            if (item.gameObject.CompareTag("Castle"))
            {
                if (lord.canConquer(item.GetComponent<Castle>()))
                {
                Castles.Add(item);
                }
            }
        }
        return Castles;
    }

    public Collider2D DeterminingClosestCastle()
    {
        if (CheckForCastleOfSight().Count>0)
        {
            Collider2D willReturn=willReturn=CheckForCastleOfSight()[0];
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

    public Collider2D DeterminingClosestVillage()
    {
        if (CheckForVillageOfSight().Count>0)
        {
            Collider2D willReturn=willReturn=CheckForVillageOfSight()[0];
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

    void Start()
    {lord=GetComponent<Lord>();
    rb=GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        LordMovement();
    }
    private void LordMovement()
    {
        if (!lord.onConquest)
        {
          
            if (DeterminingClosestUnderAttackCastle() != null)
            {
            
                if (DeterminingClosestUnderAttackCastle().Conquerer != null)
                {
                    Vector3 difference = DeterminingClosestUnderAttackCastle().Conquerer.transform.position - this.transform.position;
                    difference = difference.normalized;
                    rb.velocity = new Vector2(difference.x, difference.y) * Time.fixedDeltaTime * lord.speed;
                    Debug.Log(lord.name+"kalesi saldırı altında koşuyor");
                }
            }    

            else if (DeterminingClosestEnemy() != null)
            {
                if (DeterminingClosestEnemy().GetComponent<Lord>().soldierCount < this.GetComponent<Lord>().soldierCount)
                {
                    Vector3 difference = DeterminingClosestEnemy().transform.position - this.transform.position;
                    difference=difference.normalized;
                   // this.transform.position += difference.normalized * Time.fixedDeltaTime * speed;
                   rb.velocity=new Vector2(difference.x,difference.y)*Time.fixedDeltaTime*lord.speed;
                   Debug.Log(lord.name+"asker sayısı dusuk en yakın dusmana kosuyor.");
                }
                else if (DeterminingClosestEnemy().GetComponent<Lord>().soldierCount >= this.GetComponent<Lord>().soldierCount)
                {
                    if (DeterminingClosestVillage() != null)
                    {
                        float differencebwvillage = Vector3.Distance(DeterminingClosestVillage().transform.position, this.transform.position);
                        float differencebwenemy = Vector3.Distance(DeterminingClosestEnemy().transform.position, this.transform.position);

                        if (differencebwenemy >= differencebwvillage)
                        {
                            Debug.Log(lord.name+"koyden asker toplamaya gıdıyor");
                            Vector3 difference = DeterminingClosestVillage().transform.position - this.transform.position;
                            difference = difference.normalized;
                            rb.velocity = new Vector2(difference.x, difference.y) * Time.fixedDeltaTime * lord.speed;
                        }
                        else
                        {
                            Vector3 difference = DeterminingClosestEnemy().transform.position - this.transform.position;
                            difference = difference.normalized;
                            rb.velocity = new Vector2(difference.x, difference.y) * Time.fixedDeltaTime * lord.speed * -1;
                            Debug.Log(lord.name+" adam daha yakında kaçıyor");
                        }
                    }
                    else
                    {
                        Vector3 difference = DeterminingClosestEnemy().transform.position - this.transform.position;
                        difference = difference.normalized;
                        rb.velocity = new Vector2(difference.x, difference.y) * Time.fixedDeltaTime * lord.speed * -1;
                        Debug.Log(lord.name+"köy yok admın canı daha fazla kaçıyor");
                    }
                
                }
            }
            else if (DeterminingClosestVillage() != null && DeterminingClosestCastle() != null)
        
            {
                Castle castle=DeterminingClosestCastle().GetComponent<Castle>();
                if (castle.garrison*1.5f<lord.soldierCount)
                {
                    Debug.Log(lord.name+"kale fethetmeye gidiyoruz");
                Vector3 difference = DeterminingClosestCastle().transform.position - this.transform.position;
                difference = difference.normalized;
                rb.velocity = new Vector2(difference.x, difference.y) * Time.fixedDeltaTime * lord.speed;
                }else
                {
                    Debug.Log(lord.name+"köyden asker toplamaya");
                Vector3 difference = DeterminingClosestVillage().transform.position - this.transform.position;
                difference=difference.normalized;
                rb.velocity=new Vector2(difference.x,difference.y)*Time.fixedDeltaTime*lord.speed;
                }
            }
            else if (DeterminingClosestVillage() != null)
            {
                Debug.Log(lord.name+"köyden asker toplamaya");
                Vector3 difference = DeterminingClosestVillage().transform.position - this.transform.position;
                difference=difference.normalized;
                rb.velocity=new Vector2(difference.x,difference.y)*Time.fixedDeltaTime*lord.speed;
            }

            else if (DeterminingClosestCastle() != null)
            {
                Debug.Log(lord.name+"kale fethetmeye gidiyoruz");
                Vector3 difference = DeterminingClosestCastle().transform.position - this.transform.position;
                difference = difference.normalized;
                rb.velocity = new Vector2(difference.x, difference.y) * Time.fixedDeltaTime * lord.speed;
            }
            else
            {rb.velocity=Vector2.zero;
            }
        }
        else if (DeterminingClosestCastle() != null)
        {
            Debug.Log(lord.name + "kale fethetmeye gidiyoruz");
            Vector3 difference = DeterminingClosestCastle().transform.position - this.transform.position;
            difference = difference.normalized;
            rb.velocity = new Vector2(difference.x, difference.y) * Time.fixedDeltaTime * lord.speed;
        }
    }
}