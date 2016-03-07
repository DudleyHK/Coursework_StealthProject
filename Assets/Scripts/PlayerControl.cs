using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float speed = 0f;
	public float rotationSpeed = 0f;
	public bool inMainRoom = true;



	// Update is called once per frame
	void Update () 
	{
		Movement ();
		SideStep ();
	}

	void Movement()
	{
		// set variable for forward motion and turning.
		float translation = Input.GetAxis ("Vertical") * speed * Time.deltaTime;
		float rotation = Input.GetAxis ("Horizontal") * rotationSpeed * Time.deltaTime;


		// move player
		transform.Translate (0, 0, translation);
		transform.Rotate (0, rotation, 0);
	}

	// This is good but need to determine the direction the player is facing
	void SideStep()
	{
		if(Input.GetKey(KeyCode.Q))
		{
			transform.localPosition -= Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.E))
		{
			transform.localPosition += Vector3.right * speed * Time.deltaTime;
		}
	}


	
	void OnTriggerEnter(Collider other)
	{
		// if you hit the trigger
		if(other.tag == (Tags.mainRoomCamTrigger))
		{
			inMainRoom = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		// if you hit the main room trigger
		if(other.tag == (Tags.mainRoomCamTrigger))
		{
			inMainRoom = false;
		}

	}
}