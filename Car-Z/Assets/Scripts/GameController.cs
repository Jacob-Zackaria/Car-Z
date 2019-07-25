using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public Button reset;
    public Transform resetPosition;
    public CinemachineVirtualCamera carCamera;

    
    private int i = 0;
    private GameObject currentCar;

    public GameObject[] cars;
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
        i = 0;
        if (cars != null)
        {
            currentCar = cars[i];

            CurrentCarReference = Instantiate(currentCar, resetPosition.position, Quaternion.identity);
            carCamera.Follow = CurrentCarReference.transform;
            carCamera.LookAt = CurrentCarReference.transform;
        }
    }
    public void OnPress()
    {
        Destroy(CurrentCarReference);

        CurrentCarReference = Instantiate(currentCar, resetPosition.position, Quaternion.identity);
        carCamera.Follow = CurrentCarReference.transform;
        carCamera.LookAt = CurrentCarReference.transform;
    }

    private void Update()
    {
        //Change cars.
        if(Input.GetKeyDown(KeyCode.K) && cars != null)
        {
            i += 1;
            currentCar = cars[i % cars.Length];

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
}
