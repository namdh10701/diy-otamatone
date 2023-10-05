using Game.Audio;
using Game.Datas;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
   public void RewardCoin()
    {
        GameDataManager.Instance.UpdateGold(25);
        GameDataManager.Instance.SaveDatas();
        AudioManager.Instance.PlaySound(SoundID.Coin);
    }
}
