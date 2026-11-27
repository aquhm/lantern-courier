using LanternCourier.Core;
using LanternCourier.Player;
using UnityEngine;

namespace LanternCourier.Quest
{
    [RequireComponent(typeof(Collider))]
    public sealed class DeliveryZone : MonoBehaviour
    {
        [SerializeField] string villageName;
        [SerializeField] QuestBoard board;
        [SerializeField] Color gizmoColor = new(1f, 0.8f, 0.3f, 0.35f);

        public string VillageName => villageName;

        void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out DroneController _))
            {
                return;
            }

            var order = board.Active;
            if (order == null || order.Destination != villageName)
            {
                return;
            }

            var recipient = GetComponentInChildren<Recipient>(true);
            recipient.Receive(order);

            var session = GameSession.Instance;
            session.Wallet.Add(order.Reward);
            session.Data.deliveriesCompleted++;
            board.CompleteActive();
        }

        void OnDrawGizmos()
        {
            if (!TryGetComponent(out Collider zone))
            {
                return;
            }

            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(zone.bounds.center, zone.bounds.size);

            var recipient = GetComponentInChildren<Recipient>(true);
            if (recipient != null)
            {
                Gizmos.DrawLine(zone.bounds.center, recipient.transform.position);
            }
        }
    }
}
