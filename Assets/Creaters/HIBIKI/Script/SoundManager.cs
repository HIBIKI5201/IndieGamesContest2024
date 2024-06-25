using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] audioClips;

    static AudioClip[] staticAudioClips;
    static Vector3 Pos;

    void Start()
    {
        staticAudioClips = new AudioClip[audioClips.Length];
        /*
        for (int i = 0; i < audioClips.Length; i++)
        {
            staticAudioClips[i] = audioClips[i];
        }

        Pos = transform.position;

        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void Attack()
    {
        AudioSource.PlayClipAtPoint(staticAudioClips[0], Pos);
    }
}
