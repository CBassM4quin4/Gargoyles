using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cTurtle : cPhysicalObject {
    public enum enumDirection {
        North,
        East,
        South,
        West
    }
    public enumDirection eDirection;

    protected override void Start() {
        base.Start();
        setMoveType(enumMoveType.Quadratic);
    }

    protected override void Update()
    {
        base.Update();
 
        if (!bIsMoving)
        {
            Vector2 LOOK_AHEAD = new Vector2();
            if (eDirection == enumDirection.North) {
                LOOK_AHEAD = new Vector2(0, 1);
                rigidbody.rotation = 0;
            }
            if (eDirection == enumDirection.East) {
                LOOK_AHEAD = new Vector2(1, 0);
                rigidbody.rotation = 270;
            }
            if (eDirection == enumDirection.South) {
                LOOK_AHEAD = new Vector2(0, -1);
                rigidbody.rotation = 180;
            }
            if (eDirection == enumDirection.West) {
                LOOK_AHEAD = new Vector2(-1, 0);
                rigidbody.rotation = 90;
            }

            LOOK_AHEAD *= 0.32f;
            Collider2D[] DETECTEDOBJECTS = DetectObjects((Vector2)transform.position + LOOK_AHEAD, 0.01f);

            bool bMOVE_BLOCKED = false; // DEFAULT until blocked
            if (DETECTEDOBJECTS.Length == 0) {
                //bMOVE_BLOCKED = true; 
            }
            else {
                foreach (Collider2D OTHER in DETECTEDOBJECTS) {
                    cTile TILE_OTHER = OTHER.GetComponent<cTile>();
                    if (TILE_OTHER != null) {
                        cPhysicalObject TMP = GetComponent<cPhysicalObject>();
                        if (TMP != null)
                            bMOVE_BLOCKED = TILE_OTHER.CheckIfBlocked(TMP, traversibleTerrain);
                    }
                }
            }
            if (!bMOVE_BLOCKED) { startMove(LOOK_AHEAD, 0.6f); }
            else {
                eDirection++;
                if (eDirection > enumDirection.West) { eDirection = 0; }
            }
        }
    }
}
