using LanternCourier.Economy;
using LanternCourier.Save;
using R3;
using UnityEngine;

namespace LanternCourier.Core
{
    public sealed class GameSession : MonoBehaviour
    {
        public static GameSession Instance { get; private set; }

        public ReactiveProperty<float> FuelRatio { get; } = new(1f);
        public Wallet Wallet { get; } = new();
        public SaveData Data { get; private set; } = new();

        // Created before the first scene so Game.unity can be played on its own.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void CreateInstance()
        {
            if (Instance != null)
            {
                return;
            }

            var go = new GameObject(nameof(GameSession));
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<GameSession>();
        }

        public void NewGame()
        {
            SaveSerializer.Delete();
            Data = new SaveData();
            Wallet.Coins.Value = Data.coins;
            FuelRatio.Value = 1f;
        }

        public bool Continue()
        {
            if (!SaveSerializer.TryRead(out var data))
            {
                return false;
            }

            Data = data;
            Wallet.Coins.Value = data.coins;
            FuelRatio.Value = 1f;
            return true;
        }

        public void CaptureWallet()
        {
            Data.coins = Wallet.Coins.Value;
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            FuelRatio.Dispose();
            Wallet.Dispose();
        }
    }
}
