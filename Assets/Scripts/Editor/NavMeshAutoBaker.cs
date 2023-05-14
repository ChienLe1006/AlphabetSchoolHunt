using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoadAttribute]
public static class DefaultSceneLoader
{
    [MenuItem("NavMesh/BuildNavMeshForAllLevels")]
    public static void Build()
    {
        string prefix = "Assets/_Scenes/Level_";
        string suffix = ".unity";
        List<string> sceneNames = new List<string>();
        for (int i = 1; i < EditorSceneManager.sceneCountInBuildSettings; i++)
        {
            sceneNames.Add(prefix + i + suffix);
        }
    }
}
