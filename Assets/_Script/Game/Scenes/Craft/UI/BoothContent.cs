using Game.Datas;
using Game.Settings;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Craft
{
    public class BoothContent : MonoBehaviour
    {
        [SerializeField] FitGridHeight FitGridHeight;
        [SerializeField] private Booth _booth;
        [SerializeField] ItemButton[] _itemButtons;
        private int _numberOfItem;
        GameData _gameData;
        [SerializeField] GameObject _scrollBar;


        Coroutine displayItem;
        private void Start()
        {
            _gameData = GameDataManager.Instance.GameDatas;
            _itemButtons = GetComponentsInChildren<ItemButton>();
        }
        public void SetData(Sprite[] sprites)
        {
            Debug.Log(sprites.Length);
            _scrollBar.SetActive(true);
            _numberOfItem = sprites.Length;
            for (int i = 0; i < _itemButtons.Length; i++)
            {
                _itemButtons[i].DeActive();
            }
            for (int i = 0; i < _numberOfItem; i++)
            {
                Locker locker = null;
                for (int j = 0; j < _gameData.LockedIndex.Count; j++)
                {
                    if (i == _gameData.LockedIndex[j].Index && _booth.CurrentSequence.PartID == _gameData.LockedIndex[j].PartID)
                    {
                        locker = _gameData.LockedIndex[j];
                    }
                }
                _itemButtons[i].Active();
                _itemButtons[i].SetData(sprites[i], i, locker);


            }
            if (displayItem != null)
            {
                StopCoroutine(displayItem);
            }
            displayItem = StartCoroutine(DisplayItems());

            for (int i = _numberOfItem; i < _itemButtons.Length; i++)
            {
                _itemButtons[i].DeActive();
            }
            FitGridHeight.UpdateHeight();
        }

        private IEnumerator DisplayItems()
        {
            for (int i = 0; i < _numberOfItem; i++)
            {
                int index = i;
                _itemButtons[index].Display();
                if (i == _booth.CurrentSequence.SelectedItemIndex)
                {
                    _itemButtons[index].SelectWithoutTriggerAnim();
                }
                yield return new WaitForSeconds(.1f);
            }
        }

        public void OnItemSelected(int index)
        {
            for (int i = 0; i < _numberOfItem; i++)
            {
                if (i != index)
                {
                    _itemButtons[i].DeSelect();
                }
            }
        }
        public void ClearData()
        {


        }


        /*  private ScrollRect _scrollRect;
          private RectTransform _contentRect;
          [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
          [SerializeField] private Canvas _canvas;
          private float _totalContentHeight;
          [SerializeField] private RectTransform _labelTransform;

          private void Start()
          {
              _scrollRect = GetComponent<ScrollRect>();
              _contentRect = _scrollRect.content;

          }

          private void CalculateTotalContentHeight()
          {
              _totalContentHeight = 0f;
              _totalContentHeight += _contentRect.GetChild(0).GetComponent<RectTransform>().rect.height;
              _totalContentHeight += _contentRect.GetChild(1).GetComponent<RectTransform>().rect.height;
              for (int i = 0; i < _verticalLayoutGroup.transform.childCount; i++)
              {
                  _totalContentHeight += _verticalLayoutGroup.transform.GetChild(i).GetComponent<RectTransform>().rect.height;
              }
              _totalContentHeight += 300;
          }

          private void UpdateContentHeight()
          {
              _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x,
                  _totalContentHeight - (_canvas.GetComponent<RectTransform>().rect.height - _labelTransform.rect.height));
              _scrollRect.verticalNormalizedPosition = 1; // Scroll to top after resizing
          }

          // You might also want to call this method whenever you add or remove content dynamically
          public void RecalculateContentHeight()
          {
              CalculateTotalContentHeight();
              UpdateContentHeight();
          }*/
    }
}