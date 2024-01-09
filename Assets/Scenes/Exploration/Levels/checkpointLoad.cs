using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class checkpointLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string sName = SceneManager.GetActiveScene().name;
        AudioManager.instance.updateSong(sName);
        string lScene = MovingScenes.instance.getFromScene();

        if(!(lScene.Equals("Level 1 BS") | lScene.Equals("Level 2 BS") | lScene.Equals("Level 3 BS"))){
            Debug.Log("Must load deck");
            cardDB.instance.loadSaveState();
            MovingScenes.instance.resetTriggers();
        }
        
        //Debug.Log(MovingScenes.instance.getPreCombatPosition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
