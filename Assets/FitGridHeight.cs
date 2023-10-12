using Game.Craft;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FitGridHeight : MonoBehaviour
{
    public Scrollbar scrollbar;
    public void UpdateHeight()
    {
        int count = 0;
        ItemButton[] itemButtons = GetComponentsInChildren<ItemButton>();
        foreach (ItemButton button in itemButtons)
        {
            if (button.IsActivated)
            {
                count++;
            }
        }

        int rows = count / 5 + 1;
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, (rows * 221 + (rows - 1) * 10 + 30 + 30));
        scrollbar.value = 1;
    }
}
