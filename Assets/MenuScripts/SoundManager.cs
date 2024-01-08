using UnityEngine;
using UnityEngine.UI;

public class SoundManager  : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    
    void Start()
    {
    }
    
    public void ChangeVolume()
    {
        AudioListener.volume = soundSlider.value;
    }
}
