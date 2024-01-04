using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField] private bool _isLogEnabled;

    [ExecuteAlways]
    private void OnValidate()
    {
        Debug.unityLogger.logEnabled = _isLogEnabled;
    }
}
