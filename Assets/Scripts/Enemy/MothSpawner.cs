using LanternCourier.World;
using UnityEngine;

namespace LanternCourier.Enemy
{
    public sealed class MothSpawner : MonoBehaviour
    {
        [SerializeField] MothBrain mothPrefab;
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] DayNightCycle cycle;
        [SerializeField] float spawnInterval = 4f;
        [SerializeField] int maxAlive = 6;

        float timer;
        int alive;
        int nextPoint;

        public int Alive => alive;

        void Update()
        {
            if (!cycle.IsNight)
            {
                timer = 0f;
                return;
            }

            timer += Time.deltaTime;
            if (timer < spawnInterval || alive >= maxAlive)
            {
                return;
            }

            timer = 0f;
            Spawn();
        }

        void Spawn()
        {
            var point = spawnPoints[nextPoint];
            nextPoint = (nextPoint + 1) % spawnPoints.Length;

            var moth = Instantiate(mothPrefab, point.position, Quaternion.identity);
            moth.Init(this, cycle);
            alive++;
        }

        public void NotifyMothGone()
        {
            alive = Mathf.Max(0, alive - 1);
        }
    }
}
