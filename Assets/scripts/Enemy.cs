using UnityEngine;

[AddComponentMenu ("Vistage/Enemy")]
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
}
