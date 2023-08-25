using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

[TestFixture]
public class CodeChecker
{
    private static string _rootPath;
    private static string _solutionPath;
    private static string _codeCheckerPath;
    private static string _rulesetPath;

    public static void UpdateRuleset()
    {
        _rootPath = Application.dataPath.Replace(@"/Assets", string.Empty);

        _rulesetPath = _rootPath + @"/Assets/CodeChecker/Ruleset.txt";

        string[] namesInPath = _rootPath.Split(new char[1] { '/' });
        _solutionPath = _rootPath + "/" + namesInPath[namesInPath.Length - 1] + ".sln";

        _codeCheckerPath = _rootPath + @"/Assets/CodeChecker/ConventionCodeChecker.exe";

        Process codeChecker = new Process();
        codeChecker.StartInfo.FileName = _codeCheckerPath;
        codeChecker.StartInfo.Arguments = $"{_solutionPath} {_rulesetPath}";
        codeChecker.Start();
        codeChecker.WaitForExit();

        if (codeChecker.ExitCode == 2)
        {
            UnityEngine.Debug.Log("Ruleset was updated.");
        }
        else
        {
            throw new System.Exception("Ruleset wasn't updated. " + codeChecker.ExitCode);
        }
    }

    [TestCaseSource(nameof(GetAllRules))]
    public void TestCodeWithRule(string rule)
    {
        Process codeChecker = new Process();
        codeChecker.StartInfo.FileName = _codeCheckerPath;
        codeChecker.StartInfo.Arguments = $"{_solutionPath} {_rulesetPath} {rule}";
        codeChecker.Start();
        codeChecker.WaitForExit();
        Assert.AreEqual(1, codeChecker.ExitCode);
    }

    public static IEnumerable<string> GetAllRules()
    {
        UpdateRuleset();
        return File.ReadAllLines(_rulesetPath);
    }
}
