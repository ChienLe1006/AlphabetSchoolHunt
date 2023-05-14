using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MyTools
{
    [MenuItem("Tools/Clear Data")]
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Open Scene/MasterLevel")]
    public static void OpenSceneMaster()
    {
        string scenePath = "Assets/_Scenes/MasterLevel.unity";
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            int dialogResult = EditorUtility.DisplayDialogComplex(
                "Scene has been modified",
                "Do you want to save the changes you made in the current scene?",
                "Save", "Don't Save", "Cancel");

            switch (dialogResult)
            {
                case 0: 
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(scenePath);
                    break;
                case 1: 
                    EditorSceneManager.OpenScene(scenePath);
                    break;
                case 2: 
                    break;
                default:
                    Debug.LogWarning("Something went wrong when switching scenes.");
                    break;
            }
        }
        else
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }

    [MenuItem("Tools/Open Scene/Loading")]
    public static void OpenSceneLoading()
    {
        string scenePath = "Assets/_Scenes/Loading.unity";
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            int dialogResult = EditorUtility.DisplayDialogComplex(
                "Scene has been modified",
                "Do you want to save the changes you made in the current scene?",
                "Save", "Don't Save", "Cancel");

            switch (dialogResult)
            {
                case 0: 
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(scenePath);
                    break;
                case 1: 
                    EditorSceneManager.OpenScene(scenePath);
                    break;
                case 2: 
                    break;
                default:
                    Debug.LogWarning("Something went wrong when switching scenes.");
                    break;
            }
        }
        else
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }

    [MenuItem("Tools/Hack Money")]
    public static void HackMoney()
    {
        PlayerPrefs.SetInt(Helper.GOLD, 10000);
    }
}
