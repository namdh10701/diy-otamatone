using System.Linq;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    private Tab[] _tabs;
    [SerializeField] private PanelGroup _panelGroup;
    private int _currentTabIndex;

    private void Awake()
    {
        _tabs = GetComponentsInChildren<Tab>();

    }

    private void Start()
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            int index = i;
            _tabs[index].AddSelectEvent(() => OnTabClick(index));
        }
        _currentTabIndex = 0;
        for (int i = 0; i < _tabs.Length; i++)
        {
            int index = i;
            _tabs[index].Deselect();
        }
        _tabs[_currentTabIndex].Select();
    }

    public void OnTabClick(int index)
    {
        _tabs[_currentTabIndex].Deselect();
        _currentTabIndex = index;
        _tabs[_currentTabIndex].Select();

        _panelGroup.SetPageIndex(_currentTabIndex);
    }
}
