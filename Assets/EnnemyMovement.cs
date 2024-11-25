using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = -1f;
    
    private GameObject player = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);

        gameObject.transform.LookAt(playerPos);

        transform.position = Vector3.MoveTowards(transform.position,player.transform.position, moveSpeed*Time.deltaTime);
    }
}
