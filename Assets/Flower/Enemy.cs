using Flower;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Action<object[]> HasKicked;

    protected override void InitialzeActions()
    {
        AddAction(ref HasKicked);
    }

    protected override void ResetActions()
    {
        DeleteAction(ref HasKicked);
    }

    public void Kick(object[] damage)
    {
        Debug.Log($"Kick with damage = {(int)damage[0]}");
        HasKicked?.Invoke(damage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Kick(new object[1] { 50 });
        }
    }
}
