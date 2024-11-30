using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtNearestElement : MonoBehaviour
{
    [SerializeField]
    float nearRange = -1f;

    GameObject nearest = null;
    float minDistance = -1f;

    private GameObject player = null;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, nearRange);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (Vector3.Distance(player.transform.position, hitColliders[i].transform.position)< minDistance)
            {
                minDistance = Vector3.Distance(player.transform.position, hitColliders[i].transform.position);
                nearest = hitColliders[i].gameObject;
            }
        }
        player.transform.LookAt(new Vector3(nearest.transform.position.x, player.transform.position.y, nearest.transform.position.z));
    }
}
