using System;

public static class PlayerDamageEvent
{
    public static event EventHandler<PlayerDamageEventArgs> OnPlayerDamaged;

    public static void TriggerPlayerDamage(int damage)
    {
        OnPlayerDamaged?.Invoke(null, new PlayerDamageEventArgs(damage));
    }
}