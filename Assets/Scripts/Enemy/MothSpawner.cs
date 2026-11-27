using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LanternCourier.World;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LanternCourier.Enemy
{
    public sealed class MothSpawner : MonoBehaviour
    {
        [SerializeField] MothBrain mothPrefab;
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] DayNightCycle cycle;
        [SerializeField, FormerlySerializedAs("spawnInterval")] float waveInterval = 4f;
        [SerializeField] int maxPerWave = 3;
        [SerializeField] float waveSpread = 1.5f;
        [SerializeField] int maxAlive = 6;

        int alive;
        int wave;
        int nextPoint;

        public int Alive => alive;
        public int Wave => wave;

        void Start()
        {
            RunNightWaves(this.GetCancellationTokenOnDestroy()).Forget();
        }

        async UniTaskVoid RunNightWaves(CancellationToken token)
        {
            while (true)
            {
                bool canceled = await UniTask
                    .Delay(TimeSpan.FromSeconds(waveInterval), cancellationToken: token)
                    .SuppressCancellationThrow();
                if (canceled)
                {
                    return;
                }

                try
                {
                    if (cycle.IsNight)
                    {
                        SpawnWave();
                    }
                    else
                    {
                        wave = 0;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        void SpawnWave()
        {
            wave++;

            var point = spawnPoints[nextPoint];
            nextPoint = (nextPoint + 1) % spawnPoints.Length;

            var origin = point.position;
            int count = Mathf.Min(wave, maxPerWave, maxAlive - alive);
            for (int i = 0; i < count; i++)
            {
                var offset = Random.insideUnitCircle * waveSpread;
                Spawn(origin + new Vector3(offset.x, 0f, offset.y));
            }
        }

        void Spawn(Vector3 position)
        {
            var moth = Instantiate(mothPrefab, position, Quaternion.identity);
            moth.Init(this, cycle);
            alive++;
        }

        public void NotifyMothGone()
        {
            alive = Mathf.Max(0, alive - 1);
        }
    }
}
