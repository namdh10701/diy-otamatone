
using Core.Singleton;
using UnityEngine;

[ExecuteInEditMode]
public class AssetHolder : Singleton<AssetHolder>
{
    public Sprite UpSprite;
    public Sprite LeftSprite;
    public Sprite DownSprite;
    public Sprite RightSprite;

    public Sprite UpTrail;
    public Sprite DownTrail;
    public Sprite LeftTrail;
    public Sprite RightTrail;
    private void OnEnable()
    {
        base.Awake();
    }
}
