using System.Collections.Generic;
using System.IO;
using Axvemi.Inventories.Samples;
using UnityEditor;

public static class Constants
{
    public const string INFINITE_AMOUNT_SCRIPTABLE_OBJECT_ID = "infinite-amount";
    public const string SINGLE_AMOUNT_SCRIPTABLE_OBJECT_ID = "single-amount";
    public const string STACK_AMOUNT_SCRIPTABLE_OBJECT_ID = "stack-amount";
    
    private const string ITEMS_SO_TEST_PATH = "Assets/Unity-Inventories/Tests/Items";
    
    public static Dictionary<string, ItemScriptableObject> GenerateItemsScriptableObjectDictionary()
    {
        Dictionary<string, ItemScriptableObject> itemsScriptableObjectDictionary = new();
        itemsScriptableObjectDictionary.Clear();

        ItemScriptableObject infiniteAmountScriptableObject = GetItemScriptableObject("InfiniteAmountInventoryItem");
        ItemScriptableObject singleAmountScriptableObject = GetItemScriptableObject("SingleAmountInventoryItem");
        ItemScriptableObject stackAmountScriptableObject = GetItemScriptableObject("StackAmountInventoryItem");

        itemsScriptableObjectDictionary.Add(infiniteAmountScriptableObject.id, infiniteAmountScriptableObject);
        itemsScriptableObjectDictionary.Add(singleAmountScriptableObject.id, singleAmountScriptableObject);
        itemsScriptableObjectDictionary.Add(stackAmountScriptableObject.id, stackAmountScriptableObject);

        return itemsScriptableObjectDictionary;
    }
    
    public static ItemScriptableObject GetItemScriptableObject(string scriptableObjectName)
    {
        return (ItemScriptableObject)AssetDatabase.LoadAssetAtPath(Path.Combine(ITEMS_SO_TEST_PATH, scriptableObjectName + ".asset"), typeof(ItemScriptableObject));
    }
}
