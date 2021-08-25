using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public int coinValue = 1;
    public GameObject banana = null;
    public AudioClip audioClip = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        Debug.Log("Picked up a banana");

        // manager -> update banana count
        GameManager.instance.UpdateBananaCount(coinValue);

        banana.SetActive(false);
        this.GetComponent<AudioSource>().PlayOneShot(audioClip);

        Destroy(this.gameObject, audioClip.length); // how long will it play before destroying the obj
    }
}
