    using UnityEngine;
using System.Collections;

/*https://www.youtube.com/watch?v=hrFJLwpKXXQ*/
/*https://unity3d.com/learn/tutorials/projects/stealth/enemy-ai?playlist=17168*/ // Enenmy AI

enum EnemyState
{
	Patrol,		// move between points
	Chase,		// Follow player and hover above them
	Search		// If player goes out of range scan area around them
}


public class SaucerControl : MonoBehaviour {


	/*
	 * What this script should do:
	 * 	- Move between two points
	 * 	- Shoot a light out below
	 * 	- Check for a detection in the light
	 * 	- chase the player and hover above them when detection is made
	 * 	- fall to the floor and set a bool activation script to make pillars fall 
	 * 	- Take away player health
	 * 	- shoot player
	 * 	- Have three states; patrol, chase and scan area for player - use laser to scan area for player scifi style
	 */

	EnemyState state = new EnemyState();

	public bool pointAHit = false;
	public bool pointBHit = false;
	private bool isDetected = false;

	public float plusSpeed = 0f;
	public float minusSpeed = 0f;
	public float chaseSpeed = 0f;


	public float height;// NEW
	public float currentHeight;// NEW

	public float chaseTimer = 0f;
	public float chaseTimeLimit = 5f;

	private NavMeshAgent nav;
	private Transform player;
	private LastPlayerSighting lastPlayerSighting;



	void Awake()
	{
		// this will momentarily start the movement
		pointBHit = true;

		// Set state
		state = EnemyState.Patrol;

		// set reference to player object
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
	}


	// Update is called once per frame
	void Update () 
	{
		// Depending on the state
		switch (state)
		{
		case EnemyState.Patrol:


			Patrol ();

			// if the player has been hit by the raycast
			if(isDetected == true)
			{
				// reset the variable
				isDetected = false;

				// go to next state
				state = EnemyState.Chase;
			}

			break;

		case EnemyState.Chase:

			Debug.Log("Chase State Active");

			Chase();

			break;

		case EnemyState.Search:
			break;

		default:
			break;
		}
	}


	// if either trigger point is hit - move in given direction
	void Patrol()
	{
		if(pointAHit == true)
		{
			transform.Translate (Vector3.right * plusSpeed * Time.deltaTime);
		}
		if(pointBHit == true)
		{
			transform.Translate (Vector3.right * minusSpeed * Time.deltaTime);
		}
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


	void Chase ()
	{	
		//get the distance to the player.
	//float	distance = Vector3.Distance (transform.position,player.position);

		// increment the timer
		chaseTimer += Time.deltaTime;

		//if (chaseTimer <= chaseTimeLimit) 
		//{

		 
	//	Vector3 playerPosition = player.position;

	//	transform.position = Vector3.Lerp (transform.position, playerPosition, chaseSpeed * Time.deltaTime);
	

		//}
	}


	void OnTriggerEnter(Collider other)
	{
		// if saucer hits trigger move between points
		if(other.tag == (Tags.saucerPointA))
		{
			pointBHit = false;
			pointAHit = true;
		}
		if (other.tag == (Tags.saucerPointB))
		{
			pointAHit = false;
			pointBHit = true;
		}
	}

	void OnTriggerStay(Collider other)
	{
		// If the player is inside the trigger box
		if(other.tag == (Tags.player))
		{
			// set this to the return value
			isDetected = Detect();
		}
	}

	void OnTriggerExit(Collider other)
	{

	}
}



















































