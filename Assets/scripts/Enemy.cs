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
	private int showDamageForFrames = 2;
	private Color[] originalColors;
	private Material[] materials;
	public int remainingDamageFrames;
	
	private Bounds bounds;
	private Vector3 boundsCenterOffset;

	private void Awake ()
	{
		materials = Utilities.GetAllMaterials (gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++)
		{
			originalColors[i] = materials[i].color;
		}

		InvokeRepeating ("CheckOffScreen", 0f, 2f);
	}

	private void Update ()
	{
		Move ();
		if (remainingDamageFrames > 0)
		{
			remainingDamageFrames--;
			if (remainingDamageFrames == 0)
			{
				UnShowDamage ();
			}
		}
	}

	private void OnCollisionEnter (Collision other)
	{
		GameObject otherGo = other.gameObject;

		Debug.Log(otherGo.name);

		switch (otherGo.tag)
		{
			case ("projectileHero"):
			Projectile p = otherGo.GetComponent<Projectile> ();
			bounds.center = transform.position + boundsCenterOffset;

			if (bounds.extents == Vector3.zero || Utilities.ScreenBoundsCheck (bounds, BoundsTest.OffScreen) != Vector3.zero)
			{
				Destroy (otherGo);
				break;
			}

			ShowDamage ();
			health -= Main.W_DEFS[p.Type].damageOnHit;
			if (health <= 0)
			{
				Destroy (gameObject);
			}

			Destroy (otherGo);
			break;

		}
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

	private void ShowDamage ()
	{
		foreach (Material m in materials)
		{
			m.color = Color.red;
		}
		remainingDamageFrames = showDamageForFrames;
	}

	private void UnShowDamage() {
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].color = originalColors[i];
		}
	}
}
