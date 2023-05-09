using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : Entity
{
    [SerializeField] private int _cachedDamage;
    protected override void InitialzeActions()
    {
    }

    protected override void ResetActions()
    {
    }

    public void ShowDamage(object[] damage)
    {
        _cachedDamage = (int)damage[0];
    }
}
