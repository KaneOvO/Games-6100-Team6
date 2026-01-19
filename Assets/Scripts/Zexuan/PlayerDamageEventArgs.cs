using System;
using UnityEngine;

public class PlayerDamageEventArgs : EventArgs
{
    public int Damage { get; }
    
    public PlayerDamageEventArgs(int damage)
    {
        Damage = damage;
    }
}