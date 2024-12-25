using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;

    public void TakeDamage(float damage)
    {
        Debug.Log("HEALTH : " + health + " DAMAGE : " + damage);

        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            EnnemySpawner.Instance.downEnnemyCount();

        }
    }
}
