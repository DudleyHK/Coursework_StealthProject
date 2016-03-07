using UnityEngine;
using System.Collections;

/*https://www.youtube.com/watch?v=hrFJLwpKXXQ*/

enum DroneState
{
	Patrol,		// move between points
	Chase,		// Follow player and hover above them
	Dead		// If player goes out of range scan area around them
}


public class SaucerControl : MonoBehaviour {


	/*
	 * What this script should do:
	 * 	- Move between two points
	 * 	- Shoot a light out below
	 * 	- Check for a detection in the light
	 * 	- chase the player and hover above them when detection is made
	 * 	- fall to the floor and set a bool activation script to make pillars fall 
	 * 	- Have three states; patrol, chase and scan area for player - use laser to scan area for player scifi style
	 */

	DroneState state = new DroneState();

	private bool isDetected = false;

	public float patrolSpeed = 0f;
	public float chaseSpeed = 0f;

	private int wayPointIndex;
	
	public float timer = 0f;
	public float timeLimit = 0f;
	
	private LastPlayerSighting lastPlayerSighting;

	private Transform player;
	public Transform childObject;
	public Transform[] wayPoints;

	private Light collisionLight;
	private NavMeshAgent nav;



	void Awake()
	{
		// Set state
		state = DroneState.Patrol;

		// set reference to objects
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;														// Player
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();			// Last Player Sighting Script
		collisionLight = childObject.GetComponent<Light> ();																	// Drones' Collision Light																				// Drones' Main Light
		nav = GetComponent<NavMeshAgent> ();
	}


	// Update is called once per frame
	void Update () 
	{
		// Depending on the state
		switch (state)
		{
		case DroneState.Patrol:

			Patrol ();

			// if the player has been hit by the raycast
			if(isDetected == true)
			{
				// if(playerHealth > 0f) // if player is alivee
				// go to next state
				state = DroneState.Chase;
			}

			break;

		case DroneState.Chase:

			// turn the objects lights off
			collisionLight.enabled = false;


			Chase ();

			break;

		case DroneState.Dead:
			
			Disable ();
			
			break;

		default:
			break;
		}
	}


	// if either trigger point is hit - move in given direction
	void Patrol()
	{
		nav.speed = patrolSpeed;

		if (nav.remainingDistance < nav.stoppingDistance) 
		{
			// check if we're at the way point
			if (wayPointIndex == wayPoints.Length - 1) 
			{
				Debug.Log ("Set wayPointIndex to 0");
				wayPointIndex = 0;
			} 
			else 
			{
				Debug.Log ("Increment wayPointIndex");
				wayPointIndex++;
			}
		}

		nav.destination = wayPoints [wayPointIndex].position;
	}


	/*When the player is inside the the trigger box return true to go to the chase state*/
	bool Detect()
	{
		// ... raycast from the camera towards the player.	
		Vector3 relPlayerPos = player.transform.position - transform.position;
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position, relPlayerPos, out hit))
		{
			// If the raycast hits the player...
			if(hit.transform == player)
			{
				// ... set the last global sighting of the player to the player's position.
				lastPlayerSighting.position = player.transform.position;
				return true;
			}
		}

		return false;
	}

	
	void Chase()
	{
		/// could set a distance and check if the player is within distance


		nav.destination = player.position;
		nav.speed = chaseSpeed;


		// increment timer
		timer += Time.deltaTime;
		if (timer >= timeLimit) 
		{
			// for now deactivate the drone
			state = DroneState.Dead;
		}
	}


	void Disable()
	{
		
		Debug.Log("Drone is dead");

		// decactive the drones
		GetComponent<Rigidbody> ().useGravity = true;
		Destroy (nav);
	}

	void OnTriggerStay(Collider other)
	{
		// If the player is inside the trigger box
		if(other.tag == (Tags.player))
		{
			isDetected = Detect();
		}
	}

	void OnTriggerExit(Collider other)
	{

	}
}



















































