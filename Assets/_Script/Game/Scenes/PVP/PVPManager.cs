using Core.Singleton;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PVPManager : Singleton<PVPManager>
{
    public enum Player
    {
        P1, P2
    }

    public enum State
    {
        Pause, Playing, Stop
    }

    public State CurrentState;
    public SongDefinition[] songDefinitions;
    public TextMeshProUGUI songTitle;
    public SongDefinition SelectedSongDefinition;

    public DancingMonster[] dancingMonsterPrefabs;

    private DancingMonster P1Monster;
    private DancingMonster P2Monster;
    private int P1MonsterIndex;
    private int P2MonsterIndex;

    [SerializeField] private Game.Timer timer;
    [SerializeField] private GameObject endgamePanel;
    [SerializeField] private Animator playgroundAnimator;
    [SerializeField] private Transform arrows;
    [SerializeField] private PVPSceneUIController ui;

    [HideInInspector] public UnityEvent<PVPManager.Player> OnNoteMissed;
    [HideInInspector] public UnityEvent<PVPManager.Player> OnNoteHit;
    private GameObject levelGO;

    private Vector3 originalArrowPos;
    private void Start()
    {
        originalArrowPos = arrows.transform.position;
        SelectRandomSong();
        InitLevel();
    }
    public void ResetArrows()
    {
        arrows.DOMove(originalArrowPos, 0f);
    }
    public void StartGame()
    {
        timer.StartTimer();
        arrows.DOMove(originalArrowPos, 0f);
        TileRunner.Instance.StopGameEvent.RemoveListener(() => HideHUD());
        TileRunner.Instance.StopGameEvent.AddListener(() => HideHUD());
        TileRunner.Instance.ResetLevel();
        CurrentState = State.Playing;
    }

    private void SelectRandomSong()
    {
        SelectedSongDefinition = Instantiate(songDefinitions[UnityEngine.Random.Range(0, songDefinitions.Length)]);
        songTitle.text = "-" + SelectedSongDefinition.SongName + "-";
    }

    public void PauseGame()
    {

    }

    public void ResetGameForNewMatch()
    {
        Destroy(levelGO);
        TileRunner.Instance = null;
        InitLevel();
        ui.ResetGameForNewMatch();
        SelectRandomSong();

    }



    public void HideHUD()
    {
        CurrentState = State.Stop;
        ui.HideHUD();
        arrows.DOMoveY(arrows.transform.position.y - 4.8f, 1f);
    }

    public void DisplayEndgameUI()
    {
        endgamePanel.SetActive(true);
    }

    public void InitLevel()
    {
        levelGO = Instantiate(SelectedSongDefinition.HardLevel);
        TileRunner.Instance = levelGO.GetComponent<TileRunner>();
    }

    public void OnOpponentSelected(int index)
    {
        /* P2Monster = Instantiate(dancingMonstersPrefab[index]);
         P1Monster = Instantiate(dancingMonstersPrefab[P1MonsterIndex]);

         P1Monster.Init(OnNoteMissed, OnNoteHit, Player.P1);
         P2Monster.Init(OnNoteMissed, OnNoteHit, Player.P2);*/
    }
}