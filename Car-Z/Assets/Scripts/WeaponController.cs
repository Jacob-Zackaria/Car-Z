using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float bwRateOfFire;
    private float rateOfFire;

    GameController gameController;

    public Transform BaseWeapon { get; private set; }
    public Transform SecondWeapon { get; private set; }
    public Transform ThirdWeapon { get; private set; }

    private void Start()
    {
        rateOfFire = bwRateOfFire;
        gameController = GameController.instance;

        BaseWeapon = transform.Find("Weapons").GetChild(0);
        SecondWeapon = transform.Find("Weapons").GetChild(1);
        ThirdWeapon = transform.Find("Weapons").GetChild(2);
    }
    private void Update()
    {
        if(gameController.CurrentCarReference == null)
        {
            return;
        }

        if (Input.GetButton("Fire1") && rateOfFire <= 0f)
        {
            gameController.SpawnFromPool("BaseWeapon", BaseWeapon.position, BaseWeapon.rotation);
            rateOfFire = bwRateOfFire;

        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            gameController.SpawnFromPool("SecondWeapon", SecondWeapon.position, SecondWeapon.rotation);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            gameController.SpawnFromPool("ThirdWeapon", ThirdWeapon.position, ThirdWeapon.rotation);
        }

        if(rateOfFire > 0f)
        {
            rateOfFire -= Time.deltaTime;
        }
    }
}
