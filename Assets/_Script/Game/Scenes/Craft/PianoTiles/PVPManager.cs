using Core.Singleton;
using UnityEngine;
public class PVPManager : Singleton<PVPManager>
{
    public GameObject LevelPrefab;
    [SerializeField] private Game.Timer timer;
    [SerializeField] private GameObject endgamePanel;
    public void StartGame()
    {
        timer.StartTimer();
        TileRunner.Instance.StopGameEvent.AddListener(() => StopGame());
        TileRunner.Instance.StartTheGame();
    }

    public void PauseGame()
    {

    }

    public void StopGame()
    {
        Debug.Log("Game Stopped");
        endgamePanel.SetActive(true);
    }

    public void InitLevel()
    {
        Instantiate(LevelPrefab);
    }
}