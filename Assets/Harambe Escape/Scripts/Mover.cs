using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 1.0f;
    public float moveDirection = 0;
    public bool parentOnTrigger = true;
    public bool hitBoxOnTrigger = false;
    public GameObject moverObject = null;

    private Renderer renderer = null;
    private bool isVisible = false;

    private void Start()
    {
        renderer = moverObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        this.transform.Translate(speed * Time.deltaTime, 0, 0);

        IsVisible();
    }

    void IsVisible()
    {
        if (renderer.isVisible)
        {
            isVisible = true;
        }

        if (!renderer.isVisible && isVisible)
        {
            Debug.Log("Remove object no longer seen by camera");

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Enter");

            if (parentOnTrigger)
            {
                Debug.Log("Enter: Parent");
                other.transform.parent = this.transform;
                PlayerController pc = other.GetComponent<PlayerController>();
                pc.enableAngle = true;
                pc.parentedToObject = true;
            }

            if (hitBoxOnTrigger)
            {
                Debug.Log("Enter: Hitbox. Game over.");
                other.GetComponent<PlayerController>().GotHit();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (parentOnTrigger)
            {
                Debug.Log("Exit");
                other.transform.parent = null;
                PlayerController pc = other.GetComponent<PlayerController>();
                pc.enableAngle = false;
                pc.parentedToObject = false;
            }

        }
    }
}
