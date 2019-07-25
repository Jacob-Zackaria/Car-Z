using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public BulletCharacteristics bulletCharacteristics;
    GameController gameController;

    private Vector3 lookDirection;

    private void Start()
    {
        gameController = GameController.instance;
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.forward * -1 * bulletCharacteristics.bulletVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            return;
        }
        gameController.SpawnFromPool("BulletHit", transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }
}
