using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyNet : MonoBehaviour
{
    private int previousRow = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            Tile tile = collision.GetComponent<Tile>();
     
                if (!tile.IsDestroyed)
                {
                    PVPManager.Instance.OnNoteMissed.Invoke(PVPManager.Player.P1);

                    if (HpManager.Instance.CurrentP1Heath > 0)
                    {
                        tile.SeftDestroy();
                    }
                }
            
        }
    }
}
