using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem : cPhysicalObject {
    public int charges;
    public Sprite pickupIcon;
    public cPlayer owner;

    public enum enumItemState {
        Pickup,
        Inactive,
        Active
    }
    public enumItemState eItemState;
    
	// Use this for initialization
	virtual protected void Start () {
        base.Start();
    }
    virtual public void FirstPickup() {
        eItemState = enumItemState.Inactive;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    virtual public void Stack(cItem PICKUP) {
        charges += PICKUP.charges;
        PICKUP.eItemState = enumItemState.Inactive;
        PICKUP.GetComponent<SpriteRenderer>().enabled = false;
    }

    virtual public void Select() {
    }
    virtual public void Activate() {
        charges--;
        print("Activating item, charges remaining: "+charges);
        eItemState = enumItemState.Active;
    }
    virtual public void Deactivate()
    {
        eItemState = enumItemState.Inactive;
    }
}
