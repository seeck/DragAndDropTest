using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropList : DropZone
{
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

    public ListItem AddItemAtIndex(ListItemData data, GameObject prefab, int siblingIndex)
    {
        ListItem lI = AddItem(data, prefab);
        lI.transform.SetSiblingIndex(siblingIndex);
        return lI;
    }

    public bool Contains(ListItemData data)
    {
        //Rethink how to do the contains operation.
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
