using UnityEngine;
using Core.Singleton;

public class PianoTilesManager : Singleton<PianoTilesManager>
{
    [SerializeField] private Transform _musicNotes;
    [SerializeField] private GameObject _tiles;
    [SerializeField] private Transform _newMonsterPos;
    [SerializeField] private Transform _newMusicNotePos;
    [SerializeField] private GameObject _booth;
    private void Start()
    {

    }
    public void EnterPianoTilesMode()

    {
        MonsterManager.Instance.Monster.transform.localScale *= .6f;
        _musicNotes.transform.localScale *= .6f;
        MonsterManager.Instance.Monster.transform.position = _newMonsterPos.position;
        _musicNotes.transform.position = _newMusicNotePos.position;
        _musicNotes.GetComponent<ParticleSystem>().Clear();
        _tiles.gameObject.SetActive(true);
        _booth.gameObject.SetActive(false);
    }
}
