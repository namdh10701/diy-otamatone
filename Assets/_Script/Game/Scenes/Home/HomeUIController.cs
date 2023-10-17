using Core.Singleton;
using Game.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIController : Singleton<HomeUIController>
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI noteText;

    GameData2 gameData2;
    GameData gameData;

    [SerializeField] LoadingMonsterManager lmm;
    private void Start()
    {
        gameData2 = GameDataManager.Instance.GameDatas2;
        gameData = GameDataManager.Instance.GameDatas;

        coinText.text = gameData.Coin.ToString();
        noteText.text = gameData2.Notes.ToString();
    }

    public void EnterDIY()
    {
        SceneManager.LoadScene("DIYScene");
    }

    public void EnterPVP()
    {
        gameData2.Notes -= 5;
        GameDataManager.Instance.SaveDatas2();
        lmm.StartLoading("PVPScene");
    }

    public void EnterPlayBot(SongItem songItem)
    {
        lmm.StartLoading("PlayBotScene");
    }




}
