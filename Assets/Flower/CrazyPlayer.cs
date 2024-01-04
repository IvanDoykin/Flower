using Flower;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyPlayer : Entity, IPlayer
{
    public event Action<object[]> HasShoot;

    [SerializeField] private int _damage;
    private object[] _damageRaw;

    private void Start()
    {
        _damageRaw = new object[1] { _damage };
    }

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
        HasShoot?.Invoke(damage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shoot(_damageRaw);
        }
    }

}
