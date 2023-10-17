using UnityEngine;

public class PlayBotDestroyNet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayBotManager.Instance.CurrentState == PlayBotManager.State.Playing)
        {
            if (collision.CompareTag("Note"))
            {
                Tile tile = collision.GetComponent<Tile>();

                if (!tile.IsDestroyed)
                {
                    tile.SeftDestroy();
                    TileRunner.Instance.OnNoteMissed.Invoke(TileRunner.Player.P1);
                }
            }
        }
    }
}
