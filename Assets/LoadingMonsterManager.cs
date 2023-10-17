using DG.Tweening;
using Game;
using Monetization.Ads;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingMonsterManager : MonoBehaviour
{
    [SerializeField] SkeletonGraphic[] monsters;

    public static int MonsterIndex;
    public Image bg;
    public Image above;
    public GameObject texts;
    private void Awake()
    {
        monsters[MonsterIndex].gameObject.SetActive(true);
    }

    public void StartLoading(string sceneName)
    {
        gameObject.SetActive(true);
        bg.DOFade(1, .2f).OnComplete(
            () =>
            {
                texts.gameObject.SetActive(true);
            }
            );

        StartCoroutine(LoadSceneCoroutine(sceneName));

    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        bool IsShowOpen = false;
        float elapsedTime = 0;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (elapsedTime <= 4)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 2 && !IsShowOpen)
            {
                IsShowOpen = true;
                //AdsController.Instance.ShowAppOpenAd();
            }
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
    }
}
