using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItemData {
    public string Name { get; private set; }
    public ItemDataType DataType { get; private set; }

    public ListItemData(string name, ItemDataType dataType)
    {
        Name = name;
        DataType = dataType;
    }
}

public enum ItemDataType {
    Music,
    Contacts
}
