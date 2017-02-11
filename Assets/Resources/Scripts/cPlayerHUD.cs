using UnityEngine;

public class cPlayerHUD : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 80, 150, 30), "Current Scene: " + Application.loadedLevel);
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 50, 150, 40), "Move to Scene " + cGameManager.instance.sceneToLoad))
        {
            Application.LoadLevel(cGameManager.instance.sceneToLoad);
        }
    }
}
