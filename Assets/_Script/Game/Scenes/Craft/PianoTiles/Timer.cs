using System;
using System.Collections;
using TMPro;
using UnityEngine;
namespace Game
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        public float currentTime;
        TimeSpan timeSpan;
        public void StartTimer()
        {
            StartCoroutine(UpdateTimeCoroutine());
        }

        private IEnumerator UpdateTimeCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                currentTime++;
                timeSpan = TimeSpan.FromSeconds(currentTime);
                string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
                text.text = formattedTime;
            }
        }

        public void PauseTime()
        {
            StopCoroutine(UpdateTimeCoroutine());
        }
        public void ResetTimer()
        {
            currentTime = 0;
        }
    }
}