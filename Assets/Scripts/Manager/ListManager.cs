using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The manager class that populates all the list and manages their behavior.
/// </summary>
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
        //Set up the model
        userData = new UserData();
        userData.ParseContacts(contactData);
        userData.ParseSongs(songData);

        //Populate the lists.
        PopulateList(music, userData.Songs, listItemPrefab, null);
        PopulateList(contacts, userData.Contacts, listItemPrefab, OnSongDroppedOnContact);

        //Set up the delegates.
        music.OnItemDropped += IgnoreDrop;
        contacts.OnItemDropped += IgnoreDrop;
        shortcuts.OnItemDropped += DroppedOnShortcut;
        playlists.OnItemDropped += DroppedOnPlaylist;
        trash.OnItemDropped += DroppedOnTrash;
    }

    //Pass user data into the list and instantiate UI elements.
    void PopulateList(DropList targetList, List<ListItemData> data, GameObject prefab, Action<Draggable,Draggable> OnDraggableDropped)
    {
        foreach(ListItemData datum in data)
        {
            ListItem lI = targetList.AddItem(datum, prefab);
            lI.OnDraggableDropped += OnDraggableDropped;
            lI.IsShortcut = false;
        }
    }
    
    //For ignoring items dropped on music/contacts.
    void IgnoreDrop(DropZone dropZone, Draggable draggable)
    {
        if(draggable!=null)
        {
            draggable.parentToReturnTo = draggable.originalParent;
        }
    }

    //Shortcut creates a copy of the dropped item, then returns it to the original location.
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

    //Playlist only allows songs to be added. Duplicates are allowed.
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

    //Trash deletes one reference or all references depending on the item.
    void DroppedOnTrash(DropZone dropZone, Draggable draggable)
    {
        ListItem listItem = draggable as ListItem;
        ListItemData listItemData = listItem.Data;
        if(!listItem.IsShortcut)
        {
            Func<ListItemData, bool> condition = (data) => { return data == listItemData; };
            shortcuts.RemoveItems(condition);
            if (listItem.Data.DataType == ItemDataType.Music)
            {
                music.RemoveItems(condition);
                playlists.RemoveItems(condition);
            }
            if (listItem.Data.DataType == ItemDataType.Contacts)
            {
                contacts.RemoveItems(condition);
            }
        }
        Destroy(listItem.gameObject);
    }

    //BONUS condition: Dropping song on contact.
    void OnSongDroppedOnContact(Draggable song, Draggable contact)
    {
        ListItem songItem = song as ListItem;
        ListItem contactItem = contact as ListItem;
        if (songItem != null && contactItem != null)
        {
            Debug.Log("Shared song " + songItem.Data.Name + " with contact " + contactItem.Data.Name);
        }
    } 
}
