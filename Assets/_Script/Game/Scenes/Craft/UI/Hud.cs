using DG.Tweening;
using Game.Audio;
using Game.Datas;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class Hud : MonoBehaviour
    {
        SoundID _updateCoin;
        [SerializeField] TextMeshProUGUI cointText;
        [SerializeField] TextMeshProUGUI noteText;
        [SerializeField] Transform _coinUI;
        private int currentNote;
        private int currentCoin;
        private int targetCoin;

        private Coroutine addCoinCoroutine; // Reference to the running coroutine
        private Coroutine removeCoinCoroutine; // Reference to the running coroutine
        [SerializeField] private CoinCollectAnim _coinCollectAnim;
        private void OnEnable()
        {
            cointText.text = GameDataManager.Instance.GameDatas.Coin.ToString();
            noteText.text = GameDataManager.Instance.GameDatas2.Notes.ToString();
            currentNote = GameDataManager.Instance.GameDatas2.Notes;
            targetCoin = GameDataManager.Instance.GameDatas.Coin;
            currentCoin = targetCoin;
            GameDataManager.Instance.OnGoldUpdate.AddListener(amount => UpdateGoldText(amount));
            GameDataManager.Instance.OnNoteUpdate.AddListener(amount => UpdateNoteText(amount));
        }
        private void OnDisable()
        {
            GameDataManager.Instance.OnGoldUpdate.RemoveListener(amount => UpdateGoldText(amount));
            GameDataManager.Instance.OnNoteUpdate.RemoveListener(amount => UpdateNoteText(amount));
        }

        private void UpdateNoteText(int amount)
        {
            currentNote += amount;
            noteText.text = currentNote.ToString();
        }

        public void UpdateGoldText(int amount)
        {
            if (amount > 0)
            {
                if (addCoinCoroutine != null)
                {
                    StopCoroutine(addCoinCoroutine); // Stop the existing coroutine if it's running
                }
                targetCoin += amount; // Update the target coin count
                addCoinCoroutine = StartCoroutine(IncrementCoinSmoothly());
                _coinCollectAnim.PlayAnim(amount);
            }
            else
            {
                if (removeCoinCoroutine != null)
                {
                    StopCoroutine(removeCoinCoroutine); // Stop the existing coroutine if it's running
                }
                targetCoin += amount; // Update the target coin count
                removeCoinCoroutine = StartCoroutine(DecrementCoinSmoothly());
            }
        }



        private IEnumerator IncrementCoinSmoothly()
        {

            yield return new WaitForSeconds(.8f);
            //AudioController.Instance.PlaySound("getCoin");
            float duration = 1.5f; // Change this value to adjust the duration of the smooth incrementation

            float elapsedTime = 0f;
            int startCoinCount = currentCoin;
            int targetCount = targetCoin;
            //_coinUI.transform.DOPunchScale(new Vector3(.17f, .17f, 0), duration,6).SetEase(Ease.InSine);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                currentCoin = Mathf.RoundToInt(Mathf.Lerp(startCoinCount, targetCount, elapsedTime / duration));
                cointText.text = currentCoin.ToString();
                yield return null;
            }

            currentCoin = targetCount;
            cointText.text = targetCount.ToString();
        }
        private IEnumerator DecrementCoinSmoothly()
        {
            float duration = 1.3f; // Change this value to adjust the duration of the smooth decrementation

            float elapsedTime = 0f;
            int startCoinCount = currentCoin;
            int targetCount = targetCoin;

            _coinUI.transform.DOPunchScale(new Vector3(.17f, .17f, 0), duration, 8).SetEase(Ease.InOutFlash);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                currentCoin = Mathf.RoundToInt(Mathf.Lerp(startCoinCount, targetCount, elapsedTime / duration));
                cointText.text = currentCoin.ToString();
                yield return null;
            }

            currentCoin = targetCount;
            cointText.text = targetCount.ToString();
        }
    }
}