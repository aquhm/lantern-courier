using System;
using System.Collections.Generic;
using UnityEngine;

namespace LanternCourier.Quest
{
    public sealed class QuestBoard : MonoBehaviour
    {
        [Serializable]
        struct Route
        {
            public string destination;
            public int reward;
        }

        [SerializeField] string origin = "Warehouse";
        [SerializeField] Route[] routes;
        [SerializeField] int boardSize = 3;

        readonly List<DeliveryOrder> available = new();
        int nextId = 1;
        int nextRoute;

        public event Action<DeliveryOrder> OrderAdded;
        public event Action<DeliveryOrder> OrderRemoved;
        public event Action<DeliveryOrder> OrderAccepted;
        public event Action<DeliveryOrder> OrderCompleted;

        public IReadOnlyList<DeliveryOrder> Available => available;
        public DeliveryOrder Active { get; private set; }

        void Start()
        {
            Restock();
        }

        public bool Accept(DeliveryOrder order)
        {
            if (Active != null || !available.Remove(order))
            {
                return false;
            }

            Active = order;
            OrderRemoved?.Invoke(order);
            OrderAccepted?.Invoke(order);
            return true;
        }

        public void CompleteActive()
        {
            var order = Active;
            if (order == null)
            {
                return;
            }

            Active = null;
            OrderCompleted?.Invoke(order);
            Restock();
        }

        void Restock()
        {
            while (available.Count < boardSize)
            {
                Post();
            }
        }

        void Post()
        {
            var route = routes[nextRoute];
            nextRoute = (nextRoute + 1) % routes.Length;

            var order = new DeliveryOrder(nextId++, origin, route.destination, route.reward);
            available.Add(order);
            OrderAdded?.Invoke(order);
        }
    }
}
