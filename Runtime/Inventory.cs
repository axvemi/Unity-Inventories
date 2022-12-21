using System;
using System.Collections.Generic;
using System.Linq;

namespace Axvemi.Inventories {

    public class FailedToFindSlotException : Exception { }
    /// <summary>
    /// Representation of an inventory.
    /// Composed by inventory slots, that can have T, where T : IInventoryItem<T>
    /// </summary>
    public class Inventory<T> where T : IInventoryItem<T>
    {
        private List<InventorySlot<T>> slots;
 
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
            slots = new List<InventorySlot<T>>();
        }

        /// <summary>
        /// Create a new inventory instance, with a starting slot size
        /// </summary>
        /// <param name="startingSlotSize"></param>
        public Inventory(int startingSlotSize) {
            slots = new List<InventorySlot<T>>();
            for (int i = 0; i < startingSlotSize; i++)
            {
                CreateNewSlot();
            }
        }

        public List<InventorySlot<T>> Slots => this.slots;

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
        /// <param name="slot"></param>
        /// <exception cref="Exception"></exception>
        public void RemoveSlot(InventorySlot<T> slot)
        {
            if(slot == null || !slots.Contains(slot))
            {
                throw new Exception("Can't remove that slot");
            }
            slot.Inventory = null;
            Slots.Remove(slot);
            OnSlotRemoved?.Invoke(slot);
        }

        /// <summary>
        /// Try to add an item to the inventory
        /// First add it to one where already has one of its type
        /// If it cannot or doesn't exists search for an empty one
        /// </summary>
        /// <param name="item">Item to add</param>
        public void AddItem(T item, int ammount = 1) {
            for (int i = 0; i < ammount; i++) {
                InventorySlot<T> slot = GetFreeInventorySlot(item);
                if(slot == null)
                {
                    throw new FailedToFindSlotException();
                }
                slot.StoreItem(item);
            }
        }

        /// <summary>
        /// Removes X amount items that have that identifier.
        /// Remvoes the firsts found
        /// </summary>
        /// <param name="itemId">Items with the id to remove</param>
        /// <param name="amount">Amount to remove</param>
        public void RemoveItem(string itemId, int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                try
                {
                    InventorySlot<T> slot = GetInventorySlotsWithItemId(itemId)[0];
                    slot.RemoveItem();
                }
                catch (IndexOutOfRangeException) {}
            }
        }

        /// <summary>
        /// Gets the first slot that has free space.
        /// First check if there is already one with this type
        /// If not, search for an empty one
        /// Else, return null
        /// </summary>
        /// <param name="item">Item to store</param>
        /// <returns>Inventory slot that meets the parameters. Null if none</returns>
        public InventorySlot<T> GetFreeInventorySlot(T item) {
            InventorySlot<T> slot = null;
            //Can stack unlimited ammount
            if (item.IsInfiniteStack()) {
                slot = Slots.Find(s => (s.Item != null) && (s.Item.IsSameItem(item)));
            }
            else {
                slot = Slots.Find(s => (s.Item != null) && (s.Item.IsSameItem(item)) && (s.Amount < item.GetMaxStackAmount()));
            }
            //Look for an empty one
            if(slot == null)
            {
                slot = Slots.Find(s => s.Item == null);
            }

            return slot;
        }

        /// <summary>
        /// Get all the slots that have an item type
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public List<InventorySlot<T>> GetInventorySlotsWithItemId(string itemId)
        {
            return this.Slots.Where(slot => slot.Item.GetId().Equals(itemId)).ToList();
        }
    }
}
