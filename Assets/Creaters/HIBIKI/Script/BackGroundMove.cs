using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [SerializeField, ReadOnly, Tooltip("前フレームの位置")]
    Vector3 lastPlayerPos;

    [SerializeField, Tooltip("移動速度の倍率")]
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
