using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform, Width, Height, FixedRow, FixedColumn
    }
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public FitType fitType;
    public bool fitX;
    public bool fitY;
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }


        if (fitType == FitType.Width || fitType == FitType.FixedColumn)
        {
            rows = Mathf.CeilToInt(transform.childCount / columns) + 1;
        }

        if (fitType == FitType.Height || fitType == FitType.FixedRow)
        {
            columns = Mathf.CeilToInt(transform.childCount / rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;
        float cellWidth = parentWidth / columns - spacing.x / columns * 2 - padding.left / columns - padding.right / columns;
        float cellHeight = parentHeight / rows - spacing.y / rows * 2 - padding.top / rows - padding.bottom / rows;

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;
        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;
            var item = rectChildren[i];
            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }
    public override void CalculateLayoutInputVertical()
    {
       
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}
