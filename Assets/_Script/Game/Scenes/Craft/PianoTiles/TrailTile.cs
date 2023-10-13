using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TrailTile : Tile
{
    public SpriteRenderer Trail;
    public SpriteRenderer TrailTop;
    public float TrailHeight;
    public Material TrailMat;
    public Material TrailTopMat;
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
                TrailTop.sprite = AssetHolder.Instance.LeftTrailTop;
                break;
            case 1:
                Trail.sprite = AssetHolder.Instance.DownTrail;
                TrailTop.sprite = AssetHolder.Instance.DownTrailTop;
                break;
            case 2:
                Trail.sprite = AssetHolder.Instance.UpTrail;
                TrailTop.sprite = AssetHolder.Instance.UpTrailTop;
                break;
            case 3:
                Trail.sprite = AssetHolder.Instance.RightTrail;
                TrailTop.sprite = AssetHolder.Instance.RightTrailTop;
                break;
        }
    }



    public void SetTrailHeight(float trailHeight)
    {
        TrailHeight = trailHeight;
        Trail.transform.localScale = new Vector3(1,
        trailHeight, 2);
        TrailTop.transform.localPosition = new Vector3(0, 0.6945f * Trail.transform.localScale.y, 0);
    }
    public void OnRelease()
    {
        Debug.Log("OnRelease");
        IsClicked = false;
        StartCoroutine(LerpMaterialProperty("_IsActive", 0f, .2f));
    }
    public override void OnClicked()
    {
        Debug.Log("OnClick");
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
                float gothroughAmount = Mathf.Abs(transform.position.y - (-TileRunner.Instance.CameraYBound + 2.4f));
                float y = (0.6945f * Trail.transform.localScale.y - gothroughAmount) / (0.6945f * Trail.transform.localScale.y);
                SetTrailHeightAlpha(y);
                if (y <= 0)
                {
                    y = (1 + 0.6945f * Trail.transform.localScale.y) - (gothroughAmount) / 1;
                    TrailTopMat.SetFloat("_Height", y);
                }
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
            TrailTopMat.SetFloat(propertyName, lerpedValue);
            TrailMat.SetFloat(propertyName, lerpedValue);
            yield return null; // Wait for the next frame
        }
        TrailTopMat.SetFloat(propertyName, targetValue);

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
