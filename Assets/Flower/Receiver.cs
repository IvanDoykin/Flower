using Flower;
using UnityEngine;

public class Receiver : Entity
{
    protected override void InitialzeActions()
    {
    }

    protected override void ResetActions()
    {
    }

    public void Receive(object[] messageData)
    {
        Debug.Log($"Received with id {(int)messageData[0]}");
    }
}
