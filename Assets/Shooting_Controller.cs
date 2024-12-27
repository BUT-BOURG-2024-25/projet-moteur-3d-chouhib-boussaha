using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting_Controller : MonoBehaviour
{
    public Button ShootingButton;
    public GameObject player = null;

    private Shoot_Enemies shooting;

    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        if (player != null)
        {
            shooting = player.GetComponent<Shoot_Enemies>();
        }


        if (shooting != null)
        {
            ShootingButton.onClick.AddListener(() => shooting.Shoot(player));
        }
    }


    public void Update()
    {
        /*
        if (player != null)
        {
            // Check for input (e.g., mouse left button or a custom input for shooting)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Shoot_Enemies playerShoot = player.GetComponent<Shoot_Enemies>();
                if (playerShoot != null)
                {
                    playerShoot.Shoot();
                    Debug.Log("shooting");
                }
                else
                {
                    Debug.LogError("PlayerShoot script is missing on the player GameObject!");
                }
            }
        }
        */
    }
}
