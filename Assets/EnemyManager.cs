using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("敵の最大体力")]
    float _maxHealth;
    [ReadOnly, Tooltip("現在の体力")]
    public float _currentHealth;

    public bool _moveActive;


    void Start()
    {
        _currentHealth = _maxHealth;
        _moveActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
