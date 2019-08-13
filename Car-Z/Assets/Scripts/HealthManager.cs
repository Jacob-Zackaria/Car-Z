using UnityEngine.UI;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public Slider healthBar;
    GameController gameController;

    private void Start()
    {
        gameController = GameController.instance;
    }

    public void IncreaseHealth(int healthValue)
    {
        healthBar.value += healthValue;
    }

    public void DecreaseHealth(int damageValue)
    {
        healthBar.value -= damageValue;
    }

    private void Update()
    {
        if(healthBar.value == 0)
        {
            gameController.PlayerDeath();
        }
    }
}
