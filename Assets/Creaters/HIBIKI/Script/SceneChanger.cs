using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    //êRç∏âÔópÇÃâºê›íË
    static int _enemyCount;
    [SerializeField]
    int _enemyValue;
    [SerializeField]
    GameObject _clearMassege;

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

        _clearMassege.SetActive(false);
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
        _clearMassege .SetActive(true);
        yield return new WaitForSeconds(3);
        LoadHIBIKIScene(SceneKind.InGame);
    }

    public static void KillEnemey()
    {
        _enemyCount--;
        Debug.LogWarning($"écÇËÇÃìGÇÕ{_enemyCount}");
    }

    public static void LoadHIBIKIScene(SceneKind sceneKind)
    {
        SceneManager.LoadScene(sceneName[sceneKind]);
    }
}
