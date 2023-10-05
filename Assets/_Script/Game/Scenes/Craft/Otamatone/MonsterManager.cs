using Core.Singleton;
using Game.Audio;
using Game.Craft;
using System.Collections;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField] public Monster[] monsters;
    [SerializeField] public Monster Monster;
    protected override void Awake()
    {
        base.Awake();
        HideAllMonster();
    }

    public void SelectMonster(int id)
    {
        foreach (var monster in monsters)
        {
            monster.gameObject.SetActive(false);
        }
        monsters[id].gameObject.SetActive(true);
        Monster = monsters[id];
        if (id == 2)
        {
            AudioManager.Instance.PlaySound(SoundID.Cat_Sound);
        }
        else
        {
            AudioManager.Instance.PlaySound(SoundID.Monster_Voice);
        }
    }
    public void HideAllMonster()
    {
        foreach (var monster in monsters)
        {
            monster.gameObject.SetActive(false);
        }
    }
}
