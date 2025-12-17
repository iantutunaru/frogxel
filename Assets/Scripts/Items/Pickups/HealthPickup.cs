using Player;
using UnityEngine;

public class HealthPickup : PickupBase
{

    public float amount = 10f;
    public float duration = 3f;

    protected override void OnCollected(PlayerController player)
    {
        var powerups = player.GetComponent<PlayerPowerupController>();
        if (powerups != null)
        {
            powerups.GrantHealth(amount, duration);

            Debug.Log("Player get healed : " + amount + "hp");
        }
    }
}
