using Core.Singleton;
using DG.Tweening;
using UnityEngine;
public class PVPManager : Singleton<PVPManager>
{
    public GameObject LevelPrefab;
    [SerializeField] private Game.Timer timer;
    [SerializeField] private GameObject endgamePanel;
    [SerializeField] private Animator playgroundAnimator;
    [SerializeField] private Transform arrows;
    
    public void StartGame()
    {
        timer.StartTimer();
        TileRunner.Instance.StopGameEvent.AddListener(() => HideHUD());
        TileRunner.Instance.StartTheGame();
    }

    public void PauseGame()
    {

    }

    public void HideHUD()
    {
        arrows.DOMoveY(arrows.transform.position.y - 4.8f, 1f);
        playgroundAnimator.SetTrigger("Disappear");
    }

    public void DisplayEndgameUI()
    {
        endgamePanel.SetActive(true);
    }

    public void InitLevel()
    {
        Instantiate(LevelPrefab);
    }
}