using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private float Timer=-1;
    private void Start() {
        playerManager=GetComponent<PlayerManager>();
    }
    public void Conquer(Lord lord,Castle castle){
        if (Timer<=0)
        {
            Timer=1/((float)lord.soldierCount)*10;
            lord.SoldierCountUpdate(lord.soldierCount-2);
            castle.GarrisonUpdate(castle.garrison-1);
        }else
        {
            Timer-=Time.deltaTime;
        }
        if (castle.garrison<=0){
            Conquered(lord,castle);
        }
    }
    public void Conquered(Lord lord, Castle castle){
        if (lord.gameObject==playerManager.player.gameObject)
        {
            playerManager.Conquered();
        }
        int sameGarrison=lord.soldierCount/2;
        lord.SoldierCountUpdate(sameGarrison);
        castle.GarrisonUpdate(sameGarrison);

        castle.lord.castles.Remove(castle);
        lord.castles.Add(castle);
        castle.lord=lord;
    }


}
