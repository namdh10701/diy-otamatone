using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleProgressBar : MonoBehaviour
{
    [SerializeField] float CurrentProgess;
    [SerializeField] RectTransform icon;
    Material mat;
    float parentWidth;
    private void Awake()
    {
        parentWidth = Mathf.Abs(transform.GetComponent<RectTransform>().rect.x * 2);
        Debug.Log(parentWidth);
        mat = GetComponent<Image>().material;
    }
    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("_P1Progress", CurrentProgess);
        icon.anchoredPosition = new Vector2(parentWidth * CurrentProgess, 0);
    }
}
