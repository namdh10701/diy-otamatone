using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    public enum State
    {
        Lighting, Draken
    }
    public enum ButtonDirection
    {
        Left, Down, Up, Right
    }
    public AudioSource AudioSource;
    public AudioClip clip;
    public Tween ScaleUpTween;
    public Tween ScaleDownTween;
    public ButtonDirection Direction;
    public List<Tile> CollidedTiles;

    public Material mat;
    public Tile target;
    public Coroutine lightCoroutine;
    public Coroutine darkCoroutine;
    public State CurrentState;
    public bool IsClickedOn;
    public bool IsP2Playing = false;
    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        CurrentState = State.Draken;
    }

    public void OnRealease()
    {
        if (IsP2Playing)
        {
            return;
        }
        IsClickedOn = false;
        if (ScaleDownTween != null)
        {
            ScaleDownTween.Kill();
        }
        if (ScaleUpTween != null)
        {
            ScaleUpTween.Kill();
        }
        ScaleUpTween = transform.DOScale(1f, .1f);

        if (target != null)
        {
            if (target is TrailTile)
            {
                ((TrailTile)target).OnRelease();

            }
            target = null;
        }
        mat.SetFloat("_IsActive", 0);
    }

    public void OnButton()
    {
        if (IsP2Playing)
        {
            return;
        }
        IsClickedOn = true;
        if (ScaleDownTween != null)
        {
            ScaleDownTween.Kill();
        }
        if (ScaleUpTween != null)
        {
            ScaleUpTween.Kill();
        }
        ScaleUpTween = transform.DOScale(1.15f, .1f);
        if (Peek())
        {
            mat.SetFloat("_IsActive", 1);
            Process();
        }
    }

    public bool Process()
    {
        if (target != null)
        {
            //AudioSource.PlayOneShot(clip);
            target.OnClicked();
            return true;
        }
        return false;
    }
    private void Update()
    {
        if (CurrentState == State.Lighting)
        {
        }
    }

    private IEnumerator LerpMaterialProperty(string propertyName, float targetValue, float duration)
    {
        float startTime = Time.time;
        float startValue = mat.GetFloat(propertyName);

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            float lerpedValue = Mathf.Lerp(startValue, targetValue, t);
            mat.SetFloat(propertyName, lerpedValue);
            yield return null; // Wait for the next frame
        }

        mat.SetFloat(propertyName, targetValue); // Ensure it reaches the exact target value
    }
    private bool Peek()
    {
        int lowestInRow = 10000;
        for (int i = 0; i < CollidedTiles.Count; i++)
        {
            if (CollidedTiles[i].Row < lowestInRow)
            {
                lowestInRow = CollidedTiles[i].Row;
                target = CollidedTiles[i];
            }
        }
        return target != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Endgame"))
        {
            TileRunner.Instance.StopGame();
        }
        if (collision.CompareTag("Note"))
        {
            Tile tile = collision.GetComponent<Tile>();
            if (!tile.IsP2Turn)
                CollidedTiles.Add(tile);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            Tile tile = collision.GetComponent<Tile>();
            if (tile.IsP2Turn)
            {
                if (tile.transform.position.y <= transform.position.y)
                {
                    StartCoroutine(P2Play(tile));
                }
            }
        }
    }

    private IEnumerator P2Play(Tile tile)
    {
        IsP2Playing = true;
        mat.SetFloat("_IsP2Turn", 1);
        mat.SetFloat("_IsActive", 1);
        target = tile;

        ScaleUpTween = transform.DOScale(1f, 0f);
        Process();
        yield return new WaitForSeconds(.2f);

        ScaleUpTween = transform.DOScale(1f, 0f);
        target = null;
        mat.SetFloat("_IsP2Turn", 0);
        mat.SetFloat("_IsActive", 0);
        IsP2Playing = false;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Note"))
        {
            Tile tile = collision.GetComponent<Tile>();
            if (!tile.IsP2Turn)
                CollidedTiles.Remove(tile);
        }
        if (collision.CompareTag("Trail"))
        {
            if (collision.GetComponentInParent<Tile>() == target)
            {
                target = null;
                mat.SetFloat("_IsActive", 0);
            }
        }
    }
}
