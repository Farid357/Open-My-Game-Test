using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace OMG.Views
{
    public class PuzzleGalleryItemView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;

        private UnityAction _clickEvent;
        
        public void Initialize(string text, Sprite sprite, UnityAction clickEvent)
        {
            _clickEvent = clickEvent;
            _text.text = text;
            _button.image.sprite = sprite;
            _button.onClick.AddListener(clickEvent);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(_clickEvent);
        }
    }
}