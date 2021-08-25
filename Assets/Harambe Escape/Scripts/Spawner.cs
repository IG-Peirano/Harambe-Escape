using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform startPos;

    //spawn time based
    [SerializeField] float delayMin = 1.5f;
    [SerializeField] float delayMax = 5;
    [SerializeField] float speedMin = 1;
    [SerializeField] float speedMax = 4;

    //spawn upon start
    public bool useSpawnPlacement = false;
    public int spawnCountMin = 4;
    public int spawnCountMax = 20;

    private float lastTime = 0; // counting how often remakes and instanciate new obj
    private float delayTime = 0;
    private float speed = 0;

    [HideInInspector] public GameObject item;
    [HideInInspector] public bool goLeft = false;
    [HideInInspector] public float spawnLeftPos = 0; //position of the spawn obj in case it needs to be adjusted
    [HideInInspector] public float spawnRightPos = 0;

    private void Start()
    {   

        if (useSpawnPlacement)
        {
            int spawnCount = Random.Range(spawnCountMin, spawnCountMax);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnItem();
            }
        }
        else
        {
            speed = Random.Range(speedMin, speedMax);
        }
    }
    private void Update()
    {
        if (useSpawnPlacement) return;
        if(Time.time > lastTime + delayTime)
        {
            lastTime = Time.time;
            delayTime = Random.Range(delayMin, delayMax);

            SpawnItem();
        }

        /*if (player.transform.position.x - item.transform.position.x > 100)
        {
            Destroy(item);
        }*/
    }

    void SpawnItem()
    {
        Debug.Log("Spawn item"); 

        GameObject obj = Instantiate(item) as GameObject;
        obj.transform.position = GetSpawnPosition();

        float direction = 0;
        if (goLeft)
        {
            direction = 180;
        }

        if (!useSpawnPlacement)
        {
            obj.GetComponent<Mover>().speed = speed;
            obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(0, direction, 0);
        }
    }

    Vector3 GetSpawnPosition()
    {
        if (useSpawnPlacement)
        {
            int x = (int) Random.Range(spawnLeftPos, spawnRightPos);
            Vector3 pos = new Vector3(x, startPos.position.y, startPos.position.z);
            return pos;
        }
        else
        {
            return startPos.position;
        }
    }
}
