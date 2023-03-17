using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordManager : MonoBehaviour
{
    
    private Castle teleportedCastle;
    public VillageManager villageManager;
    public CastleManager castleManager;
    public PlayerManager playerManager;
    public void OnFight(Lord lord1,Lord lord2){

        if (!lord1.onFight && !lord2.onFight)
        {
            lord1.onFight=true;
            lord2.onFight=true;
            if (lord1.soldierCount>lord2.soldierCount)
            {
                Debug.Log(lord1.name+" wins");
                LordWinner(lord1);
                LordLosing(lord2);

            }else if (lord1.soldierCount<lord2.soldierCount)
            {
                Debug.Log(lord2.name+" wins");
                LordWinner(lord2);
                LordLosing(lord1);
            }
        }
    }

    private void LordWinner(Lord lord){

        if (lord.gameObject==playerManager.player.gameObject)
        {
            playerManager.killCountIncrease();
        }
    }


    private void LordLosing(Lord lord){
        if (lord.gameObject==playerManager.player.gameObject)
        {
            playerManager.onDie();
        }
        lord.fightCooldown=2f;
        if (lord.whatIConquering!=null)
        {
        lord.conquestCooldown=0;
        lord.onConquest=false;
        lord.whatIConquering.Conquerer=null;
        lord.whatIConquering=null;
        }
            float tempDistance=int.MaxValue;
            foreach (Castle castle in lord.castles)
            {
                float distance=Vector3.Distance(lord.transform.position,castle.transform.position);

                if (distance<tempDistance)
                {
                    tempDistance=distance;
                    teleportedCastle=castle;
                }
            }

            lord.transform.position=new Vector3(teleportedCastle.transform.position.x+1.5f,teleportedCastle.transform.position.y,lord.transform.position.z);

            lord.SoldierCountUpdate(teleportedCastle.garrison/2);
            teleportedCastle.GarrisonUpdate(teleportedCastle.garrison/2);

        
        if (lord.castles.Count==0 || lord.soldierCount<=0)
        {
            
            if (playerManager.player.gameObject==lord.gameObject)
            {
                playerManager.Death();   
            }else
            {
                
            Destroy(lord.gameObject);
            }

            
        }
    }
    public void GetSoldierFromVillage(Lord lord,Village village){
        if (village.canTakeSoldier())
        {
        if (playerManager.player.gameObject==lord.gameObject)
        {
            playerManager.AlertSoldierAdd(village.soldierCount);
        }
        lord.SoldierCountUpdate(lord.soldierCount+village.soldierCount);
        villageManager.DestroyVillage(village);
        }
    }

    public void ConqueuerTheCastle(Lord lord,Castle castle){
        if (lord.canConquer(castle))
        {
        
        castleManager.Conquer(lord,castle);
        }
        // lord health will decrease by timer
    }
}
