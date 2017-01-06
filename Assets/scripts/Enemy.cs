using UnityEngine;

public class Enemy : MonoBehaviour {

	public Vector3 Pos {
		get { return transform.position; }
		set { transform.position = value; }
	}

	private float speed = 10f;
	private float fireRate = 0.3f;
	private float health = 10f;
	private int score = 100;
	
	private Bounds bounds;
	private Vector3 boundsCenterOffset;

	private void Awake ()
	{
		InvokeRepeating ("CheckOffScreen", 0f, 2f);
	}

	private void Update ()
	{
		Move ();
	}

	private void Move ()
	{
		Vector3 tempPos = Pos;
		tempPos.y -= speed * Time.deltaTime;
		Pos = tempPos;
	}

	private void CheckOffScreen ()
	{
		// Check if bounds are still at default value, set them
		if (bounds.size == Vector3.zero)
		{
			bounds = Utilities.CombineBoundsOfChildren (gameObject);
			boundsCenterOffset = bounds.center - transform.position;
		}

		// Update the bounds
		bounds.center = transform.position + boundsCenterOffset;

		// Check if the bounds are completely offscreen
		Vector3 offset = Utilities.ScreenBoundsCheck (bounds, BoundsTest.OffScreen);
		if (offset != Vector3.zero)
		{
			// Check if the enemy has gone off the bottom of the screen
			if (offset.y < 0)
			{
				Destroy (gameObject);
			}
		}
	}
}
