using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThreeManager : MonoBehaviour
{
    [HideInInspector]
    public float _skillThreeDuration;

    float _timer;
    void Start()
    {
        Invoke("DestroyTimer", _skillThreeDuration);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_timer + 1 < Time.time)
            {
                _timer = Time.time;

                Debug.Log("‰ñ•œ");
                collision.GetComponent<PlayerController>().HitHeal(100);
            }
        }
    }

    void DestroyTimer()
    {
        Destroy(this.gameObject);
    }
}
