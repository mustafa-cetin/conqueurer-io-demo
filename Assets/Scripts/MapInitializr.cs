using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInitializr : MonoBehaviour
{
    public GameObject couple;
    public GameObject village;
    public int lordNumber;
    public int villageCount;

    public LordManager lordManager;
    public Transform spawnPointForActors;
    public float intervalSpawn;
    
    public float intervalSpawnVillage;




    void Start()
    {
        for (int i = 0; i < lordNumber; i++)
        {
            Vector3 coordinate=new Vector3(spawnPointForActors.position.x+Random.Range(-intervalSpawn,intervalSpawn),spawnPointForActors.position.y+Random.Range(-intervalSpawn,intervalSpawn),0);

            GameObject coupleCloned=Instantiate(couple,coordinate,Quaternion.identity);
            Lord lord=coupleCloned.GetComponentsInChildren<Lord>()[0];
            lord.lordManager=lordManager;
        }
        
            VillageManager villageManager=lordManager.GetComponent<VillageManager>();
        for (int i = 0; i < villageCount; i++)
        {
            Vector3 selectedPos=villageManager.spawnerPoints[Random.Range(0,villageManager.spawnerPoints.Count)].position;
            Vector3 coordinate=new Vector3(selectedPos.x+Random.Range(-intervalSpawnVillage,intervalSpawnVillage),selectedPos.y+Random.Range(-intervalSpawnVillage,intervalSpawnVillage),0);

            GameObject spawnedVillage=Instantiate(village,coordinate,Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
