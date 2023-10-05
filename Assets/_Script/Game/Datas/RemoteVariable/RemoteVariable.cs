using Services.FirebaseService.Remote;
using UnityEngine;
namespace Game.RemoteVariable
{
    public class RemoteVariable
    {
        public bool IsInterOn;
        public bool IsRewardOn;

        public float InterInterInterval;
        public float InterRewardInterval;
        public float OpenOpenInterval;
        public float OpenInterInterval;

        public bool BeforeWinPanelShowInter;
        public bool BgTapShowInter;
        public bool BodyTapShowInter;
        public bool EyeTapShowInter;
        public bool HeadTapShowInter;
        public bool MonsterTapShowInter;
        public bool MouthTapShowInter;
        public bool NextBtnTapShowInter;
        public bool SequenceBtnTapShowInter;
        public static T Convert<T>(RemoteJson remoteJson)
        {
            return JsonUtility.FromJson<T>((string)remoteJson.Value);
        }
        public static RemoteVariable Convert(MyRemoteVariableCollection myRemoteVariableCollection)
        {
            RemoteVariable remoteVariable = new RemoteVariable();
            remoteVariable.IsInterOn = myRemoteVariableCollection.IsInterOn.Value;
            remoteVariable.IsRewardOn = myRemoteVariableCollection.IsRewardOn.Value;

            remoteVariable.InterInterInterval = (float)myRemoteVariableCollection.InterInterInterval.Value;
            remoteVariable.InterRewardInterval = (float)myRemoteVariableCollection.InterRewardInterval.Value;
            remoteVariable.OpenOpenInterval = (float)myRemoteVariableCollection.OpenOpenInterval.Value;
            remoteVariable.OpenInterInterval = (float)myRemoteVariableCollection.OpenInterInterval.Value;

            remoteVariable.BeforeWinPanelShowInter = myRemoteVariableCollection.BeforeWinPanelShowInter.Value;
            remoteVariable.BgTapShowInter = myRemoteVariableCollection.BgTapShowInter.Value;
            remoteVariable.BodyTapShowInter = myRemoteVariableCollection.BodyTapShowInter.Value;
            remoteVariable.EyeTapShowInter = myRemoteVariableCollection.EyeTapShowInter.Value;
            remoteVariable.HeadTapShowInter = myRemoteVariableCollection.HeadTapShowInter.Value;
            remoteVariable.MonsterTapShowInter = myRemoteVariableCollection.MonsterTapShowInter.Value;
            remoteVariable.MouthTapShowInter = myRemoteVariableCollection.MouthTapShowInter.Value;
            remoteVariable.NextBtnTapShowInter = myRemoteVariableCollection.NextBtnTapShowInter.Value;
            remoteVariable.SequenceBtnTapShowInter = myRemoteVariableCollection.SequenceBtnTapShowInter.Value;

            return remoteVariable;
        }
    }
}