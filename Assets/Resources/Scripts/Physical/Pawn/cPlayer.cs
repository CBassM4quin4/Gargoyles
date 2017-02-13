using UnityEngine;
using System.Collections;

public partial class cPlayer : cPhysicalObject
{
    public cInventory Inventory;

    protected override void Start() {
        base.Start();

        setMoveType(enumMoveType.Linear);
        Inventory = Instantiate(Inventory);
    }

    public override IEnumerator FinishTurn(){
        while ( true ) {
            int horizontal = 0;
            int vertical = 0;

            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            if ( horizontal != 0 ) {
                vertical = 0;
            }

            if ( horizontal != 0 || vertical != 0 ) {
                if ( !bIsMoving ) {
                    Vector2 ADJACENT = (Vector2)transform.position + new Vector2(horizontal, vertical) * 0.32f;
                    Collider2D[] DETECTEDOBJECTS = DetectObjects(ADJACENT);

                    bool bMOVE_BLOCKED = false; // DEFAULT until blocked
                    if ( DETECTEDOBJECTS.Length == 0 ) {
                        bMOVE_BLOCKED = true;
                    }
                    else {
                        yield return null;
                        foreach ( Collider2D OTHER in DETECTEDOBJECTS ) {
                            cTile TILE_OTHER = OTHER.GetComponent<cTile>();
                            if ( TILE_OTHER != null ) {
                                bMOVE_BLOCKED = TILE_OTHER.CheckIfBlocked(GetComponent<cPhysicalObject>(), traversibleTerrain);
                            }
                        }
                    }
                    if ( !bMOVE_BLOCKED ) {
                        startMove(ADJACENT - (Vector2)transform.position, 0.1f);
                        while ( bIsMoving ) { yield return null; }
                        yield return "end";
                        yield break;
                    }
                }
            }
            // end of while loop
            yield return null;
        }
    }
}

public partial class cPlayer {
    public override void TakeDamage(cPhysicalObject INSTIGATOR, int DAMAGE, Vector2 DIRECTION = default(Vector2), enumDamageType TYPE = enumDamageType.Normal) {
        health -= DAMAGE;
        print("OUCH! " + INSTIGATOR.name + " hit you for "+DAMAGE+" damage!");
        if ( health <= 0 ) {
            StartCoroutine(Died(INSTIGATOR));
        }
    }

    public virtual IEnumerator Died(cPhysicalObject INSTIGATOR) {
        float RESET_TIMER = Time.time+1;
        print("You have been killed by "+INSTIGATOR.name);
        while ( Time.time < RESET_TIMER ) { }
        yield return "end";
        Application.LoadLevel(Application.loadedLevel);
        yield break;
    }

    virtual protected void OnTriggerEnter2D(Collider2D OTHER) {
        cItem ITEM_OTHER = OTHER.GetComponent<cItem>();
        if (ITEM_OTHER != null) {
            Inventory.PickUp(ITEM_OTHER);
        }
    }
}