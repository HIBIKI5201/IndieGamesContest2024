using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;

    static AudioClip[] staticAudioClips;
    static AudioSource staticAudioSource;

    void Start()
    {
        staticAudioClips = new AudioClip[audioClips.Length];

        for (int i = 0; i < audioClips.Length; i++)
        {
            staticAudioClips[i] = audioClips[i];
        }

        audioSource = GetComponent<AudioSource>();
        staticAudioSource = audioSource;


    }

    // Update is called once per frame
    void Update()
    {

    }

    static public void Attack()
    {
        staticAudioSource.PlayOneShot(staticAudioClips[0]);
    }

    static public void Fire()
    {
        staticAudioSource.PlayOneShot(staticAudioClips[1]);
    }

    static public void ChangeSun()
    {
        staticAudioSource.PlayOneShot(staticAudioClips[2]);
    }

    static public void ChangeMoon()
    {
        staticAudioSource.PlayOneShot(staticAudioClips[3]);
    }

    static public void SkillOne()
    {
        staticAudioSource.PlayOneShot(staticAudioClips[4]);
    }

    static public void SkillTwo()
    {
        staticAudioSource.PlayOneShot(staticAudioClips[5]);
    }

    static public void SkillThree()
    {
        staticAudioSource.PlayOneShot(staticAudioClips[6]);
    }

    static public void SkillFour()
    {
        staticAudioSource.PlayOneShot(staticAudioClips[7]);
    }
}
