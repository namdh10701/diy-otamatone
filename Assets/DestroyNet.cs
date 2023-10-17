using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyNet : MonoBehaviour
{
    private int previousRow = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PVPManager.Instance.CurrentState == PVPManager.State.Playing)
        {
            if (collision.CompareTag("Note"))
            {
                Tile tile = collision.GetComponent<Tile>();

                if (!tile.IsDestroyed)
                {
                    HpManager.Instance.OnNoteMissed(tile.IsP2Turn ? TileRunner.Player.P2 : TileRunner.Player.P1);
                    if (HpManager.Instance.CurrentP1Heath > 0)
                    {
                        tile.SeftDestroy();
                    }
                }

            }
        }
    }
}
