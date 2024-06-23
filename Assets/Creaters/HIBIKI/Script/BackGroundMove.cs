using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [SerializeField, ReadOnly, Tooltip("�O�t���[���̈ʒu")]
    Vector3 lastPlayerPos;

    [SerializeField, Tooltip("�ړ����x�̔{��")]
    float _moveSpeed;
    void Start()
    {
        lastPlayerPos = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2((Player.transform.position.x - lastPlayerPos.x) * _moveSpeed, 0));

        lastPlayerPos = Player.transform.position;
    }
}
