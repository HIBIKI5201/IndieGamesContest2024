using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneChanger;

public class SceneChanger : MonoBehaviour
{

    //êRç∏âÔópÇÃâºê›íË
    static int _enemyCount;
    [SerializeField]
    int _enemyValue;
    [SerializeField]
    GameObject _clearMessage;

    [SerializeField]
    bool _active;

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


    void Start()
    {
        if (_active)
        {
            _enemyCount = _enemyValue;
            _clearMessage.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            if (_enemyCount <= 0)
            {
                StartCoroutine(Clear());
            }
        }
    }

    IEnumerator Clear()
    {
        _clearMessage.SetActive(true);
        yield return new WaitForSeconds(3);
        LoadScene(SceneKind.InGame);
    }

    public static void KillEnemy()
    {
        _enemyCount--;
        Debug.LogWarning($"écÇËÇÃìGÇÕ{_enemyCount}");
    }

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
