using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThreeManager : MonoBehaviour
{
    [HideInInspector]
    public float _skillThreeDuration;
    void Start()
    {
        Invoke("DestroyTimer", _skillThreeDuration);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroyTimer()
    {
        Destroy(this.gameObject);
    }
}
