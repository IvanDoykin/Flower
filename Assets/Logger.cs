using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField] private bool _isLogEnabled;

    private void Awake()
    {
        Debug.unityLogger.logEnabled = _isLogEnabled;
    }
}
