using Game.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Game.Craft.CraftStateSequence;

namespace Game.Craft
{
    public class Booth : MonoBehaviour
    {
        public OtamatoneParts OtamatoneParts;
        private PartID[] _order;
        private Sequence _currentSequence;
        public Sequence CurrentSequence => _currentSequence;



        [SerializeField] private GameObject top;
        [SerializeField] private GameObject scrollRect;

        [SerializeField] private BoothContent _boothContent;

        readonly Dictionary<PartID, Sprite[]> _parts = new();

        private void Awake()
        {
            foreach (var part in OtamatoneParts.Parts)
            {
                _parts.Add(part.PartID, part.Sprites);
            }

        }
        public void OnSequenceEnter(Sequence sequence)
        {
            _currentSequence = sequence;
            SetBoothContent();
        }

        private void SetBoothContent()
        {
            _boothContent.SetData(_parts[_currentSequence.PartID]);
        }

        public void HideContents()
        {
            scrollRect.gameObject.SetActive(false);
            top.gameObject.SetActive(false);
        }

        public void ShowContent()
        {
            scrollRect.gameObject.SetActive(true);
            top.gameObject.SetActive(true);
        }
    }
}