using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    private List<Tile> CollidedTiles;
    public Texture sparkTex;
    public ParticleSystem spark;
    public ParticleSystem OnHitSpark;

    public Tween ScaleUpTween;
    public Tween ScaleDownTween;

    private bool isAbleToBeClicked = true;
    public float CooldownTime = .05f;
    private float animationTime = 0.075f;

    public Material mat;
    public Tile target;
    public Coroutine lightCoroutine;
    public Coroutine darkCoroutine;

    private bool IsClickedOn;
    public bool IsP2Playing = false;
    private Coroutine p2PlayCoroutine;

    private float _originalScale;
    private void Awake()
    {
        _originalScale = transform.localScale.x;
        spark.GetComponent<Renderer>().material.SetTexture("_MainTex", sparkTex);
        mat = GetComponent<SpriteRenderer>().material;
        CollidedTiles = new List<Tile>();
    }

    /*   public void OnRealease()
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
                    spark.Stop();
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
    */

    public void OnButton()
    {
        if (!isAbleToBeClicked)
            return;

        IsClickedOn = true;
        isAbleToBeClicked = false;
        target = Peek();
        if (target == null)
        {
            Invoke("ResetClick", CooldownTime);
            ScaleUpTween = transform.DOScale(_originalScale * 1.15f, animationTime / 2f).OnComplete(
            () =>
            {
                ScaleDownTween = transform.DOScale(_originalScale, animationTime);
            }
            );
            return;
        }
        else
        {
            if (target.IsDestroyed)
            {
                return;
            }
            TileRunner.Instance.OnNoteHit.Invoke(IsP2Playing ? TileRunner.Player.P2 : TileRunner.Player.P1);
            mat.SetFloat("_IsActive", 1);
            if (target.Type == Tile.NoteType.Normal)
            {
                Invoke("ResetClick", CooldownTime);
                ScaleUpTween = transform.DOScale(_originalScale * 1.15f, animationTime / 2f).OnComplete(
                () =>
                {
                    ProcessNormalTile();
                    ScaleDownTween = transform.DOScale(_originalScale, animationTime).OnComplete(
                        () =>
                        {
                            mat.SetFloat("_IsActive", 0);
                        }
                        );
                }
                );

            }
            else if (target.Type == Tile.NoteType.Trail)
            {
                ScaleUpTween = transform.DOScale(_originalScale * 1.15f, .1f);
                ProcessTrailTile();
            }
        }
    }

    private void ProcessNormalTile()
    {
        if (target != null)
        {
            OnHitSpark.Play();
            target.OnClicked();
        }
    }
    private void ProcessTrailTile()
    {
        if (target != null)
        {
            target.OnClicked();
            OnHitSpark.Play();
            spark.Play();
        }
    }

    public void OnRealease()
    {
        if (target != null)
        {
            if (target.Type == Tile.NoteType.Trail)
            {
                Invoke("ResetClick", 0);
                mat.SetFloat("_IsActive", 0);
                ScaleDownTween = transform.DOScale(_originalScale, animationTime).OnComplete(
                       () =>
                       {
                           
                       }
                       );
                spark.Stop();
                bool IsTotallyCleared = ((TrailTile)target).OnRelease();
                if (IsTotallyCleared)
                {
                    target = null;
                    TileRunner.Instance.OnNoteHit.Invoke(IsP2Playing ? TileRunner.Player.P2 : TileRunner.Player.P1);
                }
            }
        }
    }






    private void ResetClick()
    {
        isAbleToBeClicked = true;
    }

    private Tile Peek()
    {
        target = null;
        int lowestInRow = 10000;
        for (int i = 0; i < CollidedTiles.Count; i++)
        {
            if (CollidedTiles[i].Row < lowestInRow)
            {
                lowestInRow = CollidedTiles[i].Row;
                target = CollidedTiles[i];
            }
        }
        return target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Endgame"))
        {
            TileRunner.Instance.StopGame();

        }
        if (collision.CompareTag("LastNote"))
        {

            TileRunner.Instance.LastNotePassedEvent.Invoke();

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
                    if (p2PlayCoroutine == null)
                    {
                        TileRunner.Instance.OnNoteHit.Invoke(TileRunner.Player.P2);
                        p2PlayCoroutine = StartCoroutine(P2Play(tile));
                    }
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
        if (tile.Type == Tile.NoteType.Normal)
        {
            ProcessNormalTile();
        }
        yield return new WaitForSeconds(.2f);
        ScaleUpTween = transform.DOScale(1f, 0f);
        target = null;
        mat.SetFloat("_IsP2Turn", 0);
        mat.SetFloat("_IsActive", 0);
        IsP2Playing = false;
        p2PlayCoroutine = null;

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
                OnRealease();
            }
        }
    }
}
