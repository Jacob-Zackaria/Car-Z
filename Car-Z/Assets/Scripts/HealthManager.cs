using UnityEngine.UI;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float healthRegenerationRate;
    public float amountRegenerated;
    public Slider healthBar;

    private float regenerationRate;
    GameController gameController;

    private void Start()
    {
        gameController = GameController.instance;
    }

    public void IncreaseHealth(float healthValue)
    {
        healthBar.value += healthValue;
    }

    public void DecreaseHealth(float damageValue)
    {
        healthBar.value -= damageValue;
    }

    private void Update()
    {
        if(healthBar.value == 0)
        {
            gameController.PlayerDeath(this.gameObject, this.gameObject.name);
        }
        else if (healthBar.value != 1 && regenerationRate <= 0)
        {
            healthBar.value += amountRegenerated;
            regenerationRate = healthRegenerationRate;
        }

        if(regenerationRate > 0)
        {
            regenerationRate -= Time.deltaTime;
        }
    }
}
