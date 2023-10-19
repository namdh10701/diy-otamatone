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

    public static int MonsterIndex = -1;
    public Image bg;
    public Image above;
    public Image progressBar;
    public GameObject texts;
    private void Awake()
    {
        bg.DOFade(0, 0);
        texts.SetActive(false);
        if (MonsterIndex == -1)
        {
            MonsterIndex = UnityEngine.Random.Range(0, 8);
        }
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
        float elapsedTime = 0;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (elapsedTime <= 2)
        {
            elapsedTime += Time.deltaTime;
            progressBar.fillAmount = elapsedTime / 2;
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
    }
}
