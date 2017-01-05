using UnityEngine;

[AddComponentMenu ("Vestige/Hero")]
public class Hero : Singleton<Hero> {

	private float speed = 30f;
	private float rollMult = -45f;
	private float pitchMult = 30f;

	private float shieldLevel = 1f;

	private void Update () 
    {
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;

		transform.rotation = Quaternion.Euler (yAxis * pitchMult, xAxis * rollMult, 0);
	}
}
