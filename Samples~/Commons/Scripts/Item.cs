namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Example class of an item representation
    /// </summary>
    public class Item : IInventoryItem<Item>
    {
        private ItemScriptableObject itemScriptableObject;

        public Item(ItemScriptableObject itemScriptableObject) 
        {
            this.itemScriptableObject = itemScriptableObject;
        }

        public ItemScriptableObject ItemScriptableObject { get => itemScriptableObject; set => itemScriptableObject = value; }

        public string GetId() => this.itemScriptableObject.Id;

        public int GetMaxStackAmount()
        {
            return itemScriptableObject.MaxAmount;
        }
    }
}

