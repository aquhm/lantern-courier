using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
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
            SaveAsync(destroyCancellationToken);
        }

        void OnApplicationPause(bool paused)
        {
            if (paused)
            {
                SaveAsync(destroyCancellationToken).Forget();
            }
        }

        public async UniTask SaveAsync(CancellationToken token)
        {
            var session = GameSession.Instance;
            session.CaptureWallet();
            string json = JsonUtility.ToJson(session.Data, true);

            string path = SaveSerializer.FilePath;
            string staging = path + ".tmp";

            await UniTask.SwitchToThreadPool();
            await File.WriteAllTextAsync(staging, json, token);
            File.Move(staging, path);
            await UniTask.SwitchToMainThread(token);

            SaveCount++;
        }
    }
}
