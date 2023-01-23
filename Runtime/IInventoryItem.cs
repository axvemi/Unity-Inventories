namespace Axvemi.Inventories
{
    /// <summary>
    /// Interface that allows an object to be stored inside an inventory
    /// </summary>
    public interface IInventoryItem
    {
        /// <summary>
        /// Identifier of the item.
        /// Must be unique for each item type
        /// </summary>
        /// <returns></returns>
        string GetId();

        /// <summary>
        /// Max stack size per inventory slot
        /// Set 0 to allow an infinite amount
        /// </summary>
        /// <returns></returns>
        int GetMaxStackAmount();

        /// <summary>
        /// Compares the identifier of both items. If it's the same identifier, it's the same object
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsSameItem(IInventoryItem item) => this.GetId().Equals(item.GetId());

        /// <summary>
        /// Whether it allows stacking an infinite amount in the same slot.
        /// If the max size is 0, allow it
        /// </summary>
        /// <returns></returns>
        bool IsInfiniteStack() => GetMaxStackAmount() == 0;
    }
}