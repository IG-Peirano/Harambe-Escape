using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> platform = new List<GameObject>();
    public List<float> height = new List<float>();

    private int randomRange = 0;
    private float lastPosition = 0;
    private float lastScale = 0;

    public void RandomGenerator()
    {
        randomRange = Random.Range(0, platform.Count);
        for (int i = 0; i < platform.Count; i++)
        {
            CreateLevelObject(platform[i], height[i], i);
        }
    }

    public void CreateLevelObject (GameObject obj, float height, int value)
    {   

        if(randomRange == value)
        {
        GameObject go = Instantiate(obj) as GameObject;

            float offset = lastPosition + (lastScale * 0.5f);
            offset += (go.transform.localScale.z) * 0.5f;
            Vector3 pos = new Vector3(0, height, offset);

            go.transform.position = pos;

            lastPosition = go.transform.position.z;
            lastScale = go.transform.localScale.z;

            // recording the last instantiated object position

            go.transform.parent = this.transform;
        }
    }
}
