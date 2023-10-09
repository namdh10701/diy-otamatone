
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using static LevelDefinition;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    //TODO Snap To Grid
    public int Row;
    public int Col;
    public Transform Transform;
    public LevelDefinition LevelDefinition;
    public bool IsSnapToGrid;
    public SpriteRenderer sp;
    public bool IsEditMode = true;
    public NoteType Type;
    public enum NoteType
    {
        Normal, Trail
    }
    protected virtual void Awake()
    {

        Type = NoteType.Normal;
        Transform = transform;
        sp = GetComponent<SpriteRenderer>();
        if (!IsEditMode)
        {
            return;
        }
        TileRunner tileRunner = FindAnyObjectByType<TileRunner>()
            .GetComponent<TileRunner>();
        if (tileRunner != null)
        {
            if (PrefabUtility.IsPartOfNonAssetPrefabInstance(gameObject))
            {
                Transform.SetParent(tileRunner.NoteRoot);
                LevelDefinition = tileRunner.LevelDefinition;
                return;
            }
            Transform.SetParent(tileRunner.NoteRoot);

        }
    }

    public virtual void OnClicked()
    {
        //TileRunner.Instance.ActiveTiles.Remove(this);
        sp.enabled = false;
    }

    public virtual void OnReset()
    {
        sp.enabled = true;
    }

    protected virtual void Update()
    {
        if (!IsEditMode)
        {
            return;
        }
        if (!Application.isPlaying && LevelDefinition != null)
        {
            if (Transform.hasChanged)
            {
                Transform.hasChanged = false;
                SnapToGrid();
            }
        }
    }
    protected virtual void SnapToGrid()
    {
        if (!IsSnapToGrid || LevelDefinition == null)
        {
            return;
        }
        Vector3 position = transform.position;
        float x = 0;
        float y = 1;
        while (x <= 8.8f)
        {
            x = LevelDefinition.GridHeight / 4 * y;
            y++;
        }
        float delta = x - 8.8f;
        // Calculate the snapped position
        float snappedX = LevelDefinition.GridWidth * Mathf.Round((position.x / LevelDefinition.GridWidth) + .9f) - .9f;
        float snappedY = LevelDefinition.GridHeight / 4 * Mathf.Round(position.y / (LevelDefinition.GridHeight / 4) + delta) - delta;
        //lay step lon hon gan nhat offset - y cua spawn pos


        // Offset the snapped position by the difference between the snapped and original position
        Vector3 snappedPosition = new Vector3(snappedX, snappedY, position.z);
        // Apply the offset to maintain the original position
        transform.position = snappedPosition;
        Col = (int)((snappedX + 2.7f) / LevelDefinition.GridWidth);
        Row = (int)Mathf.Round((transform.localPosition.y / (LevelDefinition.GridHeight / 4)));
        //Debug.Log(LevelDefinition.GridWidth / 4);
        name = $"C:{Col} R:{Row}";
        // Do not allow a snap to enable this flag
        Transform.hasChanged = false;
        switch (Col)
        {
            case 0:
                sp.sprite = AssetHolder.Instance.LeftSprite;
                break;
            case 1:
                sp.sprite = AssetHolder.Instance.DownSprite;
                break;
            case 2:
                sp.sprite = AssetHolder.Instance.UpSprite;
                break;
            case 3:
                sp.sprite = AssetHolder.Instance.RightSprite;
                break;
        }
    }
}