using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float baseWeaponRateOfFire;
    private Transform weapons;
    private float rateOfFire;

    GameController gameController;

    public Transform BaseWeapon { get; private set; }
    public Transform SecondWeapon { get; private set; }
    public Transform ThirdWeapon { get; private set; }

    #region Singleton
    public static WeaponController instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Start()
    {
        rateOfFire = baseWeaponRateOfFire;
        gameController = GameController.instance; 
    }
    private void Update()
    {
        if(gameController.CurrentCarReference == null)
        {
            return;
        }
        weapons = gameController.CurrentCarReference.transform.GetChild(0);

        if (Input.GetButton("Fire1") && rateOfFire <= 0f)
        {
            BaseWeapon = weapons.GetChild(0);
            gameController.SpawnFromPool("BaseWeapon", BaseWeapon.position, BaseWeapon.rotation);
            rateOfFire = baseWeaponRateOfFire;

        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            SecondWeapon = weapons.GetChild(1);
            gameController.SpawnFromPool("SecondWeapon", SecondWeapon.position, SecondWeapon.rotation);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ThirdWeapon = weapons.GetChild(2);
            gameController.SpawnFromPool("ThirdWeapon", ThirdWeapon.position, ThirdWeapon.rotation);
        }

        if(rateOfFire > 0f)
        {
            rateOfFire -= Time.deltaTime;
        }
    }
}
