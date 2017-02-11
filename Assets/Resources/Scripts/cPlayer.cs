using UnityEngine;

public partial class cPlayer : cPhysicalObject
{
    public cInventory Inventory;

    protected override void Start() {
        base.Start();

        setMoveType(enumMoveType.Linear);
        Inventory = Instantiate(Inventory);
    }
    protected override void Update(){
        base.Update();

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0){
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0){
            if (!bIsMoving) {
                Vector2 LOOK_AHEAD = new Vector2(horizontal, vertical) * 0.32f;
                Collider2D[] DETECTEDOBJECTS = DetectObjects((Vector2)transform.position + LOOK_AHEAD, 0.01f);

                bool bMOVE_BLOCKED = false; // DEFAULT until blocked
                if (DETECTEDOBJECTS.Length == 0) {
                    //bMOVE_BLOCKED = true; 
                }
                else
                {
                    foreach (Collider2D OTHER in DETECTEDOBJECTS)
                    {
                        cTile TILE_OTHER = OTHER.GetComponent<cTile>();
                        if (TILE_OTHER != null)
                        {
                            cPhysicalObject TMP = GetComponent<cPhysicalObject>();
                            if (TMP != null)
                            bMOVE_BLOCKED = TILE_OTHER.CheckIfBlocked(TMP,traversibleTerrain);
                        }
                    }
                }
                if (!bMOVE_BLOCKED) { startMove(LOOK_AHEAD, 0.2f); }
            }
        }
    }
}

public partial class cPlayer {

    virtual protected void OnTriggerEnter2D(Collider2D OTHER) {
        cItem ITEM_OTHER = OTHER.GetComponent<cItem>();
        if (ITEM_OTHER != null) {
            Inventory.PickUp(ITEM_OTHER);
        }
    }
}