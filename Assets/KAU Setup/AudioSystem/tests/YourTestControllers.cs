using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KAUGamesLviv.Services.Audio;

public class YourTestControllers : MonoBehaviour
{

    [SerializeField] private SoundDefinition coins;
    [SerializeField] private SoundDefinition music;

    public bool testCoind = false;
    public bool testMusic = false;
    public bool testGetterOnMusic = false;
    public bool testGetterOnMaster = false;
	[Space]
	[Range(0,1)]
	public float setVolumeOnMaster = 1;
	public bool testSetter = false;

	public bool testSetterMusicVolume = false;

	[Header("Music tests")]
	[Space]
	public bool testDecibelToAmplitude = false;
	public bool testAmplitudeToDecibel = false;



    void Update()
    {
        if (testCoind)
        {
            SoundSystem.StartSound(coins);
            testCoind = false;
        }

		if (testMusic)
        {
            SoundSystem.StartSound(music);
            testMusic = false;
        }

		if (testGetterOnMusic)
        {
            var something =  SoundSystem.GetVolume( SoundSettings.SoundMixerGroup.MUSIC );
			Debug.Log( " Volume on Music " + something);
            testGetterOnMusic = false;
        }

		if (testGetterOnMaster)
        {
            var something = SoundSystem.GetVolume( SoundSettings.SoundMixerGroup.Master );
			Debug.Log( " Volume on Master " + something);
            testGetterOnMaster = false;
        }

		if (testSetter)
        {
            SoundSystem.SetVolume( SoundSettings.SoundMixerGroup.Master, setVolumeOnMaster);
			Debug.Log( " Volume is set on  Master " +  SoundSystem.GetVolume( SoundSettings.SoundMixerGroup.Master ));
            testSetter = false;
        }

		if (testSetterMusicVolume)
        {
            SoundSystem.SetVolume( SoundSettings.SoundMixerGroup.MUSIC, setVolumeOnMaster);
			Debug.Log( " Volume is set on  Music " +  SoundSystem.GetVolume( SoundSettings.SoundMixerGroup.MUSIC ));
			Debug.Log( " Volume is set on  Master " +  SoundSystem.GetVolume( SoundSettings.SoundMixerGroup.Master ));

            testSetterMusicVolume = false;
        }

	
    }
}
