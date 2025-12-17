using Player;
using UnityEngine;

public class InvincibilityPickup : PickupBase
{
    [SerializeField] private float duration = 3f;

    protected override void OnCollected(PlayerController player)
    {
        var powerups = player.GetComponent<PlayerPowerupController>();
        if (powerups != null)
        {
            powerups.GrantInvincibility(duration);

            Debug.Log("Player get Invincibility for " + duration + "seconds");
        }
    }
}
