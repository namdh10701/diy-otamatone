using TMPro;
using UnityEngine;

public class UpdateText : MonoBehaviour
{
    TextMeshProUGUI _text;
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    Monster monster;
    void Update()
    {
        monster = MonsterManager.Instance.Monster;
        if (monster != null)
        {
            if (monster.SkeletonAnimation.AnimationState.GetCurrent(0) != null)
                _text.text = monster.SkeletonAnimation.AnimationState.GetCurrent(0).Animation.Name;
        }
    }
}
