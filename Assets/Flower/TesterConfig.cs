using UnityEngine;

[CreateAssetMenu(fileName = "TesterConfig", menuName = "Test/TesterConfig", order = 1)]
public class TesterConfig : ScriptableObject
{
    [SerializeField] private string _codeCheckerExecutablePath; // Ex: C:\Users\Graham\Desktop\CodeChecker\CodeChecker.exe
    [SerializeField] private string _solutionFilePath; // Ex: C:\Users\Graham\Desktop\Fighting\Fighting.sln

    public string CodeCheckerExecutablePath => _codeCheckerExecutablePath;
    public string SolutionFilePath => _solutionFilePath;
}
