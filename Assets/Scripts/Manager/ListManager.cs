using System;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour {
    [SerializeField]
    GameObject listItemPrefab;

    [SerializeField]
    DropList music;
    [SerializeField]
    DropList shortcuts;
    [SerializeField]
    DropList playlists;
    [SerializeField]
    DropList trash;
    [SerializeField]
    DropList contacts;

    string[] songData = { "Song 1", "Song 2","Song 3", "Song 4", "Song 5", "Song 6", "Song 7", "Song 8" };
    string[] contactData = { "Contact 1", "Contact 2", "Contact 3", "Contact 4", "Contact 5", "Contact 6", "Contact 7", "Contact 8" };

    UserData userData;

   
    void Awake()
    {
        userData = new UserData();
        userData.ParseContacts(contactData);
        userData.ParseSongs(songData);

        ParseData(music, userData.Songs, listItemPrefab, null);
        ParseData(contacts, userData.Contacts, listItemPrefab, OnSongDroppedOnContact);

        music.OnItemDropped += IgnoreDrop;
        contacts.OnItemDropped += IgnoreDrop;
        shortcuts.OnItemDropped += DroppedOnShortcut;
        playlists.OnItemDropped += DroppedOnPlaylist;
        trash.OnItemDropped += DroppedOnTrash;
    }

    void ParseData(DropList targetList, List<ListItemData> data, GameObject prefab, Action<Draggable,Draggable> OnDraggableDropped)
    {
        foreach(ListItemData datum in data)
        {
            ListItem lI = targetList.AddItem(datum, prefab);
            lI.OnDraggableDropped += OnDraggableDropped;
            lI.IsShortcut = false;
        }
    }
    

    void IgnoreDrop(DropZone dropZone, Draggable draggable)
    {
        if(draggable!=null)
        {
            draggable.parentToReturnTo = draggable.originalParent;
        }
    }

    void DroppedOnShortcut(DropZone dropZone, Draggable draggable)
    {
        DropList dropList = dropZone as DropList;
        ListItem listItem = draggable as ListItem;
        if (listItem == null || listItem.originalParent == dropZone.transform)
        {
            return;
        }
        if (!dropList.Contains(listItem.Data))
        {
            ListItem shortcut = dropList.AddItemAtIndex(
                listItem.Data, 
                listItemPrefab, 
                listItem.newSiblingIndex
            );
            shortcut.IsShortcut = true;
        }
        draggable.parentToReturnTo = draggable.originalParent;
        draggable.newSiblingIndex = draggable.originalSiblingIndex;
    }

    void DroppedOnPlaylist(DropZone dropZone, Draggable draggable)
    {
        DropList dropList = dropZone as DropList;
        ListItem listItem = draggable as ListItem;
        if(listItem == null || listItem.originalParent == dropZone.transform)
        {
            return;
        }
        if (listItem.Data.DataType == ItemDataType.Music)
        {
            ListItem shortcut = dropList.AddItemAtIndex(
                listItem.Data, 
                listItemPrefab, 
                listItem.newSiblingIndex
            );
            shortcut.IsShortcut = true;
        }
        draggable.parentToReturnTo = draggable.originalParent;
        draggable.newSiblingIndex = draggable.originalSiblingIndex;
    }

    void DroppedOnTrash(DropZone dropZone, Draggable draggable)
    {
        ListItem listItem = draggable as ListItem;
        ListItemData listItemData = listItem.Data;
        if(!listItem.IsShortcut)
        {
            shortcuts.RemoveItems((data) => { return data == listItemData; });
            music.RemoveItems((data) => { return data == listItemData; });
            playlists.RemoveItems((data) => { return data == listItemData; });
            contacts.RemoveItems((data) => { return data == listItemData; });
        }
        Destroy(listItem.gameObject);
    }

    void OnSongDroppedOnContact(Draggable song, Draggable contact)
    {
        ListItem songItem = song as ListItem;
        ListItem contactItem = contact as ListItem;
        if (songItem != null && contactItem != null)
        {
            Debug.Log("Dropped " + songItem.Data.Name + " on " + contactItem.Data.Name);
        }
    } 
}
