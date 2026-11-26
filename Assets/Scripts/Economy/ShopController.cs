using LanternCourier.Core;
using LanternCourier.Player;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LanternCourier.Economy
{
    public sealed class ShopController : MonoBehaviour
    {
        [SerializeField] UpgradeCatalog catalog;
        [SerializeField] LanternFuel fuel;
        [SerializeField] Button buyButton;
        [SerializeField] TMP_Text offerLabel;
        [SerializeField] TMP_Text statusLabel;

        GameSession session;

        void Start()
        {
            session = GameSession.Instance;
            statusLabel.text = string.Empty;
            session.Wallet.Coins
                .Subscribe(_ => Refresh())
                .AddTo(this);
        }

        public void BuyNextTank()
        {
            int nextTier = session.Data.tankTier + 1;
            if (!catalog.TryGet(UpgradeCatalog.FuelTank, nextTier, out var entry))
            {
                statusLabel.text = "Tank already at max";
                return;
            }

            if (!session.Wallet.TrySpend(entry.price))
            {
                statusLabel.text = "Not enough coins";
                return;
            }

            session.Data.tankTier = nextTier;
            fuel.ApplyTankTier(nextTier);
            statusLabel.text = $"Installed {entry.displayName}";
            Refresh();
        }

        void Refresh()
        {
            int nextTier = session.Data.tankTier + 1;
            if (catalog.TryGet(UpgradeCatalog.FuelTank, nextTier, out var entry))
            {
                offerLabel.text = $"{entry.displayName}  {entry.price}c";
                buyButton.interactable = session.Wallet.Coins.Value >= entry.price;
            }
            else
            {
                offerLabel.text = "Fuel tank maxed";
                buyButton.interactable = false;
            }
        }
    }
}
