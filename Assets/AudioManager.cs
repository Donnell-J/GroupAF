using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;
    // Start is called before the first frame update

    public AudioClip combatMusic;
    public AudioClip menuMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;


    public AudioClip interactSFX; 
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateSong(string sName){
        musicSource.Stop();
        Debug.Log("update song call: " + sName);
        if(sName.Equals("Start")){
            musicSource.clip = menuMusic;
        }else if(sName.Contains("Level2")){
            musicSource.clip = level1Music;
        }else if(sName.Equals("villageLevel")){
            musicSource.clip = level2Music;
        }else if(sName.Equals("dungeon")){
            musicSource.clip = level3Music;
        } else if(sName.Equals("Level 1 BS") | sName.Equals("Level 2 BS") | sName.Equals("Level 3 BS")){
            musicSource.clip = combatMusic;
        }
        musicSource.Play();
    }

}
