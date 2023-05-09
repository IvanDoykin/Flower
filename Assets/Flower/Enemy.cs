using Flower;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public event Action<object[]> HasKicked;

    [SerializeField] private int _damage;
    private object[] _damageRaw;

    private void Start()
    {
        _damageRaw = new object[1] { _damage };
    }

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
        HasKicked?.Invoke(damage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Kick(_damageRaw);
        }
    }
}
