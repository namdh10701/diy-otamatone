using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Game.Audio;
public class CoinCollectAnim : MonoBehaviour
{
    [SerializeField] Transform[] _coins;
    [SerializeField] Transform _coinUI;


    public void PlayAnim(int amount)
    {
        StartCoroutine(CollectCoin());
    }

    private IEnumerator CollectCoin()
    {
        foreach (var coin in _coins)
        {
            coin.gameObject.SetActive(true);
            coin.localPosition = Vector3.zero;
            Vector2 targetPos = Random.insideUnitCircle * 400;
            coin.DOLocalMove(targetPos, 1).SetEase(Ease.OutBack);
        }
        yield return new WaitForSecondsRealtime(1);
        AudioManager.Instance.PlaySound(SoundID.Coin_Retrieve);
        foreach (var coin in _coins)
        {
            coin.DOMove(_coinUI.position, .5f).SetEase(Ease.OutBack).OnComplete(
                () =>
                {
                    _coinUI.DOPunchScale(new Vector3(.15f, .15f, 0), .1f);
                    coin.gameObject.SetActive(false);
                }
                );

            yield return new WaitForSecondsRealtime(.07f);
        }
    }
}
