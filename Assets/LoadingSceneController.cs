using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Core.Env;
using Monetization.Ads;
using Spine.Unity;

namespace Gameplay
{
    public class LoadingSceneController : MonoBehaviour
    {
        public Image progressBar;
        private Material progressBarMaterial;
        public SkeletonGraphic Title;
        private void Start()
        {
            Title.AnimationState.SetAnimation(0, "Idle", true);
            progressBarMaterial = progressBar.material;
            Application.targetFrameRate = 60;
            if (Environment.ENV == Environment.Env.DEV)
                SceneManager.LoadScene("CraftScene");
            else
                StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            float loadingProgress = 0;
            float startTime = Time.time;
            float timeout = 4;
            AsyncOperation asyncOperation;
            asyncOperation = SceneManager.LoadSceneAsync("CraftScene");
            asyncOperation.allowSceneActivation = false;
            bool openAdShowed = false;
            float minAdditionalTimeout = 1.5f;
            float elapsedTime2 = 0;
            float newSpeed = 1 / minAdditionalTimeout;
            bool hasInternet = AdsController.Instance.HasInternet;
            // Loading phase
            while (!asyncOperation.allowSceneActivation)
            {
                float elapsedTime = Time.time - startTime;

                if ((asyncOperation.progress >= 0.9f && elapsedTime >= timeout && !openAdShowed)
                    || (asyncOperation.progress >= 0.9f && openAdShowed && elapsedTime2 >= minAdditionalTimeout)
                    || ((asyncOperation.progress >= 0.9f && (!hasInternet || AdsController.Instance.RemoveAds) && elapsedTime > minAdditionalTimeout))
                    )
                {
                    asyncOperation.allowSceneActivation = true;
                    break;
                }

                if (!openAdShowed && !AdsController.Instance.RemoveAds)
                {
                    if (AdsController.Instance.IsOpenAdReady)
                    {
                        AdsController.Instance.ShowAppOpenAd();
                        openAdShowed = true;
                        newSpeed = (1 - loadingProgress) / minAdditionalTimeout;
                    }
                }

                if (openAdShowed || AdsController.Instance.RemoveAds)
                {
                    elapsedTime2 += Time.deltaTime;
                    loadingProgress += newSpeed * Time.deltaTime;
                    progressBarMaterial.SetFloat("_Progress", loadingProgress);
                }
                else
                {
                    if (!hasInternet)
                    {
                        loadingProgress += newSpeed * Time.deltaTime;
                        progressBarMaterial.SetFloat("_Progress", loadingProgress);
                    }
                    else
                    {
                        if (elapsedTime < timeout)
                        {
                            loadingProgress = Mathf.Clamp01(elapsedTime / timeout);
                        }
                        else
                        {
                            loadingProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f + (elapsedTime - timeout) / timeout);
                        }
                        progressBarMaterial.SetFloat("_Progress", loadingProgress);
                    }
                }
                yield return null;
            }
        }
    }
}
