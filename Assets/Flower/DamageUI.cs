using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : Entity
{
    protected override void InitialzeActions()
    {
    }

    protected override void ResetActions()
    {
    }

    public void ShowDamage(object[] damage)
    {
        Debug.Log($"[Damage: {damage[0]}]");
    }
}
