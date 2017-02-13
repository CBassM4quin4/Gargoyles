using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Declarations
[Serializable]
public partial class cPhysicalObject : cGamePiece {
    public enum enumBlockingType {
        AllBlocking,
        Wall,
        Floor,
        Bridge,
        Fluid,
        Pit,
        NonBlocking,
        Interactive
    }
    public List<enumBlockingType> traversibleTerrain;
    public List<cPhysicalObject> nonBlocking;

    protected Rigidbody2D rigidbody;
    protected Vector2 desiredPosition;
    protected float desiredRotation;

    public enum enumMoveType {
        Instant,
        Linear,
        Quadratic
    }
    enumMoveType eMoveType;
    private delegate IEnumerator delegateMoveType(Vector2 VECTOR, float MODIFIER);
    private event delegateMoveType selectedMoveType;
    private Coroutine cofxnMoveType;
    protected bool bIsMoving;

    //protected virtual void Start()
    //
    //Collider2D[] DetectObjects(Vector3 center, float radius)
    //
    //public void setMoveType(enumMoveType MOVE_TYPE)
    //protected void startMove(Vector2 VECTOR, float MODIFIER)
    //protected IEnumerator MoveTypeInstant(Vector2 TRANSLATION, float DELAY)
    //protected IEnumerator MoveTypeLinear(Vector2 TRANSLATION, float MOVE_TIME)
    //protected IEnumerator MoveTypeQuadratic(Vector2 TRANSLATION, float MOVE_TIME)
}

public partial class cPhysicalObject {
    protected virtual void Start(){
        rigidbody = GetComponent<Rigidbody2D>();
        desiredPosition = rigidbody.position;
        desiredRotation = rigidbody.rotation;

        //eMoveType = enumMoveType.Instant;
        //selectedMoveType = MoveTypeInstant;
        setMoveType(enumMoveType.Instant);
        cofxnMoveType = null;
        bIsMoving = false;
    }

    public Collider2D[] DetectObjects(Vector2 center) {
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(center);
        return hitColliders;
    }
    public void setMoveType(enumMoveType MOVE_TYPE) {
        if (MOVE_TYPE == enumMoveType.Instant) { selectedMoveType = MoveTypeInstant; }
        else if (MOVE_TYPE == enumMoveType.Linear) { selectedMoveType = MoveTypeLinear; }
        else if (MOVE_TYPE == enumMoveType.Quadratic) { selectedMoveType = MoveTypeQuadratic; }
    }
    protected void startMove(Vector2 TRANSLATION_VECTOR, float MODIFIER) {
        bIsMoving = true;
        if (cofxnMoveType != null) {
            StopCoroutine(cofxnMoveType);
        }
        cofxnMoveType = StartCoroutine(selectedMoveType(TRANSLATION_VECTOR, MODIFIER));
    }
    protected IEnumerator MoveTypeInstant(Vector2 TRANSLATION, float DELAY)
    {
        Vector2 END = rigidbody.position + TRANSLATION;
        yield return new WaitForSeconds(DELAY);
        rigidbody.MovePosition(END);
        bIsMoving = false;
    }
    protected IEnumerator MoveTypeLinear(Vector2 TRANSLATION, float MOVE_TIME){
        float INVERSE_MOVE_TIME = 1f/MOVE_TIME;
        float START_TIME = Time.time;
        float END_TIME = START_TIME + MOVE_TIME;
        Vector2 START = rigidbody.position;

        while (Time.time < END_TIME) {
            rigidbody.MovePosition(START + TRANSLATION * (Time.time-START_TIME)*INVERSE_MOVE_TIME);
            yield return null;
        }
        rigidbody.MovePosition(START + TRANSLATION);
        bIsMoving = false;
    }
    protected IEnumerator MoveTypeQuadratic(Vector2 TRANSLATION, float MOVE_TIME) {
        float SINE_MOD = 180f / MOVE_TIME;
        float START_TIME = Time.time;
        float END_TIME = START_TIME + MOVE_TIME;
        Vector2 START = rigidbody.position;

        while (Time.time < END_TIME) {
            rigidbody.MovePosition(START + TRANSLATION * (Mathf.Sin(Mathf.Deg2Rad * (-90f + (Time.time - START_TIME) * SINE_MOD))+1f)/2f);
            yield return null;
        }
        rigidbody.MovePosition(START + TRANSLATION);
        bIsMoving = false;
    }
}

public partial class cPhysicalObject {
    public bool bIsTurnBased;

    // For objects that take turns making decisions, perform an action
    // Return "end" if move is completed
    public virtual bool StartTurn() {
        return true;
    }
    public virtual IEnumerator FinishTurn() {
        yield return "end";
        yield break;
    }
}
public partial class cPhysicalObject {
    public enum enumDamageType {
        Normal,
        Deathtouch,
        Corrosive,
        Water,
        Frost,
        Fire,
        Blunt,
        Piercing,
        Explosive,
        Pressure,
        Drowning,
        Falling,
        Sound,
        Toxic
    }
    public int health=100;

    public virtual void TakeDamage(cPhysicalObject INSTIGATOR, int DAMAGE, Vector2 DIRECTION = default(Vector2), enumDamageType TYPE = enumDamageType.Normal) {}
}