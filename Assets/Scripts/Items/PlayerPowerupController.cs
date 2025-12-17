using System.Collections;
using UnityEngine;

public class PlayerPowerupController : MonoBehaviour
{
    public bool IsInvincible { get; private set; }
    public bool IsHealing { get; private set; }

    private Coroutine _invincibleRoutine;
    private Coroutine _healthRoutine;


    public void GrantInvincibility(float duration)
    {
        if (_invincibleRoutine != null) StopCoroutine(_invincibleRoutine);
        _invincibleRoutine = StartCoroutine(InvincibleRoutine(duration));
    }

    private IEnumerator InvincibleRoutine(float duration)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(duration);
        IsInvincible = false;
        _invincibleRoutine = null;
    }

    public void GrantHealth(float amount, float duration)
    {
        if (_healthRoutine != null) StopCoroutine(_healthRoutine);
        // heal the player here, maybe get health stat from PlayerStats.
        _healthRoutine = StartCoroutine(HealthRoutine(duration));
    }

    private IEnumerator HealthRoutine( float duration)
    {
        IsHealing = true;
        yield return new WaitForSeconds(duration);
        IsHealing = false;
        _healthRoutine = null;

    }

    
}
