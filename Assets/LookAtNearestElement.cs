using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtNearestElement : MonoBehaviour
{
    [SerializeField]
    float nearRange = -1f;

    GameObject nearest = null;

    float minDistance;

    private GameObject player = null;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        minDistance = nearRange;
    }


    void Update()
    {
        if (player == null)
            return;
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, nearRange);
        minDistance = nearRange;
        nearest = null; //Commenter -> le player continue a regarder l'ennemi le + proche meme si il ne peut pas l'attaquer

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Debug.Log(hitColliders[i].tag);

            if (hitColliders[i].CompareTag("Ennemy") && Vector3.Distance(player.transform.position, hitColliders[i].transform.position)<= minDistance )
            {
                    minDistance = Vector3.Distance(player.transform.position, hitColliders[i].transform.position);
                    nearest = hitColliders[i].gameObject;
            }
        }
        if (nearest != null)
        {
            Debug.Log(nearest.transform.position+" "+minDistance);
            player.transform.LookAt(new Vector3(nearest.transform.position.x, transform.position.y, nearest.transform.position.z));
        }
    }
}
