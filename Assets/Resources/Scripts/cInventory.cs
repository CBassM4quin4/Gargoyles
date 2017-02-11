using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cInventory : cGamePiece {
    public int selected;
    public List<cItem> itemList;

    // Use this for initialization
    virtual protected void Start () {
        selected = 0;
    }

    virtual public cItem FindItem<T>() {
        T FoundItem;
        foreach (cItem item in itemList) {
            FoundItem = item.GetComponent<T>();
            if (FoundItem != null) { return item; }
        }
        return null;
    }
    virtual public void PickUp(cItem PICKUP) {
        if (PICKUP.eItemState != cItem.enumItemState.Pickup) { return; }
        foreach (cItem item in itemList) {
            if (item.tag == PICKUP.tag) {
                item.Stack(PICKUP);
                return;
            }
        }
        AddItem(PICKUP);
    }
    virtual protected void AddItem(cItem PICKUP) {
        PICKUP.FirstPickup();
        itemList.Add(PICKUP);
        if (itemList.Count == 1) { Select(0); }
    }

    virtual protected void Select(int QUICK_SELECT) {
        selected = QUICK_SELECT;
        if (selected == itemList.Count) { selected = 0; }
        if (selected < 0) { selected = itemList.Count - 1; }
    }
    virtual protected void SelectNext() {
        selected += 1;
        if (selected == itemList.Count) { selected = 0; }
    }
    virtual protected void SelectPrevious() {
        selected -= 1;
        if (selected < 0) { selected = itemList.Count-1; }
    }
}
