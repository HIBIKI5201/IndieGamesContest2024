using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("“G‚ÌÅ‘å‘Ì—Í")]
    float _maxHealth;
    [ReadOnly, Tooltip("Œ»İ‚Ì‘Ì—Í")]
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
