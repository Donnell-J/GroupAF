using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
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
        musicSource.clip = combatMusic;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
