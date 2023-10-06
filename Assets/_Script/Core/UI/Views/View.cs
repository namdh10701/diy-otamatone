using UnityEngine;

namespace Core.UI
{
    public abstract class View : MonoBehaviour
    {
        public abstract void Show();

        public abstract void Hide();
    }
}