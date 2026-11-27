using System;
using Cysharp.Threading.Tasks;
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
        [SerializeField] float installSeconds = 1.5f;

        GameSession session;
        bool installing;

        void Start()
        {
            session = GameSession.Instance;
            statusLabel.text = string.Empty;
            session.Wallet.Coins
                .Subscribe(_ => Refresh())
                .AddTo(this);
        }

        public async void BuyNextTank()
        {
            if (installing)
            {
                return;
            }

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

            installing = true;
            session.Data.tankTier = nextTier;
            statusLabel.text = $"Installing {entry.displayName}...";
            Refresh();

            await UniTask.Delay(TimeSpan.FromSeconds(installSeconds));

            Install();
            installing = false;
            Refresh();
        }

        void Install()
        {
            int tier = session.Data.tankTier + 1;
            var installed = catalog.Get(UpgradeCatalog.FuelTank, tier);
            session.Data.tankTier = tier;
            fuel.ApplyTankTier(tier);
            statusLabel.text = $"Installed {installed.displayName}";
        }

        void Refresh()
        {
            if (installing)
            {
                buyButton.interactable = false;
                return;
            }

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
