
using TMPro;
using UnityEngine;
using static LevelDefinition;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    //TODO Snap To Grid
    public int Row;
    public int Col;
    public TextMeshProUGUI Text;
    public Transform Transform;
    public LevelDefinition LevelDefinition;
    public float PreviousGridSize;
    public bool IsSnapToGrid;
    public Vector3 Position;
    public Vector3 SpawnPosition;
    public float CanvasUnitHeight;
    private void Awake()
    {

        Transform = transform;
    }
    void Update()
    {
        if (!Application.isPlaying && LevelDefinition != null && SpawnPosition != null)
        {
            if (Transform.hasChanged)
            {
                Position = Transform.position;
                Transform.hasChanged = false;
                SnapToGrid();
            }
        }
    }
    protected virtual void SnapToGrid()
    {

        if (!IsSnapToGrid || LevelDefinition == null || SpawnPosition == null || CanvasUnitHeight == 0)
        {
            return;
        }

        Vector3 position = transform.position;

        // Calculate the snapped position
        float snappedX = LevelDefinition.GridWidth * Mathf.Round(position.x / LevelDefinition.GridWidth);
        float snappedY = LevelDefinition.GridHeight * Mathf.Round(position.y / LevelDefinition.GridHeight);

        // Offset the snapped position by the difference between the snapped and original position
        Vector3 snappedPosition = new Vector3(snappedX, snappedY, position.z);

        // Apply the offset to maintain the original position
        transform.position = snappedPosition;
        // Do not allow a snap to enable this flag
        Transform.hasChanged = false;
    }
}