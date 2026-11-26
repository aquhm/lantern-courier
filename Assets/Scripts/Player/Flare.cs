using LanternCourier.Enemy;
using UnityEngine;

namespace LanternCourier.Player
{
    public sealed class Flare : MonoBehaviour
    {
        [SerializeField] LayerMask hitMask;
        [SerializeField] float speed = 12f;
        [SerializeField] float flightTime = 0.6f;
        [SerializeField] float restHeight = 0.2f;
        [SerializeField] float burnTime = 4f;
        [SerializeField] float contactRadius = 0.3f;
        [SerializeField] float lureRadius = 6f;
        [SerializeField] float stunSeconds = 2.5f;
        [SerializeField] float pulseInterval = 0.25f;

        readonly Collider[] hits = new Collider[16];

        FlareThrower owner;
        Vector3 velocity;
        float age;
        float landedAt;
        float nextPulse;
        bool landed;

        public bool Landed => landed;

        public void Bind(FlareThrower thrower)
        {
            owner = thrower;
        }

        public void Launch(Vector3 origin, Vector3 direction)
        {
            transform.position = origin;
            velocity = direction * speed;
            // Drop so the arc meets the ground right when flight time runs out.
            velocity.y = -(origin.y - restHeight) / flightTime;
            age = 0f;
            landed = false;
            gameObject.SetActive(true);
        }

        void Update()
        {
            age += Time.deltaTime;

            if (!landed)
            {
                transform.position += velocity * Time.deltaTime;
                int contacts = Physics.OverlapSphereNonAlloc(transform.position, contactRadius, hits, hitMask, QueryTriggerInteraction.Collide);
                if (contacts > 0 || age >= flightTime)
                {
                    Land();
                }

                return;
            }

            if (age >= nextPulse)
            {
                nextPulse = age + pulseInterval;
                Pulse();
            }

            if (age - landedAt >= burnTime)
            {
                owner.Return(this);
            }
        }

        void Land()
        {
            landed = true;
            landedAt = age;
            nextPulse = age;
            var position = transform.position;
            position.y = restHeight;
            transform.position = position;
        }

        void Pulse()
        {
            var center = transform.position;
            int count = Physics.OverlapSphereNonAlloc(center, lureRadius, hits, hitMask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < count; i++)
            {
                if (hits[i].TryGetComponent(out MothBrain moth))
                {
                    moth.Lure(center, stunSeconds);
                }
            }
        }
    }
}
