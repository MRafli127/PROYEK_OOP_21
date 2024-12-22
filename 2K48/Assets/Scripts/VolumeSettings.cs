using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer Mymixer;
    [SerializeField] private Slider MusicSlider;

    private void Start(){
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else{
            SetVolume();
        }
        
    }
    public void SetVolume()
    {
        float volume = MusicSlider.value;
        Mymixer.SetFloat("Master", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void LoadVolume()
    {
        MusicSlider.value=PlayerPrefs.GetFloat("MasterVolume");
        
        SetVolume();
    }
}
