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
        [SerializeField] Recipient recipient;

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

            recipient.Receive(order);

            var session = GameSession.Instance;
            session.Wallet.Add(order.Reward);
            session.Data.deliveriesCompleted++;
            board.CompleteActive();
        }
    }
}
