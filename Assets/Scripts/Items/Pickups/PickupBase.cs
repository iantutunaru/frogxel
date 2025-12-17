using Player;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private bool destroyOnCollect = true;

    // This is to prevent double-collect
    private bool _collected;

    public void Collect(PlayerController player)
    {
        if (_collected) return;
        _collected = true;

        OnCollected(player);

        if (destroyOnCollect)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    protected abstract void OnCollected(PlayerController player);
}
