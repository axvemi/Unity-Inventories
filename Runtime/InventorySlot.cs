using System;

namespace Axvemi.Inventories {

    /// <summary>
    /// Exception thrown when its not possible to store an item in the slot
    /// </summary>
    public class FailedToStoreItemException : Exception { }

    public class FailedToMoveItemException : Exception { }

    /// <summary>
    /// Slot of the inventory. Contains a T, or nothing (null)
    /// </summary>
    public class InventorySlot<T> where T : IInventoryItem {

        private Inventory<T> inventory;
        private T item;
        private int amount;

        /// <summary>
        /// Called when the item type, or the amount have been modified
        /// </summary>
        public event Action<InventorySlot<T>> OnSlotUpdated;

        public InventorySlot() { }

        public InventorySlot(Inventory<T> inventory)
        {
            this.inventory = inventory;
        }

        public Inventory<T> Inventory
        {
            get => this.inventory;
            set => this.inventory = value;
        }
        public T Item
        {
            get => this.item;
            set
            {
                this.item = value;
                OnSlotUpdated?.Invoke(this);
            }
        }
        public int Amount
        {
            get => this.amount;
            set
            {
                this.amount = value;
                OnSlotUpdated?.Invoke(this);
            }
        }

        /// <summary>
        /// If they are the same object, move all the possible amount from slot1 to slot2.
        /// If they are two different slots, swap the contents
        /// </summary>
        /// <param name="originSlot">Origin slot</param>
        /// <param name="targetSlot">Target slot</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void MoveBetweenSlots(InventorySlot<T> originSlot, InventorySlot<T> targetSlot, int tranferAmount)
        {
            if(originSlot == null || targetSlot == null)
            {
                throw new ArgumentNullException("Neither slot1 or slot2 can be null");
            }

            if(tranferAmount > originSlot.amount)
            {
                throw new ArgumentNullException("You can't transfer an amount bigger than the one existing in the origin slot");
            }

            //Move all possible amount from origin to target
            if(targetSlot.item == null || originSlot.item.IsSameItem(targetSlot.item))
            {
                //While there is remaining content in the origin, and remaining (or infinite) space on target
                while (tranferAmount > 0 && (targetSlot.amount < originSlot.item.GetMaxStackAmount() || originSlot.item.IsInfiniteStack()))
                {
                    targetSlot.StoreItem(originSlot.item);
                    originSlot.RemoveItem();
                    tranferAmount--;
                }
            }
            //Swap slots
            else if(tranferAmount == originSlot.amount)
            {
                (originSlot.amount, targetSlot.amount) = (targetSlot.amount, originSlot.amount);
                (originSlot.item, targetSlot.item) = (targetSlot.item, originSlot.item);
            }
            else
            {
                throw new FailedToMoveItemException();
            }

            originSlot.OnSlotUpdated?.Invoke(originSlot);
            targetSlot.OnSlotUpdated?.Invoke(targetSlot);
        }

        /// <summary>
        /// If they are the same object, move all the possible amount from slot1 to slot2.
        /// If they are two different slots, swap the contents
        /// </summary>
        public void MoveBetweenSlots(InventorySlot<T> targetSlot, int tranferAmount)
        {
            MoveBetweenSlots(this, targetSlot, tranferAmount);
        }

        /// <summary>
        /// Adds the item to this slot
        /// If the item is null set reset slot
        /// </summary>
        /// <exception cref="FailedToStoreItemException">If its not the correct Item to add this exception gets raised</exception>
        public void StoreItem(T item, int ammount = 1) {
            //If the item is not the same, or item is not null (reset) throw exception
            //If item is null, the slot will set it as null, and set the ammount to 0
            if (this.Item != null && item != null && !this.item.IsSameItem(item))
            {
                throw new FailedToStoreItemException();
            }

            this.item = item;
            this.amount = item == null ? 0 : this.Amount + ammount;
            OnSlotUpdated?.Invoke(this);
        }

        /// <summary>
        /// Removes "amount" amount of items in this slot.
        /// </summary>
        /// <param name="amount">Amount to remove. Can't be larget than the current slot amount</param>
        public void RemoveItem(int amount = 1)
        {
            if(amount <= 0 || amount > this.amount)
            {
                throw new ArgumentException("Amount must be larger than 0, and less or equal to the current amount stored");
            }

            this.amount -= amount;
            if (this.amount == 0)
            {
                this.item = default;
            }
            OnSlotUpdated?.Invoke(this);
        }
    }
}
