using System;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Example class of an item representation
    /// </summary>
    public class Item : IInventoryItem
    {
        public ItemScriptableObject ItemScriptableObject { get; }

        public Item(ItemScriptableObject itemScriptableObject)
        {
            if (itemScriptableObject == null)
            {
                throw new ArgumentNullException();
            }

            ItemScriptableObject = itemScriptableObject;
        }

        public string GetId() => ItemScriptableObject.id;

        public int GetMaxStackAmount()
        {
            return ItemScriptableObject.maxAmount;
        }

        public override string ToString()
        {
            return "Id: " + ItemScriptableObject.id;
        }
    }
}