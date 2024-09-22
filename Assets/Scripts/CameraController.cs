using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Only follow Room
    [SerializeField] private float speed;
    private float currentPosX;
    private float currentPosY;
    private Vector3 velocity = Vector3.zero;

    //Following the Player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
       //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.PosY, transform.position.z), ref velocity, speed * Time.deltaTime);

       //Following Player
       transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
       lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
         currentPosX = _newRoom.position.x;
         currentPosY = _newRoom.position.y + 0.53f;
    }
}
