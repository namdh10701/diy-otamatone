using DG.Tweening;
using Game.Audio;
using System.Linq;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    private Tab[] _tabs;
    [SerializeField] private PanelGroup _panelGroup;
    private int _currentTabIndex;

    private Vector3 onSelectedScale = new Vector3(1.125f, 1.125f, 1.125f);
    private Vector3 deSelectedScale = Vector3.one;
    Tween upTween;
    Tween downTween;
    bool initilized = false;
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
        OnTabClick(_currentTabIndex);
        initilized = true;
    }

    public void OnTabClick(int index)
    {
        if (_currentTabIndex == index && initilized)
        {
            return;
        }
        if (initilized)
            AudioManager.Instance.PlaySound(SoundID.Button_Click);
        _tabs[_currentTabIndex].Deselect();
        if (upTween != null)
        {
            upTween.Kill();
        }
        upTween = _tabs[_currentTabIndex].transform.DOScale(deSelectedScale, 0f).OnComplete(
            () => upTween = null
            );
        _currentTabIndex = index;
        _tabs[_currentTabIndex].Select();
        if (downTween != null)
        {
            downTween.Kill();
        }
        downTween = _tabs[_currentTabIndex].transform.DOScale(onSelectedScale, .15f).OnComplete(
            () => downTween = null
            );
        _tabs[_currentTabIndex].transform.parent.SetAsLastSibling();
        _panelGroup?.SetPageIndex(_currentTabIndex);
    }
}
