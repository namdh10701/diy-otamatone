using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindingOpponentScreen : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Sprite[] sprites;
    public Image monsterLoading;
    public Image avatarLoading;
    public Image monster;
    public Image avatar;

    private DancingMonster P1Monster;
    private DancingMonster P2Monster;
    private int P1MonsterIndex;
    private int P2MonsterIndex;

    public TextMeshProUGUI monsterName;
    private IEnumerator FindOpponent()
    {
        avatar.gameObject.SetActive(false);
        monster.gameObject.SetActive(false);
        animator.SetTrigger("FindingUIAppear");
        float elapsedTime = 0;
        int imageIndex = 0;
        int previousImageIndex = -1;

        while (elapsedTime <= 3)
        {
            monsterLoading.sprite = sprites[imageIndex];
            avatarLoading.sprite = sprites[imageIndex];
            previousImageIndex = imageIndex;
            while (imageIndex == previousImageIndex)
            {
                imageIndex = UnityEngine.Random.Range(0, sprites.Length);
            }
            elapsedTime += 0.35f;
            yield return new WaitForSeconds(0.35f);
        }
        avatar.sprite = avatarLoading.sprite;
        monster.sprite = monsterLoading.sprite;
        avatar.gameObject.SetActive(true);
        monster.gameObject.SetActive(true);
        monsterName.text = "User@" + "666";
        OnOpponentSelected(imageIndex);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("SelectingSongAppear");
    }
    public void OnSelectingSongAppeared()
    {
        animator.SetTrigger("FindingUIDisappear");
    }

    public void StartFindOpponent()
    {
        StartCoroutine(FindOpponent());
    }

    public void OnOpponentSelected(int index)
    {
        /* P2Monster = Instantiate(dancingMonstersPrefab[index]);
         P1Monster = Instantiate(dancingMonstersPrefab[P1MonsterIndex]);

         P1Monster.Init(OnNoteMissed, OnNoteHit, Player.P1);
         P2Monster.Init(OnNoteMissed, OnNoteHit, Player.P2);*/
    }
}
