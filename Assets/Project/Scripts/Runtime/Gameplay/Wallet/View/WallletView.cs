using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OMG.Views
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> _coinsTexts;

        public void Show(string moneyText)
        {
            _coinsTexts.ForEach(text => text.text = moneyText);
        }
    }
}