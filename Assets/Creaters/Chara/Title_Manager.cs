using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 黒画面フェードインからフェードアウト、ボタンの挙動から効果音、音楽まで
/// タイトルのすべてを管理するスクリプト。
/// </summary>
public class Title_Manager : MonoBehaviour
{
    bool _enableControl = false;
    int _chooseButton;
    Transform _buttonX;
    //Transform _titleX;
    SpriteRenderer _blackfade;
    [SerializeField] AudioClip _startSe;
    AudioSource _titleBGM;
    AudioSource _audiosource;
    GameObject selectButton;

    // Start is called before the first frame update
    void Start()
    {
        GameObject BGM = GameObject.Find("Title BGM");
        GameObject blackFade = GameObject.Find("BlackFade");
        selectButton = GameObject.Find("StartButton");
        _titleBGM = BGM.GetComponent<AudioSource>();
        _audiosource = GetComponent<AudioSource>();
        _blackfade = blackFade.GetComponent<SpriteRenderer>();
        _buttonX = selectButton.GetComponent<Transform>();
        StartCoroutine("FadeIn");

    }


    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.1f);
        _blackfade.material.color = new Color32(0, 0, 0, 255);
        for (int i = 0; i < 51; i++)
        {
            _blackfade.material.color -= new Color32(0, 0, 0, 5);
            yield return new WaitForSeconds(0.01f);
        }
        //selectButton.transform.position = new Vector2(-1400, -150);
        _titleBGM.Play();
        //-1400 → -600
        for (int i = 0;i <= 6; i++)
        {
            selectButton.transform.Translate(new Vector2(0.5f, 0));
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0;i <= 6; i++)
        {
            selectButton.transform.Translate(new Vector2(0.25f,0));
            yield return new WaitForSeconds(0.02f);
        }
        for(int i = 0;i <= 4 ; i++)
        {
            selectButton.transform.Translate(new Vector2(0.125f,0));
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i <= 10; i++)
        {
            selectButton.transform.Translate(new Vector2(0.0625f, 0));
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1f);

        _enableControl = true;

    }
    

    // Update is called once per frame
    void Update()
    {
        if (_enableControl)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _audiosource.PlayOneShot(_startSe);
                StartCoroutine("FadeOut");
                _enableControl = false;
            }

        }
    }

    IEnumerator FadeOut()
    {
        for (int i = 0; i < 51; i++)
        {
            _blackfade.material.color += new Color32(0, 0, 0, 5);
            yield return new WaitForSeconds(0.05f);
        }
        for( int i = 0;i <= 10 ; i++)
        {
            _titleBGM.volume -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("HIBIKIScene");

    }
        //1346-1292 = 54

        /*

         0.5f = 54
        1f = 108

        800分減らさなきゃいけないから

         108*4=432

        800-432=368

        54*4=216

        368-216=152

        0.25f = 27

        27*3 = 81

        0.125f = 13.5

        13.5*6 = 81


         */
    }
