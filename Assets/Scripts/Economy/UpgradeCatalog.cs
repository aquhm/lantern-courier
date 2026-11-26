using System;
using System.Collections.Generic;
using UnityEngine;

namespace LanternCourier.Economy
{
    [CreateAssetMenu(fileName = "UpgradeCatalog", menuName = "Lantern Courier/Upgrade Catalog")]
    public sealed class UpgradeCatalog : ScriptableObject
    {
        public const string FuelTank = "fuel_tank";

        [Serializable]
        public struct Entry
        {
            public string id;
            public int tier;
            public string displayName;
            public int price;
            public float capacity;
        }

        [SerializeField] List<Entry> entries = new();

        Dictionary<(string, int), Entry> lookup;

        public IReadOnlyList<Entry> Entries => entries;

        public bool TryGet(string id, int tier, out Entry entry)
        {
            lookup ??= BuildLookup();
            return lookup.TryGetValue((id, tier), out entry);
        }

        public Entry Get(string id, int tier)
        {
            lookup ??= BuildLookup();
            return lookup[(id, tier)];
        }

        Dictionary<(string, int), Entry> BuildLookup()
        {
            var map = new Dictionary<(string, int), Entry>(entries.Count);
            foreach (var entry in entries)
            {
                map[(entry.id, entry.tier)] = entry;
            }

            return map;
        }

        void OnEnable()
        {
            lookup = null;
        }

        void OnValidate()
        {
            lookup = null;
        }
    }
}
