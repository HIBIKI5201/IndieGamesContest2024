using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //�V�[���؂�ւ��p

public class Button_Game_Start : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(pushed);
    }

    void pushed()
    {
        SceneManager.LoadScene("HIBIKIScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
