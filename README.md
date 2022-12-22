# Inventory System for Unity

This package provides an inventory system for Unity. The base it's purely on C#, and it's generic so it could be used elsewhere. The Samples are for Unity.

## How to Install

Follow the Unity guide in how to install a git package: https://docs.unity3d.com/Manual/upm-ui-giturl.html

## Documentation

### IInventoryItem Interface

This interface allows that class to be one that the inventory will recognize.

```
public class Item : IInventoryItem<Item> {}
```

You have to implement the following methods:

```
//This method defines wether an item it's the same as another, allowing to share an slot.
string GetId();

//Sets the maximum stack size allowed. If it is 0, there will be no limit on how much a slot can store.
int GetMaxStackAmount();
```

### Inventory

You can create a new inventory instance with the following constructors:

```
//Creates an empty base inventory
Inventory<T> inventory = new Inventory<T>();

//Creates an inventory with a starting slot size
Inventory<T> inventory = new Inventory<T>(startingSlotSize);

```

And you can control it with the following methods:

```
//Creates a new empty slot in the inventory
public void CreateNewSlot();

//Removes the slot parameter from the inventory
public void CreateNewSlot(InventorySlot<T> slot);

//Adds the item instance to the inventory, and that amount
public void AddItem(T item, int amount = 1);

//Removes the first item found in the inventory with that id. Removes that amount. If the amount is bigger than the items amount, it will remove only the existing amount
public void RemoveItem(string itemId, int amount = 1);
```

### InventorySlot

An inventory contains InventorySlots, that contain an item.
You can create an invididual InventorySlot that doesn't belong to an inventory. For example, one inventory slot for the cursor.

```
private InventorySlot<T> mouseInventorySlot = new InventorySlot<T>(null);
```

You can control the inventory slot with the following methods

```
// If they are the same object, move all the possible amount from slot1 to slot2.
// If they are two different slots, swap the contents
public static void MoveBetweenSlots(InventorySlot<T> originSlot, InventorySlot<T> targetSlot, int tranferAmount);

public void MoveBetweenSlots(InventorySlot<T> targetSlot, int tranferAmount);

// Adds the item to this slot
// If the item is null set reset slot
public void StoreItem(T item, int ammount = 1);

// Removes "amount" amount of items in this slot.
// If the result it's 0, set T as default
public void RemoveItem(int amount = 1);
```

## Samples

The package provides a sample about how to make a grid inventory, one like the ones in Minecraft or Terraria. If you are going to import this sample, be sure to also import the Commons sample.

The Sample provides a Scene and the necesary script, with a debug menu on the right that allows the user to add new slots to the inventory, remove them, or add items.

If you click on an item, it will be moved to the mouse, if you click again, it will be swapped with whatever is on the target slot.
You can also hold the left-shift key before clicking, this will show an emergent window where you can choose how many items to transfer.

![Grid Inventory Demo Scene](./Documentation/gridInventoryDemo.png)