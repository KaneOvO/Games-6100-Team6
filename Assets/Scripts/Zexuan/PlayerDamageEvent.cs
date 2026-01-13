using System;

// 玩家伤害事件管理器（观察者模式）
public static class PlayerDamageEvent
{
    // 玩家受到伤害的事件
    public static event EventHandler<PlayerDamageEventArgs> OnPlayerDamaged;

    // 触发玩家伤害事件
    public static void TriggerPlayerDamage(int damage)
    {
        OnPlayerDamaged?.Invoke(null, new PlayerDamageEventArgs(damage));
    }
}