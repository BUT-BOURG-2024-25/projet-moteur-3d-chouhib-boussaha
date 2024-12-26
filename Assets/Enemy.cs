using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;
    private float currentHealth = 0f;

    private SkinnedMeshRenderer renderer;



    public void Start()
    {
        Transform skinnedMeshChild = transform.Find("BountyHunterRIO2").transform.Find("BountyHunter_RIO_2");
        if (skinnedMeshChild != null)
        {
            renderer = skinnedMeshChild.GetComponent<SkinnedMeshRenderer>();
        }

        currentHealth = health;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        float healthPercentage = currentHealth / health;

        Color color = new Color(1 - healthPercentage, healthPercentage, 0, 0.75f); // RGBA color based on health

        renderer.material.color = color;

        if (currentHealth <= 0)
        {

            //Die();
            Destroy(gameObject);
            EnnemySpawner.Instance.downEnnemyCount();
        }
    }

    // j'ai chope ce code dans un video
    void Die(){
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            /* PlayerStats playerStats = player.GetComponent<PlayerStats>();
             if (playerStats != null)
             {
                 playerStats.AddXP(xpReward); 
             }
             else
             {
                 Debug.LogError("PlayerStats script is missing on the Player!");
             }*/
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }

        Debug.Log("Enemy died!");
        Destroy(gameObject);
        EnnemySpawner.Instance.downEnnemyCount();
    }

}
