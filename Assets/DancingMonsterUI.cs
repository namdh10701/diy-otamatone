using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingMonsterUI : MonoBehaviour
{
    [SerializeField] GameObject otamatone;
    [SerializeField] SkeletonGraphic _monster;
    string monsterName = "";
    string[] anims = { "Dance1", "Dance2", "Dance3", "Dance4" };

    void Start()
    {
        _monster = GetComponent<SkeletonGraphic>();
        otamatone = transform.Find("Dan").gameObject;
        monsterName = gameObject.name;
        ChangeAnim();
        InvokeRepeating("ChangeAnim", 10, 10);
    }

    private void ChangeAnim()
    {
        string random = anims[Random.Range(0, 4)];

       /* if (random == "Dance1")
        {
            if (Contain(absDance1, monsterName))
            {
                otamatone.SetActive(false);
            }
            else
            {
                otamatone.SetActive(true);
            }
        }
        if (random == "Dance2")
        {
            if (Contain(absDance2, monsterName))
            {
                otamatone.SetActive(false);
            }
            else
            {
                otamatone.SetActive(true);
            }
        }
        if (random == "Dance3")
        {
            if (Contain(absDance3, monsterName))
            {
                otamatone.SetActive(false);
            }
            else
            {
                otamatone.SetActive(true);
            }
        }
        if (random == "Dance4")
        {
            if (Contain(absDance4, monsterName))
            {
                otamatone.SetActive(false);
            }
            else
            {
                otamatone.SetActive(true);
            }
        }*/
        _monster.AnimationState.SetAnimation(0, random, true);
    }

    private bool Contain(string[] a, string t)
    {
        bool ret = false;
        foreach (string s in a)
        {
            if (s == t)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
