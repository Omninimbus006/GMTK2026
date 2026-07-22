using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
	public AudioMixer audioMixer;

	//Slider references
	public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;
	public Slider fovSlider;

	
	void Start()
	{
		//Fetch saved values
		float savedMaster = PlayerPrefs.GetFloat("Master", -2f);
                float savedMusic = PlayerPrefs.GetFloat("Music", 0f);
                float savedSFX = PlayerPrefs.GetFloat("SFX", 0f);
		float savedFOV = PlayerPrefs.GetInt("FOV", 40);

		//Set slider positions to saved value
		if(masterSlider != null) masterSlider.value = savedMaster;
                if(musicSlider != null) musicSlider.value = savedMusic;
                if(sfxSlider != null) sfxSlider.value = savedSFX;
		if(fovSlider != null) fovSlider.value = savedFOV;

		//Set values
		SetMaster(savedMaster);
                SetMusic(savedMusic);
                SetSFX(savedSFX);
	}

	public void SetMaster(float volume)
	{
		audioMixer.SetFloat("Master", volume);
		PlayerPrefs.SetFloat("Master", volume);
		PlayerPrefs.Save();
	}

	public void SetMusic(float volume)
	{
		audioMixer.SetFloat("Music", volume);
		PlayerPrefs.SetFloat("Music", volume);
		PlayerPrefs.Save();
	}

	public void SetSFX(float volume)
	{
		audioMixer.SetFloat("SFX", volume);
		PlayerPrefs.SetFloat("SFX", volume);
		PlayerPrefs.Save();
	}

	public void SetFOV(float FOV)
	{
		PlayerPrefs.SetInt("FOV", (int)FOV);
		PlayerPrefs.Save();
	}
}
