using LanternCourier.Core;
using LanternCourier.Quest;
using UnityEngine;

namespace LanternCourier.Save
{
    public sealed class CheckpointSaver : MonoBehaviour
    {
        [SerializeField] QuestBoard board;

        public int SaveCount { get; private set; }

        void Awake()
        {
            board.OrderCompleted += OnOrderCompleted;
        }

        void OnDestroy()
        {
            board.OrderCompleted -= OnOrderCompleted;
        }

        void OnOrderCompleted(DeliveryOrder order)
        {
            Save();
        }

        public void Save()
        {
            var session = GameSession.Instance;
            session.CaptureWallet();
            SaveSerializer.Write(session.Data);
            SaveCount++;
        }
    }
}
