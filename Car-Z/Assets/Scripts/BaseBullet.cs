using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float bulletVelocity;
    GameController gameController;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = GameController.instance;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.up * bulletVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            return;
        }
        gameController.SpawnFromPool("BulletHit", transform.position - transform.up, Quaternion.identity);
        this.gameObject.SetActive(false);
    }
}
