using Flower;
using System;
using UnityEngine;
using System.Collections.Generic;

public class Interactor : Entity
{
    public Action<object[]> HasInteracted;

    protected override void InitialzeActions()
    {
        RegisterAction(ref HasInteracted);
    }

    public void Interact(object[] messageData)
    {
        int randomId = UnityEngine.Random.Range(0, 999999);
        Debug.Log($"Interact with id = {randomId}");
        HasInteracted?.Invoke(new object[1] { randomId });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Interact(new object[0]);
        }
    }
}
