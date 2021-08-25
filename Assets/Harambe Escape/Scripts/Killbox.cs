using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour
{
    //public bool enableKillbox = true;
    private void OnTriggerEnter(Collider other)
    {
        //if (enableKillbox)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerController>().GotHit();
            }
            else if (other.tag == "vehicle" || other.tag == "train")
            {
                Destroy(other);
            }
        }
    }
}
