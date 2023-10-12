using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TrailTile : Tile
{
    public SpriteRenderer Trail;
    public float TrailHeight;
    public Material TrailMat;
    public bool IsClicked;
    protected override void Awake()
    {
        base.Awake();
        Type = NoteType.Trail;
    }
    protected override void SnapToGrid()
    {
        base.SnapToGrid();
        switch (Col)
        {
            case 0:
                Trail.sprite = AssetHolder.Instance.LeftTrail;
                break;
            case 1:
                Trail.sprite = AssetHolder.Instance.DownTrail;
                break;
            case 2:
                Trail.sprite = AssetHolder.Instance.UpTrail;
                break;
            case 3:
                Trail.sprite = AssetHolder.Instance.RightTrail;
                break;
        }
    }



    public void SetTrailHeight(float trailHeight)
    {
        TrailHeight = trailHeight;
        Trail.transform.localScale = new Vector3(1,
        trailHeight / 0.5f, 1);
    }
    public void OnRelease()
    {
        IsClicked = false;
        StartCoroutine(LerpMaterialProperty("_IsActive", 0f, .2f));
    }
    public override void OnClicked()
    {
        IsClicked = true;

        StartCoroutine(LerpMaterialProperty("_IsActive", 1f, .2f));
        GetComponent<SpriteRenderer>().enabled = false;
    }
    protected override void Update()
    {
        if (IsEditMode)
        {
            if (!Application.isPlaying && LevelDefinition != null)
            {
                if (Transform.hasChanged)
                {
                    Transform.hasChanged = false;
                    SnapToGrid();
                }
            }
        }
        else
        {
            if (IsClicked)
            {

                float gothroughAmount = Mathf.Abs(transform.position.y - (-4f));
                float y = (TrailHeight * (LevelDefinition.GridHeight / 4) - gothroughAmount) / (TrailHeight * (LevelDefinition.GridHeight / 4));
                SetTrailHeightAlpha(y);
            }
        }
    }
    private IEnumerator LerpMaterialProperty(string propertyName, float targetValue, float duration)
    {
        float startTime = Time.time;
        float startValue = TrailMat.GetFloat(propertyName);

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            float lerpedValue = Mathf.Lerp(startValue, targetValue, t);
            TrailMat.SetFloat(propertyName, lerpedValue);
            yield return null; // Wait for the next frame
        }

        TrailMat.SetFloat(propertyName, targetValue); // Ensure it reaches the exact target value
    }

    public void SetTrailHeightAlpha(float height)
    {
        TrailMat.SetFloat("_Height", height);
    }

    public override void OnReset()
    {
        base.OnReset();
        TrailMat.SetFloat("_Height", 1);
    }
}
