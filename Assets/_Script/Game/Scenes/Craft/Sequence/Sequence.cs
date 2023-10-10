using Game.Datas;
using System;
using UnityEngine;
using static Game.Craft.CraftStateSequence;
namespace Game.Craft
{
    public abstract class Sequence : MonoBehaviour
    {
        [SerializeField] protected Otamatone _otamatone;
        [SerializeField] protected PartID _partID;
        protected bool _isCompeleted = false;
        protected bool _reEnter = false;
        protected int _selectedItemIndex = -1;
        public bool IsCompleted => _isCompeleted;
        public bool ReEnter => _reEnter;
        public int SelectedItemIndex => _selectedItemIndex;


        public PartID PartID => _partID;

        public virtual void Enter()
        {
            if (_isCompeleted)
            {
                _reEnter = true;
            }
            CraftSequenceManager.Instance.OnSequenceEnter(this);
        }
        public virtual void OnSelect(int index)
        {
            if (_selectedItemIndex != index)
            {
                switch (_partID)
                {
                    case PartID.Head:
                        _otamatone.OnHeadSelect(index);
                        break;
                    case PartID.Body:
                        _otamatone.OnBodySelect(index);
                        break;
                    case PartID.Mouth:
                        _otamatone.OnMouthSelect(index);
                        break;
                    case PartID.Eye:
                        _otamatone.OnEyeSelect(index);
                        break;
                    case PartID.Background:
                        _otamatone.OnBackgroundSelect(index);
                        break;
                    case PartID.Monster:
                        _otamatone.OnMonsterSelect(index);
                        break;
                }
                _selectedItemIndex = index;
            }
            _isCompeleted = true;

        }
        public virtual void Exit()
        {
        }

        public void ResetSequence()
        {
            _reEnter = false;
            _selectedItemIndex = -1;
            _isCompeleted = false;
        }

        public bool IsShowInter
        {
            get
            {
                Debug.Log(PartID);
                switch (PartID)
                {
                    case PartID.Head:
                        return CraftSequenceManager.HeadTapShowInter;
                    case PartID.Body:
                        return CraftSequenceManager.BodyTapShowInter;
                    case PartID.Mouth:
                        return CraftSequenceManager.MouthTapShowInter;
                    case PartID.Eye:
                        return CraftSequenceManager.EyeTapShowInter;
                    case PartID.Background:
                        return CraftSequenceManager.BgTapShowInter;
                    case PartID.Monster:
                        return CraftSequenceManager.MonsterTapShowInter;
                }
                return false;
            }
        }
    }
}