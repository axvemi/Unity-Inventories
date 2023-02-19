using System;
using System.Collections.Generic;
using Axvemi.Inventories;
using Axvemi.Inventories.Samples;
using NUnit.Framework;
using UnityEngine;

public class InventorySlotTests
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

    [TestCase(Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID)]
    [TestCase(Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID)]
    [TestCase(Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID)]
    [Test]
    public void SaveItemInSlotTest(string scriptableObjectId)
    {
        Item item = new Item(itemsScriptableObjectDictionary[scriptableObjectId]);
        InventorySlot<Item> slot = new InventorySlot<Item>();
        slot.StoreItem(item);
        Assert.AreEqual(scriptableObjectId, slot.Item.GetId());
    }

    [TestCase(Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID, 100)]
    [TestCase(Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID, 1)]
    [TestCase(Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 10)]
    [Test]
    public void SaveItemWithAmountTest(string scriptableObjectId, int amount)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        InventorySlot<Item> slot = new InventorySlot<Item>();
        try
        {
            slot.StoreItem(new Item(itemScriptableObject), amount);
        }
        catch (FailedToStoreException)
        {
            Assert.Fail("Should be able to store that item with that amount!");
        }
    }

    [TestCase(Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID, 2)]
    [TestCase(Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 11)]
    [Test]
    public void SaveItemWithAmountInvalidTest(string scriptableObjectId, int amount)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        InventorySlot<Item> slot = new InventorySlot<Item>();
        try
        {
            slot.StoreItem(new Item(itemScriptableObject), amount);
            Assert.Fail("Should be able to store that item with that amount!");
        }
        catch (Exception ex) when (ex is FailedToStoreException or ArgumentException)
        {
        }
    }

    [TestCase(Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID, 0)]
    [TestCase(Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID, 20)]
    [Test]
    public void StoreItemAndSetAmountTest(string scriptableObjectId, int amount)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        InventorySlot<Item> slot = new InventorySlot<Item>();
        slot.StoreItem(new Item(itemScriptableObject));
        try
        {
            slot.Amount = amount;
        }
        catch (Exception ex) when (ex is FailedToStoreException or ArgumentException)
        {
            Assert.Fail("Should be able to set that amount!");
        }
    }

    [TestCase(Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID, -1)]
    [TestCase(Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID, 2)]
    [TestCase(Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 11)]
    public void StoreItemAndSetAmountInvalid(string scriptableObjectId, int amount)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        InventorySlot<Item> slot = new InventorySlot<Item>();
        slot.StoreItem(new Item(itemScriptableObject));
        try
        {
            slot.Amount = amount;
            Assert.Fail("Should not be able to set that amount!");
        }
        catch (Exception ex) when (ex is FailedToStoreException or ArgumentException)
        {
        }
    }

    [TestCase(Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 1, 2)]
    [TestCase(Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 5, 7)]
    [TestCase(Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID, 1, 1)]
    [TestCase(Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID, 62, 128)]
    [Test]
    public void TransferSameContentTest(string scriptableObjectId, int amount, int amount2)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        InventorySlot<Item> slot01 = new InventorySlot<Item>();
        InventorySlot<Item> slot02 = new InventorySlot<Item>();

        Item item01 = new Item(itemScriptableObject);
        Item item02 = new Item(itemScriptableObject);

        IInventoryItem inventoryItem01 = item01;
        IInventoryItem inventoryItem02 = item01;

        slot01.StoreItem(item01, amount);
        slot02.StoreItem(item02, amount2);
        slot01.MoveBetweenSlots(slot02, slot01.Amount);

        //ASSERTS
        //Test slot01 contents
        if (inventoryItem01.IsInfiniteStack() || amount + amount2 <= inventoryItem01.GetMaxStackAmount())
        {
            Debug.Log("Expected item in slot-1: NULL" + "; Expected slot01 amount: " + 0);
            Assert.IsTrue(slot01.Amount == 0 && slot01.Item == null);
        }
        else
        {
            int expectedAmount = amount - (itemScriptableObject.maxAmount - amount2);
            Debug.Log("Expected item in slot-1: " + item01.GetId() + "; Expected slot01 amount: " + expectedAmount);
            Assert.IsTrue(slot01.Amount == expectedAmount && slot01.Item.GetId().Equals(item01.GetId()));
        }

        //Test slot02 contents
        if (inventoryItem02.IsInfiniteStack() || amount + amount2 <= inventoryItem01.GetMaxStackAmount())
        {
            Debug.Log("Expected item in slot-2: " + item02.GetId() + "; Expected slot01 amount: " + (amount + amount2));
            Assert.IsTrue(slot02.Amount == amount + amount2 && slot02.Item.GetId().Equals(item02.GetId()));
        }
        else
        {
            Debug.Log("Expected item in slot-2: " + item02.GetId() + "; Expected slot02 amount: " + inventoryItem02.GetMaxStackAmount());
            Assert.IsTrue(slot02.Amount == inventoryItem02.GetMaxStackAmount() && slot02.Item.GetId().Equals(item02.GetId()));
        }

        Debug.Log("---- RESULT: ----");
        Debug.Log("Content in slot 01: {" + slot01 + "}");
        Debug.Log("Content in slot 02: {" + slot02 + "}");
    }

    [TestCase(Constants.SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID, 1, Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID, 2)]
    [TestCase(Constants.INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID, 62, Constants.STACK_AMOUNT_SCRIPTABLE_OBJECT_ID, 6)]
    [Test]
    public void TransferToDifferentSlotContentTest(string scriptableObjectId, int amount, string scriptableObjectId2, int amount2)
    {
        ItemScriptableObject itemScriptableObject = itemsScriptableObjectDictionary[scriptableObjectId];
        ItemScriptableObject itemScriptableObject2 = itemsScriptableObjectDictionary[scriptableObjectId2];
        InventorySlot<Item> slot01 = new InventorySlot<Item>();
        InventorySlot<Item> slot02 = new InventorySlot<Item>();

        Item item01 = new Item(itemScriptableObject);
        Item item02 = new Item(itemScriptableObject2);

        slot01.StoreItem(item01, amount);
        slot02.StoreItem(item02, amount2);
        slot01.MoveBetweenSlots(slot02, slot01.Amount);
        
        //ASSERTS
        Debug.Log("Expected item in slot-1: " + item02.GetId() + "; Expected slot01 amount: " + amount2);
        Assert.IsTrue(slot01.Amount == amount2 && slot01.Item.GetId().Equals(item02.GetId()));
        
        Debug.Log("Expected item in slot-2: " + item02 + "; Expected slot01 amount: " + amount2);
        Assert.IsTrue(slot02.Amount == amount && slot02.Item.GetId().Equals(item01.GetId()));

        Debug.Log("---- RESULT: ----");
        Debug.Log("Content in slot 01: {" + slot01 + "}");
        Debug.Log("Content in slot 02: {" + slot02 + "}");
    }
    
    [Test]
    public void CannotSetAmountToNullItemTest()
    {
        InventorySlot<Item> slot = new InventorySlot<Item>();
        try
        {
            slot.Amount += 10;
            Assert.Fail("Should not be able to change amount if the item is null");
        }
        catch (FailedToStoreException)
        {
        }
    }
}