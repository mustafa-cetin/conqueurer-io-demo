using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{
    public List<Transform> spawnerPoints;
    public void DestroyVillage(Village village){
        Vector3 spawnerPoint=spawnerPoints[Random.Range(0,spawnerPoints.Count)].position;
        Vector3 coordinate=new Vector3(spawnerPoint.x+Random.Range(-3f,3f),spawnerPoint.y+Random.Range(-3f,3f),0);
        Collider2D villageCollider=village.GetComponent<Collider2D>();
        Collider2D[] hitColliders=Physics2D.OverlapCircleAll(coordinate,0);
        while (hitColliders.Length>0)
        {
            coordinate=new Vector3(spawnerPoint.x+Random.Range(-3f,3f),spawnerPoint.y+Random.Range(-3f,3f),0);
            hitColliders=Physics2D.OverlapCircleAll(coordinate,0);
        }
        village.transform.position=coordinate;
        village.UpdateSoldierCount(0);
    }
}
