using static Game.Craft.CraftStateSequence;
namespace Game.Craft
{
    public class Sequence
    {
        public PartID PartID;
        public bool IsCompleted;
        public bool ReEnter;
        public int SelectedItemIndex = -1;
        
        public Sequence(PartID partId)
        {
            PartID = partId;
        }
        public virtual void Enter()
        {
            if (IsCompleted)
            {
                ReEnter = true;
            }
            CraftSequenceManager.Instance.OnSequenceEnter(this);
        }
        public virtual void OnSelect(int index)
        {
            if (SelectedItemIndex != index)
            {
                switch (PartID)
                {
                    case PartID.Head:
                        CraftSequenceManager.Instance.Otamatone.OnHeadSelect(index);
                        break;
                    case PartID.Body:
                        CraftSequenceManager.Instance.Otamatone.OnBodySelect(index);
                        break;
                    case PartID.Mouth:
                        CraftSequenceManager.Instance.Otamatone.OnMouthSelect(index);
                        break;
                    case PartID.Eye:
                        CraftSequenceManager.Instance.Otamatone.OnEyeSelect(index);
                        break;
                    case PartID.Background:
                        CraftSequenceManager.Instance.Otamatone.OnBackgroundSelect(index);
                        break;
                    case PartID.Monster:
                        CraftSequenceManager.Instance.Otamatone.OnMonsterSelect(index);
                        break;
                }
                SelectedItemIndex = index;
            }
            IsCompleted = true;

        }
        public virtual void Exit()
        {
        }

        public void ResetSequence()
        {
            ReEnter = false;
            SelectedItemIndex = -1;
            IsCompleted = false;
        }

        public bool IsShowInter
        {
            get
            {
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