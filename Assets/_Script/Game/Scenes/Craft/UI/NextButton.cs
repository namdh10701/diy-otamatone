using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Craft
{
    public class NextButton : MonoBehaviour
    {
        public bool IsActive;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Material _material;
        [SerializeField] private Texture[] _textures;
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void OnClick()
        {
            switch (CraftSequenceManager.Instance.CurrentSeqeuence.PartID)
            {
                case CraftStateSequence.PartID.Head:
                    _material.SetTexture("_MainTex", _textures[0]);
                    break;
                case CraftStateSequence.PartID.Body:

                    _material.SetTexture("_MainTex", _textures[1]);
                    break;
                case CraftStateSequence.PartID.Eye:

                    _material.SetTexture("_MainTex", _textures[2]);
                    break;
                case CraftStateSequence.PartID.Mouth:

                    _material.SetTexture("_MainTex", _textures[3]);
                    break;
                case CraftStateSequence.PartID.Background:

                    _material.SetTexture("_MainTex", _textures[4]);
                    break;
                case CraftStateSequence.PartID.Monster:
                    _material.SetTexture("_MainTex", _textures[5]);
                    Debug.Log("monster");
                    break;
            }
            _particleSystem.Play();
        }

        public void Activate()
        {
            IsActive = true;
            _button.interactable = true;
            _text.color = new Color(0.11f, 0.11f, 0.11f, 1);
        }

        public void DeActivate()
        {
            IsActive = false;
            _button.interactable = false;
            _text.color = new Color(0.11f, 0.11f, 0.11f, 0.3f);
        }
    }
}