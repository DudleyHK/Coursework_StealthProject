﻿using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour 
{
	public Vector3 position = new Vector3(1000f, 1000f, 1000f);
	public Vector3 resetPosition = new Vector3 (1000f, 1000f, 1000f);
	public float lightHighIntensity = 0.25f;
	public float lightLowIntensity = 0f;
	public float fadeSpeed = 7f;
	public float musicFadeSpeed = 1f;

	private AlarmLight alarm;
	private Light mainLight;
	private AudioSource panicAudio;
	private AudioSource[] sirens;


	void Awake()
	{
		// Setup the reference to the alarm light.
		alarm = GameObject.FindGameObjectWithTag (Tags.alarm).GetComponent<AlarmLight> (); // Currently no alarm light

		// Setup the reference to the main directional light in the scene.
		mainLight = GameObject.FindGameObjectWithTag (Tags.mainLight).GetComponent<Light>();

		// Setup the reference to the additonal audio source.
		panicAudio = GameObject.FindGameObjectWithTag("Siren").GetComponent<AudioSource> ();

		// Find an array of the siren gameobjects.
		GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag (Tags.siren);

		// Set the sirens array to have the same number of elements as there are gameobjects.
		sirens = new AudioSource[sirenGameObjects.Length];

		// For all the sirens allocate the audio source of the gameobjects
		for(int i = 0; i < sirens.Length; i++)
		{
			sirens[i] = sirenGameObjects[i].GetComponent<AudioSource>();;
		}
	}

	void Update()
	{
		SwitchAlarms ();
		MusicFading ();
	}

	void SwitchAlarms()
	{
		// Set the alarm light to be on or off.
		alarm.alarmOn = (position != resetPosition);

		float newIntensity;

		// If the position is not the reset position...
		if(position != resetPosition)
		{
			// ... then set the new intensity to low.
			newIntensity = lightLowIntensity;

		}
		else
		{
			// Otherwise set the new intensity to high.
			newIntensity = lightHighIntensity;
		}

		// Fade the directional light's intensity in or out.
		mainLight.intensity = Mathf.Lerp (mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

		// For all of the sirens...
		for(int i = 0; i < sirens.Length; i++)
		{
			// ... if alarm is triggered and the audio isn't playing, then play the audio.
			if(position != resetPosition && !sirens[i].isPlaying)
			{
				sirens[i].Play();
			}
			// Otherwise if the alarm isn't triggered, stop the audio
			else if (position == resetPosition)
			{
				sirens[i].Stop();
			}
		}
	}

	void MusicFading()
	{
		// If the alarm is not being triggered...
		if(position != resetPosition)
		{
			// ... fade out the normal music...
			GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0f, musicFadeSpeed * Time.deltaTime);

			// ... and fade in the panic music.
			panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
		}
		else
		{
			// Otherwise fade in the normal music and fade out the panic music.
			GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0.8f, musicFadeSpeed * Time.deltaTime);
			panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
		}
	}
}
