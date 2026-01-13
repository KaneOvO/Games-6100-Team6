using System;
using UnityEngine;

// 玩家伤害事件参数
public class PlayerDamageEventArgs : EventArgs
{
    public int Damage { get; }
    
    public PlayerDamageEventArgs(int damage)
    {
        Damage = damage;
    }
}