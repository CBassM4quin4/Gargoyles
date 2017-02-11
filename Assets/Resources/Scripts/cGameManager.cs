using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TileKey
{
    public GameObject prefab;
    public string legend;
}

public class cGameManager : MonoBehaviour {

    public static cGameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.

    public List<TileKey> TileMapper;
    public int sceneToLoad;
    public TextAsset mapFile;

    // Use this for initialization
    void Start() {
        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            instance.sceneToLoad = sceneToLoad;
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        string[,] grid = CSVReader.SplitCsvGrid(mapFile.text);
        for (int i = 0; i < grid.GetUpperBound(0); i++){
            for (int j = 0; j < grid.GetUpperBound(1); j++){
                CreateFromKey(grid[i, j], new Vector3(0.32f * i, -0.32f * j, 0), new Quaternion());}
        }
    }

    void CreateFromKey(string KEY, Vector3 POSITION = new Vector3(), Quaternion ROTATION = new Quaternion()) {
        //    Instantiate(Resources.Load("Prefabs/Floor", typeof(GameObject)), new Vector3(0.32f * i, -0.32f * j, 0), new Quaternion());
        for (int i = 0; i < TileMapper.Count; i++) {
            if (TileMapper[i].legend == KEY) {
                Instantiate(TileMapper[i].prefab, POSITION, ROTATION);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
    }
}
