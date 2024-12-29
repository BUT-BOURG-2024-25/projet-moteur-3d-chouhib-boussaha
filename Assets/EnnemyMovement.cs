using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;
    private Enemy enemy;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = gameObject.GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (player == null || rb == null) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        rb.MovePosition(transform.position + direction * enemy.getMoveSpeed() * Time.fixedDeltaTime);

        Vector3 lookDirection = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookDirection);
        detectPlayer();
    }

    private void detectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, enemy.getRange());
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("PlayerCollider")) //;(
            {
                enemy.AttackPlayer();
                return;
            }
        }
    }
}
