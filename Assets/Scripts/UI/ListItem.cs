using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Uses ListItemData to populate its UI.
/// Populated via the Manager (controller)
/// Derives from Draggable so that the dragging logic is kept separate from the data.
/// </summary>
public class ListItem : Draggable
{
    [SerializeField]
    Text displayName;

    ListItemData data;
    public ListItemData Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
            Refresh();
        }
    }

    public bool IsShortcut { get; set; }

    public void Refresh()
    {
        if(data == null)
        {
            displayName.text = "";
            return;
        }
        displayName.text = data.Name;
    }
}
