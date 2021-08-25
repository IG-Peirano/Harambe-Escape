using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] bool autoMove = true;
    [SerializeField] float speed = 0.25f;
    public GameObject player = null;
    public Vector3 offset = new Vector3(3, 6, -3);
    Vector3 depth = Vector3.zero;
    Vector3 pos = Vector3.zero;

    private void Update()
    {
        if (!GameManager.instance.CanPlay()) return;

        if (autoMove)
        {
            depth = this.gameObject.transform.position += new Vector3(0, 0, speed * Time.deltaTime); // camera current position with 0.01 increment
            pos = Vector3.Lerp(gameObject.transform.position, player.transform.position + offset, Time.deltaTime); // capturing the transform between camera and the player
            gameObject.transform.position = new Vector3(pos.x, offset.y, depth.z); // actual camera position under x, offset is how high above
        }
        else
        {
            pos = Vector3.Lerp(gameObject.transform.position, player.transform.position + offset, Time.deltaTime);
            gameObject.transform.position = new Vector3(pos.x, offset.y, pos.z);
        }
    }
}
