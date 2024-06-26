using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    //審査会用の仮設定
    static int _enemyCount;
    [SerializeField]
    int _enemyValue;
    [SerializeField]
    GameObject _clearMessage;

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
        _enemyCount = _enemyValue;

        _clearMessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyCount <= 0)
        {
            StartCoroutine(Clear());
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
        Debug.LogWarning($"残りの敵は{_enemyCount}");
    }

    public static void LoadScene(SceneKind sceneKind)
    {
        SceneManager.LoadScene(sceneName[sceneKind]);
    }
}
