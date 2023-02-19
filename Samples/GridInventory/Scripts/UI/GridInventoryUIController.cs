using System.Collections.Generic;
using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Class that creates an slot gameObject for each slot in the inventory.
    /// </summary>
    public class GridInventoryUIController : MonoBehaviour
    {
        [SerializeField] private GameObject inventorySlotPrefab;

        [Header("UI")]
        [SerializeField] private Transform slotsContainer;

        private readonly Dictionary<InventorySlot<Item>, GameObject> _inventorySlotGameObjectDictionary = new();

        public Inventory<Item> Inventory { get; set; }

        private void Start()
        {
            Inventory.OnSlotAdded += OnSlotAdded;
            Inventory.OnSlotRemoved += OnSlotRemoved;
            ;

            //Create an slot for each inventory slot
            foreach (InventorySlot<Item> slot in Inventory.Slots)
            {
                CreateSlotGameObject(slot);
            }
        }

        private void OnSlotRemoved(InventorySlot<Item> slot)
        {
            Destroy(_inventorySlotGameObjectDictionary[slot]);
            _inventorySlotGameObjectDictionary.Remove(slot);
        }

        private void OnSlotAdded(InventorySlot<Item> slot)
        {
            CreateSlotGameObject(slot);
        }

        private void CreateSlotGameObject(InventorySlot<Item> slot)
        {
            GridInventorySlotUIController inventorySlotUIInstance = Instantiate(inventorySlotPrefab, slotsContainer).GetComponent<GridInventorySlotUIController>();
            inventorySlotUIInstance.Slot = slot;

            _inventorySlotGameObjectDictionary.Add(slot, inventorySlotUIInstance.gameObject);
        }
    }
}