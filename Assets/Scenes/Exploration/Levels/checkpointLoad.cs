using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class checkpointLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        cardDB.instance.loadSaveState();
        if(SceneManager.GetActiveScene().name.Equals("Start")){
            AudioManager.instance.musicSource.clip = AudioManager.instance.menuMusic;
        }else if(SceneManager.GetActiveScene().name.Equals("Level 2")){
            AudioManager.instance.musicSource.clip = AudioManager.instance.level1Music;
        }else if(SceneManager.GetActiveScene().name.Equals("villageLevel")){
            AudioManager.instance.musicSource.clip = AudioManager.instance.level2Music;
        }else if(SceneManager.GetActiveScene().name.Equals("dungeon")){
            AudioManager.instance.musicSource.clip = AudioManager.instance.level3Music;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
