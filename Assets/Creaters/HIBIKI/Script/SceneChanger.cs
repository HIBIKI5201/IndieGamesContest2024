using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    //R¸‰ï—p‚Ì‰¼İ’è
    static int _enemyCount;
    [SerializeField]
    int _enemyValue;
    [SerializeField]
    GameObject _clearMassege;

    // Start is called before the first frame update
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
            StartCoroutine(clear());
        }
    }

    IEnumerator clear()
    {
        _clearMassege .SetActive(true);
        yield return new WaitForSeconds(3);
        LoadHIBIKIScene();
    }

    public static void KillEnemey()
    {
        _enemyCount--;
    }

    public static void LoadHIBIKIScene()
    {
        SceneManager.LoadScene("HIBIKIScene");
    }
}
