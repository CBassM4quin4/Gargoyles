using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
//public struct TileKey {
//    public GameObject prefab;
//    public string legend;
//}

//public class cGameManager : MonoBehaviour {

//    public static cGameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.

//    public List<TileKey> TileMapper;
//    public int sceneToLoad;
//    public TextAsset mapFile;

//    // Use this for initialization
//    void Start() {
//        if (instance == null){
//            instance = this;
//        }
//        else if (instance != this){
//            instance.sceneToLoad = sceneToLoad;
//            Destroy(gameObject);
//        }

//        DontDestroyOnLoad(gameObject);
//        string[,] grid = CSVReader.SplitCsvGrid(mapFile.text);
//        for (int i = 0; i < grid.GetUpperBound(0); i++){
//            for (int j = 0; j < grid.GetUpperBound(1); j++){
//                CreateFromKey(grid[i, j], new Vector3(0.32f * i, -0.32f * j, 0), new Quaternion());}
//        }
//    }

//    void CreateFromKey(string KEY, Vector3 POSITION = new Vector3(), Quaternion ROTATION = new Quaternion()) {
//        //    Instantiate(Resources.Load("Prefabs/Floor", typeof(GameObject)), new Vector3(0.32f * i, -0.32f * j, 0), new Quaternion());
//        for (int i = 0; i < TileMapper.Count; i++) {
//            if (TileMapper[i].legend == KEY) {
//                Instantiate(TileMapper[i].prefab, POSITION, ROTATION);
//            }
//        }
//    }
//}

[Serializable]
public class cGameManager : MonoBehaviour {
    public static cGameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
    public List<cPhysicalObject> turnBasedObjects;
    public List<int> emptyObjects;

    // Use this for initialization
    void Start() {
        //if ( instance == null ) { instance = this; }
        //else if ( instance != this ) {
        //    // Might be good to copy some parameters from the new one first... then...
        //    Destroy(gameObject);
        //}
        //DontDestroyOnLoad(gameObject);

//            if ( OTHER.activeInHierarchy )
//                print(thisObject + " is an active object");
        cPhysicalObject[] ACTOR_LIST = FindObjectsOfType<cPhysicalObject>();
        foreach ( cPhysicalObject OTHER in ACTOR_LIST ) {
            if ( OTHER.bIsTurnBased ) { turnBasedObjects.Add(OTHER); }
        }
        StartCoroutine(RunTurnBased());
    }
    public void AddTurnBasedObject(cPhysicalObject NEW_OBJECT) {
        turnBasedObjects.Add(NEW_OBJECT);
    }

    public CoroutineVariable currentTurn;

    private IEnumerator RunTurnBased() {
        cPhysicalObject ACTOR_UP;

        while ( true ) {

            print("STARTING TURN BASED, number of objects... " + turnBasedObjects.Count);
            for ( int i = 0; i < turnBasedObjects.Count; i++ ) {
                ACTOR_UP = turnBasedObjects[i];

                // If the actor was destroyed last time around, note that item can be replaced, and skip to next...
                if ( ACTOR_UP == null ) { continue; }

                // Let the actor take their turn...
                ACTOR_UP.StartTurn();
                currentTurn = new CoroutineVariable(this, ACTOR_UP.FinishTurn());
                while ( (string)currentTurn.result != "end" ) {
                    if ( ACTOR_UP == null ) { break; }
                    yield return null;
                }

                // If the actor was destroyed last time around, note that item can be replaced, and skip to next...
                if ( ACTOR_UP == null ) { continue; }
            }
        }
    }
}