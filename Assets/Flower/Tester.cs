using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Tester : MonoBehaviour
{
    [SerializeField] private TesterConfig _config;

    [ContextMenu("Test code")]
    private void Test()
    {
        Process P = Process.Start(_config.CodeCheckerExecutablePath, _config.SolutionFilePath);
        P.WaitForExit();
        int result = P.ExitCode;

        if (result == 0)
        {
            UnityEngine.Debug.Log("Sucessful test!");
        }
        else if (result == -1)
        {
            UnityEngine.Debug.Log("Failed test!");
        }
    }
}
