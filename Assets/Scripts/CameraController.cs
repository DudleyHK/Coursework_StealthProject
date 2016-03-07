using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

// Website Link:: https://unity3d.com/learn/tutorials/projects/stealth/camera-movement

	/*CAMERA TO MOVE TO SEE PLAYER VARIABLES*/
	private float relativeCameraPosMag;		// The distance of the camera from the player
	public float birdsEyeSmoothDamp = 5f;
	private Transform player;				// Reference the players transform
	private Transform birdsEyePos;
	private Vector3 relativeCameraPos;		// The relative position of the camera from the player
	private Vector3 newPosition;			// The position the camera is trying to reach


	void Awake()
	{
		// Set up references
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		birdsEyePos = GameObject.FindGameObjectWithTag (Tags.birdsEyeCam).transform;

		// Setting the relative position as the intial relative position of the camera in the scene
		relativeCameraPos = transform.position - player.position;
		relativeCameraPosMag = relativeCameraPos.magnitude - 0.5f;
	}


	void Update()
	{
		// if you are in the main room trigger
		if (player.GetComponent<PlayerControl> ().inMainRoom == true) 
		{
			InMainRoomCameraSettings();
		}
		else
		{
			AlwaysLookingAtPLayerCameraSettings();
		}
	}


	void InMainRoomCameraSettings()
	{	
		// lerp to the next position
		transform.position = Vector3.Lerp (transform.position, birdsEyePos.position, 1.5f * Time.deltaTime);

		SmoothLookAt (birdsEyeSmoothDamp);
	}


	void AlwaysLookingAtPLayerCameraSettings()
	{
		float nonMainRoomSmoothDamp = 1.5f;

		// The standard position of the camera is the relative position of the camera from the player
		Vector3 standardPosition = player.position + relativeCameraPos;
		
		// The abovePos is direcly above the player at the same distance as the standard position
		Vector3 abovePos = player.position + Vector3.up * relativeCameraPosMag;
		
		// An array of the 5 points to check if the camera can see the player
		Vector3[] checkPoints = new Vector3[5];
		
		// the first is the standard position of the camera 
		checkPoints [0] = standardPosition;
		
		// The next three are 25%, 50% and 75% of the distance between the stanard position and abovePos.
		// Lerp finds positions between given vectors.
		checkPoints [1] = Vector3.Lerp (standardPosition, abovePos, 0.25f);
		checkPoints [2] = Vector3.Lerp (standardPosition, abovePos, 0.5f);
		checkPoints [3] = Vector3.Lerp (standardPosition, abovePos, 0.75f);
		
		checkPoints [4] = abovePos;
		
		//run through the check points
		for (int i = 0; i < checkPoints.Length; i++) 
		{
			// if the camera can see the player
			if (ViewingPosCheck (checkPoints [i])) 
			{
				Debug.Log (checkPoints [i]);

				// break out of the loop
				break;
			}
		}
		
		// Lerp the camera position between its current position and its new position
		transform.position = Vector3.Lerp (transform.position, newPosition, nonMainRoomSmoothDamp * Time.deltaTime);

		// Make sure the camera is looking at the player
		SmoothLookAt (nonMainRoomSmoothDamp);
	}


	bool ViewingPosCheck(Vector3 checkPos)
	{
		RaycastHit hit;

		// if a raycast from the check position to the player hits something
		if(Physics.Raycast(checkPos, player.position - checkPos, out hit, relativeCameraPosMag))
		{
			// if it is not the player
			if (hit.transform != player)
			{
				//This position isnt appropriate
				return false;
			}
		}

		// if we havent hit anything or we've hit the player, this is an appropriate position
		newPosition = checkPos;

		return true;
	}


	void SmoothLookAt(float smooth)
	{
		// Create a vector from the camera towards the player
		Vector3 relativePlayerPosition = player.position - transform.position;

		// create a rotation based on the relative position of the player being the forward vector
		Quaternion lookAtRotation = Quaternion.LookRotation (relativePlayerPosition, Vector3.up);

		// Lerp the cameras rotation between its current rotatin and the rotation that look at the player
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRotation, smooth * Time.deltaTime);
	}
}