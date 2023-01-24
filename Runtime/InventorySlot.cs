using System;

namespace Axvemi.Inventories
{
    /// <summary>
    /// Slot of the inventory. Contains a T, or nothing (null)
    /// </summary>
    public class InventorySlot<T> where T : IInventoryItem
    {
        public Inventory<T> Inventory { get; set; }
        public T Item { get; private set; }
        private int amount;

        /// <summary>
        /// Called when the item type, or the amount have been modified
        /// </summary>
        public event Action<InventorySlot<T>> OnSlotUpdated;

        public InventorySlot()
        {
        }

        public InventorySlot(Inventory<T> inventory)
        {
            this.Inventory = inventory;
        }

        public int Amount
        {
            get => this.amount;
            set
            {
                if (Item == null)
                {
                    throw new FailedToStoreException("Can not set an amount when the item stored is null");
                }

                if (value < 0)
                {
                    throw new ArgumentException("The amount can not be less than 0");
                }

                if (!Item.IsInfiniteStack() && value > Item.GetMaxStackAmount())
                {
                    throw new FailedToStoreException("Trying to store an amount bigger than the stack size");
                }

                this.amount = value;

                if (amount == 0)
                {
                    Clear();
                    return;
                }

                OnSlotUpdated?.Invoke(this);
            }
        }
        
        /// <summary>
        /// If they are the same object, move all the possible amount from "originSlot" to "targetSlot".
        /// If they are two different slots, swap the contents. Transfer amount will not be used
        /// </summary>
        /// <param name="originSlot">Origin slot</param>
        /// <param name="targetSlot">Target slot</param>
        /// <param name="transferAmount">Amount to transfer</param>
        public static void MoveBetweenSlots(InventorySlot<T> originSlot, InventorySlot<T> targetSlot, int transferAmount = 1)
        {
            if (originSlot == null)
            {
                throw new ArgumentNullException(nameof(originSlot), "Can't transfer from a null slot");
            }

            if (targetSlot == null)
            {
                throw new ArgumentNullException(nameof(targetSlot), "Can't transfer to a null slot");
            }

            if (transferAmount > originSlot.amount)
            {
                throw new ArgumentException("You can't transfer an amount bigger than the one existing in the origin slot");
            }

            //Move all POSSIBLE amount from origin to target
            if (targetSlot.Item == null || originSlot.Item.IsSameItem(targetSlot.Item))
            {
                //While there is remaining content in the origin, and remaining (or infinite) space on target
                while (transferAmount > 0 && (targetSlot.amount < originSlot.Item.GetMaxStackAmount() || originSlot.Item.IsInfiniteStack()))
                {
                    targetSlot.StoreItem(originSlot.Item);
                    originSlot.RemoveItem();
                    transferAmount--;
                }
            }
            //Swap slots
            else
            {
                (originSlot.amount, targetSlot.amount) = (targetSlot.amount, originSlot.amount);
                (originSlot.Item, targetSlot.Item) = (targetSlot.Item, originSlot.Item);
            }

            originSlot.OnSlotUpdated?.Invoke(originSlot);
            targetSlot.OnSlotUpdated?.Invoke(targetSlot);
        }

        /// <summary>
        /// If they are the same object, move all the possible amount from "this slot" to "targetSlot".
        /// If they are two different slots, swap the contents. Transfer amount will not be used
        /// </summary>
        /// <param name="targetSlot">Target slot</param>
        /// <param name="transferAmount">Amount to transfer</param>
        public void MoveBetweenSlots(InventorySlot<T> targetSlot, int transferAmount = 1)
        {
            MoveBetweenSlots(this, targetSlot, transferAmount);
        }

        /// <summary>
        /// Adds the item to this slot
        /// If the item is null set reset slot
        /// </summary>
        /// <exception cref="FailedToStoreException">If its not the correct Item to add this exception gets raised</exception>
        public void StoreItem(T item, int amount = 1)
        {
            if (item == null)
            {
                throw new ArgumentException("Can't store a null item!");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Amount can't be less or equal to 0!");
            }

            if (amount + this.Amount > item.GetMaxStackAmount() && !item.IsInfiniteStack())
            {
                throw new ArgumentException("Can't store an amount bigger than the stack size!");
            }

            if (this.Item != null && !this.Item.IsSameItem(item))
            {
                throw new FailedToStoreException("You are trying to store a different item on an occupied slot!");
            }

            this.Item = item;
            this.Amount += amount;
            OnSlotUpdated?.Invoke(this);
        }

        /// <summary>
        /// Removes "amount" amount of items in this slot.
        /// </summary>
        /// <param name="amount">Amount to remove. Can't be larger than the current slot amount</param>
        public void RemoveItem(int amount = 1)
        {
            if (amount <= 0 || amount > this.amount)
            {
                throw new ArgumentException("Amount must be larger than 0, and less or equal to the current amount stored");
            }

            this.amount -= amount;
            if (this.amount == 0)
            {
                Clear();
                return;
            }

            OnSlotUpdated?.Invoke(this);
        }

        /// <summary>
        /// Clears the content of the slot
        /// </summary>
        public void Clear()
        {
            this.Item = default;
            this.amount = 0;
            OnSlotUpdated?.Invoke(this);
        }

        public override string ToString()
        {
            return $"Item: {Item}; Amount: {Amount}";
        }
    }
}