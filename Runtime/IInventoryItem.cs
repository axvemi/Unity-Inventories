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
        /// </summary>
        /// <returns></returns>
        int GetMaxStackAmount();

        /// <summary>
        /// Condition to check whether to IInventoryItems are considered equal
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsSameItem(IInventoryItem item) => GetId().Equals(item.GetId());

        /// <summary>
        /// Whether it allows stacking an infinite amount in the same slot.
        /// </summary>
        /// <returns></returns>
        bool IsInfiniteStack() => GetMaxStackAmount() == 0;
    }
}