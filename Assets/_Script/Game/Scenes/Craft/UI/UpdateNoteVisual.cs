using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpdateNoteVisual : MonoBehaviour
{
    Vector3 originalPos;
    Graphic[] graphics;
    private void Awake()
    {
        graphics = GetComponentsInChildren<Graphic>(true).ToArray();
        originalPos = transform.position;
    }
    Tween tween;
    public void Play()
    {
        if (tween != null)
        {
            if (tween.IsPlaying())
            {
                tween.Kill();
            }
        }
        gameObject.SetActive(true);
        transform.position = originalPos;
        foreach (Graphic g in graphics)
        {
            g.DOFade(1, 0f);
        }
        tween = transform.DOLocalMoveY(transform.position.y + 100, .5f);
        foreach (Graphic g in graphics)
        {
            g.DOFade(0, .5f).OnComplete(() => gameObject.SetActive(false));
        }

    }
}