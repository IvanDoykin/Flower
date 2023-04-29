using Flower;
using UnityEngine;

public class Interactor : Entity
{
    [ContextMenu("Interact")]
    public void Interact()
    {
        Debug.Log("Interact");
        InvokeMessages();
    }
}
