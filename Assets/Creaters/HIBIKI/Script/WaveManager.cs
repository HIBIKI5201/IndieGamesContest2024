using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    GameObject _clearMessage;
    static GameObject _cm;

    static int _enemyCount;

    List<Transform> _enemyList;
    static int _waveCount;

    static Transform waveManagerTF;


    private void Start()
    {
        _enemyList = new();
        _cm = _clearMessage;

        for (int i = 0; i < transform.childCount; i++)
        {
            _enemyList.Add(transform.GetChild(i));
        }

        waveManagerTF = transform;
        NextWave(0);
    }

    static void NextWave(int number)
    {
        if (waveManagerTF.childCount > number)
        {
            waveManagerTF.GetChild(number).gameObject.SetActive(true);
            _enemyCount = waveManagerTF.GetChild(number).childCount;
            _waveCount++;
        }
        else
        {
            Debug.Log("テスト" + waveManagerTF.childCount + " and "+ number);
            _cm .SetActive(true);
        }
    }


    public static void KillEnemy()
    {
        _enemyCount--;
        Debug.LogWarning($"残りの敵は{_enemyCount}");
        
        if (_enemyCount <= 0)
        {
        NextWave(_waveCount);
        }
    }
}
