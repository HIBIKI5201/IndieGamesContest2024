using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //シーン切り替え用

public class Button_Game_Start : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(Pushed);
    }

    void Pushed()
    {
        SceneManager.LoadScene("HIBIKIScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
