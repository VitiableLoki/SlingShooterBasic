using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SlingShot : MonoBehaviour {

	//Public inspector fields
	public float velocityMultiplier = 10.0f;
	//f shows that it is a float and not a double (shown by d)
	private GameObject launchPoint;

	public GameObject prefabProjectile;

	public AudioClip arrowSound;

	public AudioClip music;

	//public AudioClip mother;

	//Internal fields

	private bool aimingMode;
	private Vector3 launchPos;
	private GameObject projectile;
	private AudioSource source;
	//private float vol = 1.0f;

	// Use this for initialization
	void Start () {
	
	}

	void Awake() {
		Transform launchPointTransform = transform.Find ("LaunchPoint");
		launchPoint = launchPointTransform.gameObject;
		launchPoint.SetActive (false);
		launchPos = launchPointTransform.position;
		source = GetComponent<AudioSource>();
		source.PlayOneShot(music);
	}

	void OnMouseEnter() {
		//print ("Yay");
		launchPoint.SetActive (true);
	}

	void OnMouseExit () {
		//print ("Booooooo");
		launchPoint.SetActive (false);
	}
	// Update is called once per frame

	void OnMouseDown() {

		//Set the aiming mode variable to true
		aimingMode = true;

		//Instantiate a projectile and store it in our "projectile" variable
		projectile = Instantiate(prefabProjectile) as GameObject;

		//Set its position to the launch point
		projectile.transform.position = launchPos;

		//Make our projectile kinematic (for now)
		projectile.GetComponent <Rigidbody>().isKinematic = true;

		//Set the camera poi to this newly created projectile
		//FollowCam.S.poi = projectile;

	}

	/*void OnCollision() {

		source.PlayOneShot (mother);

	}*/

	void Update () {

		//if we are not in aiming mode, then do nothing
		if (aimingMode == false) {
			return;
		}

		//get the current mouse position in screen coordinates
		Vector3 mousePos2D = Input.mousePosition;

		//convert mouse position to 3D world space
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);

		//find the difference between the launch position and my mouse position
		Vector3 mouseDelta = mousePos3D - launchPos;

		//move the projectile to this new position
		float radius = GetComponent<SphereCollider>().radius;

		mouseDelta = Vector3.ClampMagnitude (mouseDelta, radius);

		projectile.transform.position = launchPos + mouseDelta;

		//if the mouse button has been released, 
		if(Input.GetMouseButtonUp(0) == true) {

			source = GetComponent<AudioSource>();

			//exit aimaing mode
			aimingMode = false;

			//stop the projectile from being kinematic
			projectile.GetComponent<Rigidbody>().isKinematic = false;

			//set the projectile's velocity to the negative mouseDelta
			projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMultiplier;

			//Set the camera poi to this newly created projectile
			FollowCam.S.poi = projectile;

			//Add arrow sound effect
			source.PlayOneShot(arrowSound);
		}

	}
}
