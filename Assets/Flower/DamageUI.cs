using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : UI
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
        ShowDebugInfo(damage);
    }

    public override void ShowDebugInfo(object[] info)
    {
        //Debug.Log($"Cached damage = {_cachedDamage}.");
    }
}
