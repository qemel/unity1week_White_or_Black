using TMPro;
using u1w_2024_3.Src.Model;
using UnityEngine;

namespace u1w_2024_3.Src.View.UI
{
    public sealed class LevelText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _text.text = $"Level {Level.CurrentDisplayLevel}";
        }
    }
}