using LanternCourier.World;
using UnityEngine;

namespace LanternCourier.Enemy
{
    public enum MothState
    {
        Wander,
        Chase,
        Absorb,
        Stunned,
    }

    [RequireComponent(typeof(MothSenses))]
    public sealed class MothBrain : MonoBehaviour
    {
        [SerializeField] float wanderSpeed = 2f;
        [SerializeField] float chaseSpeed = 4.5f;
        [SerializeField] float wanderRadius = 3f;
        [SerializeField] float hoverHeight = 1.2f;
        [SerializeField] float absorbRange = 1.2f;
        [SerializeField] float absorbPerSecond = 4f;

        MothSenses senses;
        MothSpawner owner;
        DayNightCycle cycle;
        Vector3 home;
        Vector3 wanderTarget;
        Vector3 lurePoint;
        float stunTimer;

        public MothState State { get; private set; }

        void Awake()
        {
            senses = GetComponent<MothSenses>();
            home = transform.position;
            PickWanderTarget();
        }

        public void Init(MothSpawner spawner, DayNightCycle dayNight)
        {
            owner = spawner;
            cycle = dayNight;
            home = transform.position;
            PickWanderTarget();
        }

        public void Lure(Vector3 point, float seconds)
        {
            lurePoint = point;
            lurePoint.y = hoverHeight;
            stunTimer = Mathf.Max(stunTimer, seconds);
            State = MothState.Stunned;
        }

        void Update()
        {
            if (cycle != null && !cycle.IsNight)
            {
                Destroy(gameObject);
                return;
            }

            float dt = Time.deltaTime;

            if (State == MothState.Stunned)
            {
                stunTimer -= dt;
                MoveTowards(lurePoint, wanderSpeed, dt);
                if (stunTimer <= 0f)
                {
                    State = MothState.Wander;
                }

                return;
            }

            var target = senses.NearestLight;
            if (target == null)
            {
                State = MothState.Wander;
                Wander(dt);
                return;
            }

            var goal = target.position;
            goal.y = hoverHeight;

            if (senses.Lantern == null)
            {
                // Lamps only hold moths in place; circle the lamp instead of the spawn point.
                home = goal;
                State = MothState.Wander;
                Wander(dt);
                return;
            }

            if ((goal - transform.position).sqrMagnitude <= absorbRange * absorbRange)
            {
                State = MothState.Absorb;
                senses.Lantern.Drain(absorbPerSecond * dt);
                MoveTowards(goal, wanderSpeed, dt);
            }
            else
            {
                State = MothState.Chase;
                MoveTowards(goal, chaseSpeed, dt);
            }
        }

        void Wander(float dt)
        {
            MoveTowards(wanderTarget, wanderSpeed, dt);
            if ((wanderTarget - transform.position).sqrMagnitude < 0.25f)
            {
                PickWanderTarget();
            }
        }

        void PickWanderTarget()
        {
            var offset = Random.insideUnitCircle * wanderRadius;
            wanderTarget = new Vector3(home.x + offset.x, hoverHeight, home.z + offset.y);
        }

        void MoveTowards(Vector3 goal, float speed, float dt)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal, speed * dt);
        }

        void OnDestroy()
        {
            if (owner != null)
            {
                owner.NotifyMothGone();
            }
        }
    }
}
