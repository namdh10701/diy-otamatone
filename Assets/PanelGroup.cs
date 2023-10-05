using UnityEngine;

public class PanelGroup : MonoBehaviour
{
    [SerializeField] private GameObject[] _panels;
    public int panelIndex;
    private void Awake()
    {
        ShowCurrentPanel();
    }

    public void ShowCurrentPanel()
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == panelIndex)
            {
                _panels[i].SetActive(true);
            }
            else
            {
                _panels[i].SetActive(false);
            }
        }
    }
    public void SetPageIndex(int index)
    {
        panelIndex = index;
        ShowCurrentPanel();
    }
}
