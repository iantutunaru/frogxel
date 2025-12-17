using Player;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private PlayerController _player;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pickup = other.GetComponent<PickupBase>();
        if (pickup != null)
        {
            pickup.Collect(_player);
        }
    }
}
