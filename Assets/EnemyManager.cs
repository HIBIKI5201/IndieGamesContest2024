using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("�G�̍ő�̗�")]
    float _maxHealth;
    [ReadOnly, Tooltip("���݂̗̑�")]
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
