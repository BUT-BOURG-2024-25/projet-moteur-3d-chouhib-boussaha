using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    private GameObject player;
    private Rigidbody rb;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (player == null || rb == null) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);


        Vector3 lookDirection = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookDirection);

    }
}
