using System;
using UnityEngine;
using UnityEngine.Events;
using static PVPManager;

public class DancingMonster : MonoBehaviour
{
    private Player _player;

    public void Init(UnityEvent<PVPManager.Player> onNoteMissed, UnityEvent<PVPManager.Player> onNoteHit, Player side)
    {
        _player = side;
        onNoteHit.AddListener((player) =>
        {
            if (player == _player)
            {
                OnNoteHit();
            }
        });

        onNoteMissed.AddListener((player) =>
        {
            if (player == _player)
            {
                OnNoteMissed();
            }
        }
            );
    }

    private void OnNoteMissed()
    {
    }

    private void OnNoteHit()
    {

    }
}
