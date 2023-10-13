
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

    public Sprite UpTrailTop;
    public Sprite DownTrailTop;
    public Sprite LeftTrailTop;
    public Sprite RightTrailTop;

    public Sprite P2UpSprite;
    public Sprite P2DownSprite;
    public Sprite P2LeftSprite;
    public Sprite P2RightSprite;

    private void OnEnable()
    {
        base.Awake();
    }
}
