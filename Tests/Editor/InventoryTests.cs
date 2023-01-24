using System;
using System.Collections.Generic;
using Axvemi.Inventories;
using Axvemi.Inventories.Samples;
using NUnit.Framework;
using UnityEngine;

public class InventoryTests
{
    private static Dictionary<string, ItemScriptableObject> itemsScriptableObjectDictionary = new();

    [OneTimeSetUp]
    public void SetUp()
    {
        itemsScriptableObjectDictionary = Constants.GenerateItemsScriptableObjectDictionary();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        itemsScriptableObjectDictionary.Clear();
    }
    
    [TestCase(0)]
    [TestCase(10)]
    [TestCase(30)]
    [Test]
    public void CreateWithSlotAmountTest(int slotSize)
    {
        Inventory<Item> inventory = new Inventory<Item>(slotSize);
        Assert.AreEqual(slotSize, inventory.Slots.Count);
    }
    
    [TestCase(1, Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 10)]
    [TestCase(2, Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID, 2)]
    [Test]
    public void AddItemTest(int slotSize, string scriptableObjectId, int amount)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        Inventory<Item> inventory = new Inventory<Item>(slotSize);
        inventory.AddItem(new Item(itemScriptableObject), amount);
        
        Assert.AreEqual(amount, inventory.GetAmountOfItem(new Item(itemScriptableObject)));
        
        Debug.Log(inventory.ToString());
    }
    
    [TestCase(0, Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 5)]
    [TestCase(1, Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 0)]
    [TestCase(1, Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, -1)]
    [TestCase(1, Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 11)]
    [TestCase(2, Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID, 3)]
    [Test]
    public void AddInvalidItemAmountTest(int slotSize, string scriptableObjectId, int amount)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        Inventory<Item> inventory = new Inventory<Item>(slotSize);
        try
        {
            inventory.AddItem(new Item(itemScriptableObject), amount);
            Assert.Fail();
        }
        catch (Exception ex) when (ex is ArgumentException or FailedToStoreException)
        {
            Assert.Pass();
        }
        
        Debug.Log(inventory.ToString());
    }
    
    [TestCase(1, Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 7)]
    [TestCase(3, Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 15)]
    [TestCase(2, Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID, 2)]
    [TestCase(3, Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID, 100)]
    [Test]
    public void CheckRemainingSpaceTest(int slotSize, string scriptableObjectId, int amount)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        Inventory<Item> inventory = new Inventory<Item>(slotSize);
        Item item = new Item(itemScriptableObject);
        inventory.AddItem(item, amount);
        
        if (((IInventoryItem)item).IsInfiniteStack())
        {
            Assert.Pass();
        }
        
        int expectedRemainingAmount = (slotSize * item.GetMaxStackAmount()) - amount;
        Debug.Log("Expected free space: " + expectedRemainingAmount);
        Debug.Log("Inventory free space: " + inventory.GetRemainingSpaceForItem(item));
        Assert.AreEqual(expectedRemainingAmount, inventory.GetRemainingSpaceForItem(item));
    }
    
}