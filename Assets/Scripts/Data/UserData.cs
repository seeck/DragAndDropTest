using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public List<ListItemData> Songs { get; private set; }
    public List<ListItemData> Contacts { get; private set; }

    public UserData()
    {
        Songs = new List<ListItemData>();
        Contacts = new List<ListItemData>();
    }

    public void ParseSongs(string[] songs)
    {
        if (songs == null)
        {
            return;
        }
        foreach (string song in songs)
        {
            ListItemData data = new ListItemData(song, ItemDataType.Music);
            Songs.Add(data);
        }
    }

    public void ParseContacts(string[] contacts)
    {
        if (contacts == null)
        {
            return;
        }
        foreach (string contact in contacts)
        {
            ListItemData data = new ListItemData(contact, ItemDataType.Contacts);
            Contacts.Add(data);
        }
    }
}
