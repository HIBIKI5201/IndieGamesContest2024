using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageAndHealUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject _damageText;

    Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateDamageText(Transform Pos, float damage, float axis)
    {
        StartCoroutine(StartInstantiateDamageText(Pos, damage, axis));
    }


    IEnumerator StartInstantiateDamageText(Transform Pos, float damage, float axis)
    {
        // ダメージテキストのインスタンスを生成
        GameObject damageText = Instantiate(_damageText, Pos.position, Quaternion.identity);
        damageText.transform.SetParent(transform);
        damageText.transform.localScale = Vector3.one;

        RectTransform rectTransform = damageText.GetComponent<RectTransform>();
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(Pos.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 anchoredPosition
        );

        rectTransform.anchoredPosition3D = (Vector3)anchoredPosition;

        damageText.GetComponent<TextMeshProUGUI>().text = damage.ToString("0");
        damageText.GetComponent<Rigidbody2D>().velocity = new Vector2(-axis * 2, 3);

        Debug.LogWarning("ダメージテキスト生成");

        yield return new WaitForSeconds(1);

        Debug.LogWarning("ダメージテキスト削除");


        Destroy(damageText);
    }
}
