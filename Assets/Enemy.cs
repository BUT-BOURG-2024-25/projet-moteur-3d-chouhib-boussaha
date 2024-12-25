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
            Destroy(gameObject);
            EnnemySpawner.Instance.downEnnemyCount();
        }
    }

}
