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

    public Transform m1;
    public Transform m2;

    [SerializeField] LoadingMonsterManager lmm;
    private void Start()
    {
        m1.GetChild(Random.Range(0, m1.childCount)).gameObject.SetActive(true);
        m2.GetChild(Random.Range(0, m2.childCount)).gameObject.SetActive(true);
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
