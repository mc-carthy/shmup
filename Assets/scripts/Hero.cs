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
				Main.Instance.DelayedRestart (gameRestartDelay);
			}
		}
	}

	public delegate void WeaponFireDelegate ();
	public WeaponFireDelegate FireDelegate;

	[SerializeField]
	private Weapon[] weapons;

	private float speed = 30f;
	private float rollMult = -45f;
	private float pitchMult = 30f;
	private Bounds bounds;
	private GameObject lastTriggerGo;
	private float gameRestartDelay = 2f;

	protected override void Awake ()
	{
		base.Awake ();
		bounds = Utilities.CombineBoundsOfChildren (gameObject);
	}

	private void Start ()
	{
		ClearWeapons ();
		weapons[0].SetType (WeaponType.Blaster);
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

		if (Input.GetAxis("Jump") == 1 && FireDelegate != null)
		{
			FireDelegate ();
		}
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
			else if (go.tag == "powerUp")
			{
				AbsorbPowerUp (go);
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

	public void AbsorbPowerUp (GameObject go)
	{
		PowerUp pu = go.GetComponent<PowerUp> ();

		switch (pu.type)
		{
			case (WeaponType.Shield):
				shieldLevel++;
				break;
			default:
				if (pu.type == weapons[0].Type)
				{
					Weapon w = GetEmptyWeaponSlot ();
					if (w != null)
					{
						w.SetType (pu.type);
					}
				}
				else
				{
					ClearWeapons ();
					weapons[0].SetType (pu.type);
				}
				break;
		}
		pu.AbsorbedBy (gameObject);
	}

	private Weapon GetEmptyWeaponSlot ()
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i].Type == WeaponType.None)
			{
				return weapons[i];
			}
		}
		return null;
	}

	private void ClearWeapons ()
	{
		foreach (Weapon w in weapons)
		{
			w.SetType (WeaponType.None);
		}
	}
}
