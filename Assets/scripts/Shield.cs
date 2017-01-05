using UnityEngine;

[AddComponentMenu ("Vistage/Shield")]
public class Shield : MonoBehaviour {

	private Renderer ren;
	private float rotationsPerSecond = 0.1f;
	private int levelShown;

	private void Awake ()
	{
		ren = GetComponent<Renderer> ();
	}

	private void Update () 
    {
		int currentLevel = Mathf.FloorToInt (Hero.Instance.ShieldLevel);

		if (levelShown != currentLevel)
		{
			levelShown = currentLevel;
			Material mat = ren.material;
			mat.mainTextureOffset = new Vector2 (0.2f * levelShown, 0);
		}

		float rotZ = (rotationsPerSecond * Time.time * 360) % 360f;
		transform.rotation = Quaternion.Euler (0, 0, rotZ);
	}
}
