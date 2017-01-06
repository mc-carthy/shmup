using UnityEngine;

public class Hero : Singleton<Hero> {

	private float shieldLevel = 1f;
	public float ShieldLevel 
	{ 
		get { return shieldLevel; } 
		set {
			shieldLevel = Mathf.Min (value, 4);
			if (value < 0)
			{
				Destroy (gameObject);
			}
		}
	}

	private float speed = 30f;
	private float rollMult = -45f;
	private float pitchMult = 30f;
	private Bounds bounds;
	private GameObject lastTriggerGo;

	protected override void Awake ()
	{
		base.Awake ();
		bounds = Utilities.CombineBoundsOfChildren (gameObject);
	}


	private void Update () 
    {
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;

		bounds.center = transform.position;

		// Keep the ship constrained to the screen bounds
		Vector3 offset = Utilities.ScreenBoundsCheck (bounds, BoundsTest.OnScreen);

		if (offset != Vector3.zero)
		{
			pos -= offset;
			transform.position = pos;
		}

		transform.rotation = Quaternion.Euler (yAxis * pitchMult, xAxis * rollMult, 0);
	}

	private void OnTriggerEnter (Collider other)
	{
		GameObject go = Utilities.FindTaggedParent (other.gameObject);

		if (go != null)
		{
			if (go == lastTriggerGo)
			{
				return;
			}

			lastTriggerGo = go;

			if (go.tag == "enemy")
			{
				ShieldLevel--;
				Destroy (go);
			}
			else
			{
				Debug.Log ("Triggered : " + other.gameObject.name);
			}
		}
		else
		{
		}
	}
}
