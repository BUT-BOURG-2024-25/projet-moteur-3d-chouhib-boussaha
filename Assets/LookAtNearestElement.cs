using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtNearestElement : MonoBehaviour
{

    public static LookAtNearestElement Instance { get; private set; }

    [SerializeField]
    float nearRange = -1f; //Player's range

    GameObject nearest = null; //Ennemy the player looks at & attacks

    float minDistance; //Used to get the closest ennemy to the player

    private GameObject player = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        minDistance = nearRange;
    }


    public void Update()
    {
        GameObject nearest = getNearestWithinRange(nearRange);
        if (nearest != null)
        {
            //Debug.Log(nearest.transform.position+" "+minDistance);
            player.transform.LookAt(new Vector3(nearest.transform.position.x, transform.position.y, nearest.transform.position.z));

            
            Player.Instance.DamageEnemy(nearest, WeaponType.Auto);
        }
    }

   

    public GameObject getNearestWithinRange(float range)
    {
        if (player == null)
            return null;

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, range);
        minDistance = range;
        nearest = null; // le player continue a regarder l'ennemi le + proche meme si il ne peut pas l'attaquer

        for (int i = 0; i < hitColliders.Length; i++)
        {
            //Debug.Log(hitColliders[i].tag);

            if (hitColliders[i].CompareTag("Ennemy") && Vector3.Distance(player.transform.position, hitColliders[i].transform.position) <= minDistance)
            {
                minDistance = Vector3.Distance(player.transform.position, hitColliders[i].transform.position);
                nearest = hitColliders[i].gameObject;
            }
        }
        return nearest;
    }

    public GameObject getAbsoluteNearest()
    {
        return getNearestWithinRange(int.MaxValue);
    }
}
