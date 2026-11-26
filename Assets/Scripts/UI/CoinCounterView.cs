using LanternCourier.Core;
using R3;
using TMPro;
using UnityEngine;

namespace LanternCourier.UI
{
    public sealed class CoinCounterView : MonoBehaviour
    {
        [SerializeField] TMP_Text label;

        void Start()
        {
            GameSession.Instance.Wallet.Coins
                .Subscribe(Render)
                .AddTo(this);
        }

        void Render(int coins)
        {
            label.SetText("{0} coins", coins);
        }
    }
}
