using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 1;
    public float moveTime = 0.4f;
    public float colliderCheck = 1;
    public bool isIdle = true;
    public bool isDead = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public bool jumpStart = false;
    public bool enableAngle = true;
    public float angleCheck = 1; 
    public float angleCheckDistance = 0.5f;

    public ParticleSystem particle = null;
    public GameObject player = null;
    private Renderer renderer = null;
    private bool isVisible = false;

    //public AudioClip audioIdle1 = null;
    //public AudioClip audioIdle2 = null;
    public AudioClip audioHop = null;
    public AudioClip audioHit = null;
    public AudioClip audioSplash = null;
    public ParticleSystem splash = null;
    public bool parentedToObject = false;

    void Start()
    {
        renderer = player.GetComponent<Renderer>();
    }
    void Update()
    {

        if (!GameManager.instance.CanPlay()) return;

        if (isDead) return;
        CanIdle();
        CanMove();
        IsVisible();
    }

    void CanIdle()
    {
        if (isIdle)
        {

            if (Input.GetKey(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.W)))
            {
                CheckIfIdle(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKeyDown(KeyCode.S)))
            {
                CheckIfIdle(0, 180, 0);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A)))
            {
                CheckIfIdle(0, -90, 0);
            }

            if (Input.GetKey(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D)))
            {
                CheckIfIdle(0, 90, 0);
            }
        }
    }

    void CheckIfIdle(float x, float y, float z)
    {
        player.transform.rotation = Quaternion.Euler(x, y, z);

        if (enableAngle)
        {
            CheckIfCanMoveAngles();
        }
        else
        {
            CheckIfCanMoveSingleRay();
        }
        //PlayAudioClip(audioIdle1);
    }
    void CheckIfCanMoveAngles()
    {
        RaycastHit hit;
        RaycastHit hitLeft;
        RaycastHit hitRight;

        Physics.Raycast(this.transform.position, player.transform.forward, out hit, colliderCheck);
        Physics.Raycast(this.transform.position, player.transform.forward + new Vector3(angleCheck, 0, 0), out hitLeft, colliderCheck + angleCheckDistance);
        Physics.Raycast(this.transform.position, player.transform.forward + new Vector3(-angleCheck, 0, 0), out hitRight, colliderCheck + angleCheckDistance);

        Debug.DrawRay(this.transform.position, player.transform.forward * colliderCheck, Color.red, 2);
        Debug.DrawRay(this.transform.position, (player.transform.forward + new Vector3(angleCheck, 0, 0)) * (colliderCheck + angleCheckDistance), Color.green, 2);
        Debug.DrawRay(this.transform.position, (player.transform.forward + new Vector3(-angleCheck, 0, 0)) * (colliderCheck + angleCheckDistance), Color.blue, 2);

        if (hit.collider == null && hitLeft.collider == null && hitRight.collider == null)
        {
            SetMove();
        }
        else
        {
            if (hit.collider != null && hit.collider.tag == "collider")
            {
                Debug.Log("Hit something with collider tag");

                isIdle = true;
            }
            else if (hitLeft.collider != null && hitLeft.collider.tag == "collider")
            {
                Debug.Log("Hit left something with collider tag");

                isIdle = true;
            }
            else if (hitRight.collider != null && hitRight.collider.tag == "collider")
            {
                Debug.Log("Hit right something with collider tag");

                isIdle = true;
            }
            else
            {
                SetMove();
            }
        }

    }

    void CheckIfCanMoveSingleRay()
    {
        RaycastHit hit;

        Physics.Raycast(this.transform.position, player.transform.forward, out hit, colliderCheck);

        Debug.DrawRay(this.transform.position, player.transform.forward * colliderCheck, Color.red, 2);

        if (hit.collider == null)
        {
            SetMove();
        }

        else
        {
            if (hit.collider.tag == "collider")
            {
                Debug.Log("Hit something with tag collider");

                isIdle = true;

            }
            else
            {
                SetMove();
            }
        }
    }
    void SetMove()
    {
        Debug.Log("Nothing nearby. Moving...");
        isIdle = false;
        isMoving = true;
        jumpStart = true;
    }

    void CanMove()
    {
        if (isMoving)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow) || (Input.GetKeyUp(KeyCode.W)))
            {
                Moving(new Vector3(transform.position.x, transform.position.y, transform.position.z + moveDistance)); SetMoveForwardState();
            }

            else if (Input.GetKeyUp(KeyCode.DownArrow) || (Input.GetKeyUp(KeyCode.S)))
            {
                Moving(new Vector3(transform.position.x, transform.position.y, transform.position.z - moveDistance));
            }

            else if (Input.GetKeyUp(KeyCode.LeftArrow) || (Input.GetKeyUp(KeyCode.A)))
            {
                Moving(new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z));
            }

            else if (Input.GetKeyUp(KeyCode.RightArrow) || (Input.GetKeyUp(KeyCode.D)))
            {
                Moving(new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z));
            }
        }

    }

    void Moving(Vector3 pos)
    {
        isIdle = false;
        isMoving = false;
        isJumping = true;
        jumpStart = false;

        PlayAudioClip(audioHop);

        LeanTween.move(this.gameObject, pos, moveTime).setOnComplete(MoveComplete);

    }

    void MoveComplete()
    {
        isIdle = true;
        isJumping = false;

        //PlayAudioClip(audioIdle2);

    }

    void SetMoveForwardState()
    {
        GameManager.instance.UpdateDistanceCount();
    }

    void IsVisible()
    {
        if (renderer.isVisible)
        {
            isVisible = true;
        }

        if (!renderer.isVisible && isVisible)
        {
            Debug.Log("Player offscreen. Aplly GotHit()");
            GotHit();
        }
    }

    public void GotHit()
    {
        if (!isDead)
        {
            PlayAudioClip(audioHit);
        }

        isDead = true;
        ParticleSystem.EmissionModule em = particle.emission;
        em.enabled = true;


        GameManager.instance.GameOver();
    }

    public void Drown()
    {
        PlayAudioClip(audioSplash);
        isDead = true;
        ParticleSystem.EmissionModule em = splash.emission;
        em.enabled = true;
        player.SetActive(false);
        GameManager.instance.GameOver();
    }

    void PlayAudioClip(AudioClip clip)
    {
        this.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
