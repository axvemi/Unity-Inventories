using System;
using System.Collections.Generic;
using System.Linq;

namespace Axvemi.Inventories
{
    /// <summary>
    /// Representation of an inventory.
    /// </summary>
    public class Inventory<T> where T : IInventoryItem
    {
        /// <summary>
        /// Slots of the inventory
        /// </summary>
        public List<InventorySlot<T>> Slots { get; } = new();

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
            if (startingSlotSize < 0)
            {
                throw new ArgumentException(nameof(startingSlotSize) + " can not be less than 0!");
            }

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
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Amount can not be less or equal to 0!");
            }

            if (GetRemainingSpaceForItem(item) < amount)
            {
                throw new FailedToStoreException("There is not enough space in the inventory to store that amount");
            }

            for (int i = 0; i < amount; i++)
            {
                InventorySlot<T> slot = FindFirstFreeInventorySlotForItem(item);
                slot.StoreItem(item);
            }
        }

        /// <summary>
        /// Removes X amount items that have that identifier.
        /// Removes the firsts found
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <param name="amount">Amount to remove</param>
        public void RemoveItem(T item, int amount = 1)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Amount can not be less or equal to 0!");
            }

            if (GetAmountOfItem(item) < amount)
            {
                throw new Exception("Not enough amount");
            }

            for (int i = 0; i < amount; i++)
            {
                InventorySlot<T> slot = FindInventorySlotListWithItem(item)[0];
                slot.RemoveItem();
            }
        }

        public void Clear()
        {
            foreach (InventorySlot<T> slot in Slots)
            {
                slot.Inventory = null;
            }

            Slots.Clear();
        }

        public override string ToString()
        {
            string slotListContent = "";
            for (int i = 0; i < Slots.Count; i++)
            {
                slotListContent += "Slot " + i + ": {" + Slots[i] + "}" + (i == Slots.Count - 1 ? "" : Environment.NewLine);
            }

            return "Slots: {" + slotListContent + "}";
        }

        //QUERY

        /// <summary>
        /// Gets the first slot that has free space.
        /// First check if there is already one with this type
        /// If not, search for an empty one
        /// Else, we failed
        /// </summary>
        /// <param name="item">Item to store</param>
        /// <returns>Inventory slot that meets the parameters. Null if none</returns>
        public InventorySlot<T> FindFirstFreeInventorySlotForItem(T item)
        {
            InventorySlot<T> slot;
            //Unlimited amount. Look for a slot that already contains this item
            if (item.IsInfiniteStack())
            {
                slot = FindSlotWithItem(item);
            }
            //Limited amount. Slot with item and free space
            else
            {
                slot = FindSlotWithItemAndFreeSpace(item);
            }

            //Still null, look for an empty slot
            slot ??= FindEmptySlot();

            if (slot == null)
            {
                throw new FailedToFindSlotException();
            }

            return slot;
        }

        public InventorySlot<T> FindEmptySlot()
        {
            return Slots.Find(s => s.Item == null);
        }

        public InventorySlot<T> FindSlotWithItem(T item)
        {
            return Slots.Find(s => (s.Item != null) && (s.Item.IsSameItem(item)));
        }

        public InventorySlot<T> FindSlotWithItemAndFreeSpace(T item)
        {
            return Slots.Find(s => (s.Item != null) && (s.Item.IsSameItem(item)) && (s.Item.IsInfiniteStack() || s.Amount < item.GetMaxStackAmount()));
        }

        public List<InventorySlot<T>> FindEmptySlotList()
        {
            return this.Slots.Where(slot => slot.Item == null).ToList();
        }

        public List<InventorySlot<T>> FindInventorySlotListWithItem(T item)
        {
            return this.Slots.Where(slot => (slot.Item != null) && (slot.Item.IsSameItem(item))).ToList();
        }

        public int GetRemainingSpaceForItem(T item)
        {
            if (item.IsInfiniteStack()) return int.MaxValue;

            List<InventorySlot<T>> emptySlots = FindEmptySlotList();
            List<InventorySlot<T>> slotsWithItem = FindInventorySlotListWithItem(item);

            int remainingSpace = 0;
            remainingSpace += emptySlots.Count * item.GetMaxStackAmount();
            remainingSpace += slotsWithItem.Sum(slot => item.GetMaxStackAmount() - slot.Amount);

            return remainingSpace;
        }

        public int GetAmountOfItem(T item)
        {
            List<InventorySlot<T>> slotsWithItemList = FindInventorySlotListWithItem(item);
            return slotsWithItemList.Sum(inventorySlot => inventorySlot.Amount);
        }
    }
}