using UnityEngine;
using UnityEngine.UI;
using static Game.Craft.CraftStateSequence;
using TMPro;
using System;
using DG.Tweening;

namespace Game.Craft
{
    public class SequenceButton : MonoBehaviour
    {
        public SequenceButtonBackground SequenceButtonBackground;
        public SequenceIcon SequenceIcon;

        private PartID _partID;

        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private GameObject _chatBox;
        [SerializeField] private TextMeshProUGUI _chatBoxText;
        [SerializeField] private GameObject _doneMark;
        [SerializeField] private Button _button;

        private Sequence _sequence;
        public Sequence Sequence => _sequence;
        public PartID PartID => _partID;
        private void Awake()
        {
            _chatBox.SetActive(false);
            
            _backgroundImage.color = Color.white;
            _doneMark.SetActive(false);
        }
        public void SetData(Sequence sequence)
        {
            _sequence = sequence;
            _partID = sequence.PartID;
            _backgroundImage.sprite = SequenceButtonBackground.WhiteBackground;
            _doneMark.SetActive(false);
            foreach (Part part in SequenceIcon.parts)
            {
                if (part.PartID == _partID)
                {
                    _iconImage.sprite = part.Sprites[0];
                }
            }
            _button.onClick.AddListener(() => CraftSequenceManager.Instance.OnStateSelected(_partID));
            _chatBoxText.text = _partID.ToString();
        }

        public void Select()
        {
            _backgroundImage.sprite = SequenceButtonBackground.OrangeBackground;
            _chatBox.SetActive(true);
            _chatBox.transform.DOScale(0, 0);
            _chatBox.transform.DOScale(1, .15f);
            _doneMark.SetActive(false);
        }
        public void DeSelect()
        {
            _chatBox.SetActive(false);
            if (_sequence.IsCompleted)
            {
                _doneMark.SetActive(true);
                _backgroundImage.sprite = SequenceButtonBackground.BlueBackground;
            }
            else
            {
                _backgroundImage.sprite = SequenceButtonBackground.WhiteBackground;
            }
        }
    }
}