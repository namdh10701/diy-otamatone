using UnityEngine;
using Core.UI;
using TMPro;

namespace Game.UI
{
    public class NotificationPanel : BasePopup
    {
        [SerializeField] TextMeshProUGUI _msgText;
        public void SetData(string msg)
        {
            _msgText.text = msg;
        }
    }
}