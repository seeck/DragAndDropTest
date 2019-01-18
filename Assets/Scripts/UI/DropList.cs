using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Derived from DropZone
/// Added logic to easily add/remove items from the list.
/// </summary>
public class DropList : DropZone
{
    /// <summary>
    /// Removes all items from a list that satisfy a particular comparator
    /// </summary>
    /// <param name="condition">Condition evaluated for removal</param>
    public void RemoveItems(Func<ListItemData,bool> condition)
    {
        List<ListItem> removeThese = new List<ListItem>();
        foreach(Transform child in transform)
        {
            ListItem item = child.GetComponent<ListItem>();
            if(item!=null && condition(item.Data))
            {
                removeThese.Add(item);
            }
        }
        foreach(ListItem item in removeThese)
        {
            Destroy(item.gameObject);
        }
        removeThese.Clear();
    }
    /// <summary>
    /// Adds item to the end of the list.
    /// </summary>
    /// <param name="data">Data for the prefab</param>
    /// <param name="prefab">The prefab to be populated.</param>
    /// <returns></returns>
    public ListItem AddItem(ListItemData data, GameObject prefab)
    {
        GameObject instance = Instantiate(prefab);
        ListItem lI = instance.GetComponent<ListItem>();
        lI.Data = data;
        lI.transform.SetParent(transform);
        lI.transform.localScale = Vector3.one;
        lI.transform.SetAsLastSibling();
        lI.name = data.Name;
        return lI;
    }

    /// <summary>
    /// Add item at a given sibling index.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="prefab"></param>
    /// <param name="siblingIndex"></param>
    /// <returns></returns>
    public ListItem AddItemAtIndex(ListItemData data, GameObject prefab, int siblingIndex)
    {
        ListItem lI = AddItem(data, prefab);
        lI.transform.SetSiblingIndex(siblingIndex);
        return lI;
    }

    /// <summary>
    /// Check if the list contains a particular data Item.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Contains(ListItemData data)
    {
        if(data == null)
        {
            return false;
        }
        foreach(Transform child in transform)
        {
            ListItem listItem = child.GetComponent<ListItem>();
            if (listItem!= null && listItem.Data == data)
            {
                return true;
            }
        }
        return false;
    }

}
