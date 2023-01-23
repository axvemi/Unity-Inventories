using Axvemi.Inventories;
using Axvemi.Inventories.Samples;
using NUnit.Framework;

public class InventoryTests
{
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(10, ExpectedResult = 10)]
    [TestCase(30, ExpectedResult = 30)]
    [Test]
    public int CreateInventoryWithSlotsTest(int slotSize)
    {
        Inventory<Item> inventory = new Inventory<Item>(slotSize);
        return inventory.Slots.Count;
    }
    
    
}