using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneChanger;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    SceneKind _sceneKind;

    public enum SceneKind
    {
        Title,
        InGame
    }

    static readonly Dictionary<SceneKind, string> sceneName = new()
    {
        {SceneKind.Title, "Title"},
        {SceneKind.InGame, "HIBIKIScene"}
    };

    public void LoadSceneTitle()
    {
        SceneManager.LoadScene(sceneName[SceneKind.Title]);
    }

    public void LoadSceneInGame()
    {
        SceneManager.LoadScene(sceneName[SceneKind.InGame]);
    }


    public static void LoadScene(SceneKind sceneKind)
    {
        SceneManager.LoadScene(sceneName[sceneKind]);
    }
}
