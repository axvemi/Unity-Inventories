using System.Collections.Generic;
using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Class that creates an slot gameObject for each slot in the inventory.
    /// </summary>
    public class GridInventoryUIController : MonoBehaviour
    {
        [SerializeField] private GameObject inventorySlotPrefab = null;
        [Header("UI")]
        [SerializeField] private Transform slotsContainer = null;

        private Dictionary<InventorySlot<Item>, GameObject> inventorySlotGameObjectDictionary = new Dictionary<InventorySlot<Item>, GameObject>();

        private Inventory<Item> inventory = null;

        public Inventory<Item> Inventory { get => inventory; set => inventory = value; }

        private void Start() {
            inventory.OnSlotAdded += OnSlotAdded;
            inventory.OnSlotRemoved += OnSlotRemoved; ;

            //Create an slot for each inventory slot
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                CreateSlotGameObject(inventory.Slots[i]);
            }
        }

        private void OnSlotRemoved(InventorySlot<Item> slot)
        {
            Destroy(inventorySlotGameObjectDictionary[slot]);
            inventorySlotGameObjectDictionary.Remove(slot);
        }

        private void OnSlotAdded(InventorySlot<Item> slot)
        {
            CreateSlotGameObject(slot);
        }

        private void CreateSlotGameObject(InventorySlot<Item> slot)
        {
            GridInventorySlotUIController inventorySlotUIInstance = Instantiate(inventorySlotPrefab, slotsContainer).GetComponent<GridInventorySlotUIController>();
            inventorySlotUIInstance.Slot = slot;

            inventorySlotGameObjectDictionary.Add(slot, inventorySlotUIInstance.gameObject);
        }
    }
}
