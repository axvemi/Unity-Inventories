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
            this.ItemScriptableObject = itemScriptableObject;
        }

        public string GetId() => this.ItemScriptableObject.Id;

        public int GetMaxStackAmount()
        {
            return ItemScriptableObject.MaxAmount;
        }
    }
}