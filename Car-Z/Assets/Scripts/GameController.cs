using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public Button reset;
    public Transform[] resetPositions;
    public CinemachineVirtualCamera carCamera;
    public GameObject DeathScreen;


    private int carIndex = 0;
    private GameObject currentCar;

    public GameObject[] cars;
    public DestroyedCars[] destroyedCars;
    public GameObject carExplosion;
    public float forceRadius = 10f;
    public float explosionForce = 700f;
    public GameObject CurrentCarReference { get; private set; }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;



    #region Singleton
    public static GameController instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [System.Serializable]
    public struct DestroyedCars
    {
        public string carName;
        public GameObject destroyedCar;
    }

    private void Start()
    {
        //Create pool of objects.
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

        //Initial car spawn.
        carIndex = 0;
        if (cars != null)
        {
            currentCar = cars[carIndex];

            CreateCar();
        }
    }
    public void OnPress()
    {
        Destroy(CurrentCarReference);

        CreateCar();
    }

    private void Update()
    {
        //Change cars.
        if (Input.GetKeyDown(KeyCode.K) && cars != null)
        {
            carIndex += 1;
            currentCar = cars[carIndex % cars.Length];

            OnPress();
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);
        return (objectToSpawn);
    }

    private void CreateCar()
    {
        int j = Random.Range(0, resetPositions.Length);
        CurrentCarReference = Instantiate(currentCar,  resetPositions[j].position, Quaternion.identity);
        carCamera.Follow = CurrentCarReference.transform;
        carCamera.LookAt = CurrentCarReference.transform;
        Canvas canvas = CurrentCarReference.GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public void PlayerDeath(GameObject carToDestroy, string destroyedCarName)
    {
        //DeathScreen.SetActive(true);
        GameObject carDestroyed = null;

        Destroy(carToDestroy);
        Instantiate(carExplosion, carToDestroy.transform.position, Quaternion.identity);

        foreach (DestroyedCars car in destroyedCars)
        {
            if(car.carName == destroyedCarName)
            {
                carDestroyed = Instantiate(car.destroyedCar, carToDestroy.transform.position, carToDestroy.transform.rotation);
                break;
            }
        }

        Collider[] componentsToMove = Physics.OverlapSphere(carDestroyed.transform.position, forceRadius);
        foreach (Collider nearbyObject  in componentsToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            HealthManager hm = nearbyObject.GetComponent<HealthManager>();
            if (hm != null && rb != null)
            {
                hm.DecreaseHealth(0.4f);
                rb.AddForce(-transform.forward * 40f, ForceMode.VelocityChange);
            }
            else if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, carDestroyed.transform.position, forceRadius);

            }
        }

    }
}