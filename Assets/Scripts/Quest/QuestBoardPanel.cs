using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LanternCourier.Quest
{
    public sealed class QuestBoardPanel : MonoBehaviour
    {
        [SerializeField] QuestBoard board;
        [SerializeField] InputActionReference toggle;
        [SerializeField] GameObject window;
        [SerializeField] RectTransform rowParent;
        [SerializeField] GameObject rowPrefab;
        [SerializeField] TMP_Text activeLabel;

        readonly Dictionary<DeliveryOrder, GameObject> rows = new();

        public int RowCount => rows.Count;

        void Awake()
        {
            board.OrderAdded += AddRow;
            board.OrderRemoved += RemoveRow;
            board.OrderAccepted += ShowActive;
            board.OrderCompleted += OnOrderCompleted;

            ShowActive(board.Active);
            SetOpen(window.activeSelf);
        }

        void OnDestroy()
        {
            board.OrderAdded -= AddRow;
            board.OrderRemoved -= RemoveRow;
            board.OrderAccepted -= ShowActive;
            board.OrderCompleted -= OnOrderCompleted;
        }

        void OnEnable()
        {
            toggle.action.Enable();
            toggle.action.performed += OnTogglePerformed;
        }

        void OnDisable()
        {
            toggle.action.performed -= OnTogglePerformed;
        }

        void OnTogglePerformed(InputAction.CallbackContext context)
        {
            SetOpen(!window.activeSelf);
        }

        public void SetOpen(bool open)
        {
            window.SetActive(open);

            if (open)
            {
                RebuildRows();
            }
        }

        void RebuildRows()
        {
            foreach (var row in rows.Values)
            {
                Destroy(row);
            }

            rows.Clear();

            foreach (var order in board.Available)
            {
                AddRow(order);
            }
        }

        void AddRow(DeliveryOrder order)
        {
            var row = Instantiate(rowPrefab, rowParent);
            row.GetComponentInChildren<TMP_Text>().text =
                $"#{order.Id}  {order.Origin} > {order.Destination}   {order.Reward}c";
            row.GetComponentInChildren<Button>().onClick.AddListener(() => Accept(order));
            rows[order] = row;
        }

        void RemoveRow(DeliveryOrder order)
        {
            if (rows.Remove(order, out var row))
            {
                Destroy(row);
            }
        }

        void Accept(DeliveryOrder order)
        {
            if (!board.Accept(order))
            {
                activeLabel.text = "Finish the current delivery first";
            }
        }

        void OnOrderCompleted(DeliveryOrder order)
        {
            ShowActive(null);
        }

        void ShowActive(DeliveryOrder order)
        {
            activeLabel.text = order == null
                ? "No active delivery"
                : $"Deliver to {order.Destination} for {order.Reward}c";
        }
    }
}
