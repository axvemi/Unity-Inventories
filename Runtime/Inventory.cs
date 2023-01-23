using System;
using System.Collections.Generic;
using System.Linq;

namespace Axvemi.Inventories
{
    public class FailedToFindSlotException : Exception
    {
    }


    /// <summary>
    /// Representation of an inventory.
    /// </summary>
    public class Inventory<T> where T : IInventoryItem
    {
        /// <summary>
        /// Slots of the inventory
        /// </summary>
        public List<InventorySlot<T>> Slots { get; set; } = new();

        /// <summary>
        /// A new slot has been added to the inventory
        /// </summary>
        public event Action<InventorySlot<T>> OnSlotAdded;

        /// <summary>
        /// An slot has been removed from the inventory
        /// </summary>
        public event Action<InventorySlot<T>> OnSlotRemoved;

        public Inventory()
        {
        }

        public Inventory(int startingSlotSize)
        {
            for (int i = 0; i < startingSlotSize; i++)
            {
                CreateNewSlot();
            }
        }

        /// <summary>
        /// Create a new empty slot
        /// </summary>
        public void CreateNewSlot()
        {
            InventorySlot<T> slot = new InventorySlot<T>(this);
            Slots.Add(slot);
            OnSlotAdded?.Invoke(slot);
        }

        /// <summary>
        /// Remove the selected slot from the inventory
        /// </summary>
        /// <param name="slot">Slot to remove. Must belong to this dictionary</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void RemoveSlot(InventorySlot<T> slot)
        {
            if (slot == null)
            {
                throw new ArgumentNullException(nameof(slot), "Can not delete a null slot from an inventory!");
            }

            if (!Slots.Contains(slot))
            {
                throw new Exception("Can not remove a slot that doesn't belong to this inventory!");
            }

            slot.Inventory = null;
            Slots.Remove(slot);
            OnSlotRemoved?.Invoke(slot);
        }

        /// <summary>
        /// Try to add an item to the inventory
        /// First add it to one slot where it already has one of its type
        /// If it can not or doesn't exists search for an empty slot
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="amount">Amount to add</param>
        public void AddItem(T item, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                InventorySlot<T> slot = GetFreeInventorySlotForItemId(item);
                //TODO: Add only if we have enough space for everything. Not half of it and then exception

                slot.StoreItem(item);
            }
        }

        /// <summary>
        /// Removes X amount items that have that identifier.
        /// Removes the firsts found
        /// </summary>
        /// <param name="itemId">Items with the id to remove</param>
        /// <param name="amount">Amount to remove</param>
        public void RemoveItem(string itemId, int amount = 1)
        {
            if (GetAmountOfItem(itemId) < amount)
            {
                throw new Exception("Not enough amount");
            }

            for (int i = 0; i < amount; i++)
            {
                InventorySlot<T> slot = GetInventorySlotsWithItemId(itemId)[0];
                slot.RemoveItem();
            }
        }

        /// <summary>
        /// Look how many of an item we have in the inventory
        /// </summary>
        /// <param name="itemId">Id to look for</param>
        /// <returns>Amount of that item in the inventory</returns>
        public int GetAmountOfItem(string itemId)
        {
            List<InventorySlot<T>> slotsWithItemList = GetInventorySlotsWithItemId(itemId);
            return slotsWithItemList.Sum(inventorySlot => inventorySlot.Amount);
        }

        /// <summary>
        /// Gets the first slot that has free space.
        /// First check if there is already one with this type
        /// If not, search for an empty one
        /// Else, we failed
        /// </summary>
        /// <param name="item">Item to store</param>
        /// <returns>Inventory slot that meets the parameters. Null if none</returns>
        public InventorySlot<T> GetFreeInventorySlotForItemId(T item)
        {
            InventorySlot<T> slot;
            //Unlimited amount. Look for a slot that already contains this item
            if (item.IsInfiniteStack())
            {
                slot = Slots.Find(s => (s.Item != null) && (s.Item.IsSameItem(item)));
            }
            //Limited amount. Slot with item and free space
            else
            {
                slot = Slots.Find(s => (s.Item != null) && (s.Item.IsSameItem(item)) && (s.Amount < item.GetMaxStackAmount()));
            }

            //Still null, look for an empty slot
            slot ??= Slots.Find(s => s.Item == null);

            if (slot == null)
            {
                throw new FailedToFindSlotException();
            }
            
            return slot;
        }

        /// <summary>
        /// Get all the slots that have an item with the ID = "itemId"
        /// </summary>
        /// <param name="itemId">Id of the item to look for</param>
        /// <returns>All the slots that contain at least one item with "itemId". An empty list if none</returns>
        public List<InventorySlot<T>> GetInventorySlotsWithItemId(string itemId)
        {
            return this.Slots.Where(slot => slot.Item.GetId().Equals(itemId)).ToList();
        }
    }
}