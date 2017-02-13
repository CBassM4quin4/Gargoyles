using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cDoor : cTile {

	public bool bLocked;

    public override bool InteractiveBlock(cPhysicalObject OTHER, List<enumBlockingType> TRAVERSIBLE_BLOCKS) {
        if (bLocked) {
            cPlayer PLAYER_OTHER = OTHER.GetComponent<cPlayer>();
            if (PLAYER_OTHER != null) {
                cItem KEY_RING_ITEM = PLAYER_OTHER.Inventory.FindItem<cKey>();
                if (KEY_RING_ITEM != null) {
                    cKey KEY_RING = KEY_RING_ITEM.GetComponent<cKey>();
                    if (KEY_RING.charges > 0) {
                        print("UNLOCKED");
                        KEY_RING.Activate();
                        Unlock();
                        return false;
                    }
                }
                print("The door is locked. You need a key!");
            }
        }
        // Default case is to block unless otherwise returned blocking as false
        return true;
    }
    private void Unlock() {
        bLocked = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
