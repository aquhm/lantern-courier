using UnityEngine;

namespace LanternCourier.Quest
{
    public sealed class Recipient : MonoBehaviour
    {
        [SerializeField] float hopHeight = 0.4f;
        [SerializeField] float hopDuration = 0.5f;

        Vector3 restPosition;
        float hopTimer;

        public int ParcelsReceived { get; private set; }
        public DeliveryOrder LastOrder { get; private set; }

        void Awake()
        {
            restPosition = transform.localPosition;
        }

        public void Receive(DeliveryOrder order)
        {
            LastOrder = order;
            ParcelsReceived++;
            hopTimer = hopDuration;
        }

        void Update()
        {
            if (hopTimer <= 0f)
            {
                return;
            }

            hopTimer = Mathf.Max(0f, hopTimer - Time.deltaTime);
            float t = 1f - hopTimer / hopDuration;
            var position = restPosition;
            position.y += Mathf.Sin(t * Mathf.PI) * hopHeight;
            transform.localPosition = position;
        }
    }
}
