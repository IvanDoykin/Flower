using Flower;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public event Action<object[]> HasShoot;

    protected override void InitialzeActions()
    {
        AddAction(ref HasShoot);
    }

    protected override void ResetActions()
    {
        DeleteAction(ref HasShoot);
    }

    public void Shoot(object[] damage)
    {
        Debug.Log($"Shoot with damage = {(int)damage[0]}");
        HasShoot?.Invoke(damage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shoot(new object[1] { 35 });
        }
    }
}
