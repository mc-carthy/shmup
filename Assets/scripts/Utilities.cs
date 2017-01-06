using UnityEngine;
using System.Collections.Generic;

public enum BoundsTest {
	// Center of gameObject on screen
	Center,
	// Entirety of gameObject on screen
	OnScreen, 
	// Entirety of gameObject off screen
	OffScreen
}

public class Utilities : MonoBehaviour {

// Bounds functions =======================================
	

	public static Bounds BoundsUnion (Bounds b0, Bounds b1)
	{
		if (b0.size == Vector3.zero && b1.size != Vector3.zero)
		{
			return (b1);
		}
		if (b0.size != Vector3.zero && b1.size == Vector3.zero)
		{
			return (b0);
		}
		if (b0.size == Vector3.zero && b1.size == Vector3.zero)
		{
			return (b0);
		}

		b0.Encapsulate(b1.min);
		b0.Encapsulate(b1.max);
		return (b0);
	}

	public static Bounds CombineBoundsOfChildren (GameObject go)
	{
		Bounds b = new Bounds (Vector3.zero, Vector3.zero);

		Renderer ren = go.GetComponent<Renderer> ();
		if (ren != null)
		{
			b = BoundsUnion (b, ren.bounds);
		}

		Collider col = go.GetComponent<Collider> ();
		if (col != null)
		{
			b = BoundsUnion (b, col.bounds);
		}

		foreach (Transform t in go.transform)
		{
			b = BoundsUnion (b, CombineBoundsOfChildren (t.gameObject));
		}

		return b;
	}

	static private Bounds camBounds;
	static public Bounds CamBounds
	{
		get {
			if (camBounds.size == Vector3.zero)
			{
				SetCameraBounds ();
			}
			return camBounds;
		}
	}

	public static void SetCameraBounds (Camera cam = null)
	{
		if (cam == null)
		{
			// Ensure camera is orthographic and has a rotation of Vector3.zero
			cam = Camera.main;

			Vector3 topLeft = new Vector3 (0, 0, 0);
			Vector3 bottomRight = new Vector3 (Screen.width, Screen.height, 0);

			Vector3 boundTLN = cam.ScreenToWorldPoint (topLeft);
			Vector3 boundBRF = cam.ScreenToWorldPoint (bottomRight);
			boundTLN.z += cam.nearClipPlane;
			boundBRF.z += cam.farClipPlane;

			Vector3 center = (boundTLN + boundBRF) / 2f;
			camBounds = new Bounds (center, Vector3.zero);
			camBounds.Encapsulate (boundTLN);
			camBounds.Encapsulate (boundBRF);
		}
	}

	public static Vector3 ScreenBoundsCheck (Bounds bound, BoundsTest test = BoundsTest.Center)
	{
		return (BoundsInBoundsCheck (CamBounds, bound, test));
	}

	// Check to see if Bounds lilB are within bigB
	public static Vector3 BoundsInBoundsCheck (Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.OnScreen)
	{
		Vector3 pos = lilB.center;
		Vector3 offset = Vector3.zero;

		switch (test)
		{
			// Determine what offset would need to be applied to lilB to move its center within bigB
			case (BoundsTest.Center):
				if (bigB.Contains (pos))
				{
					return Vector3.zero;
				}

				if (pos.x > bigB.max.x)
				{
					offset.x = pos.x - bigB.max.x;
				}
				else if (pos.x < bigB.min.x)
				{
					offset.x = pos.x - bigB.min.x;
				}

				if (pos.y > bigB.max.y)
				{
					offset.y = pos.y - bigB.max.y;
				}
				else if (pos.y < bigB.min.y)
				{
					offset.y = pos.y - bigB.min.y;
				}

				if (pos.z > bigB.max.z)
				{
					offset.z = pos.z - bigB.max.z;
				}
				else if (pos.z < bigB.min.z)
				{
					offset.z = pos.z - bigB.min.z;
				}

				return offset;

			// Determine what offset would have to be applied to lilb to keep it entirely within bigB
			case (BoundsTest.OnScreen):
				if (bigB.Contains (lilB.min) && bigB.Contains (lilB.max))
				{
					return Vector3.zero;
				}

				if (lilB.max.x > bigB.max.x)
				{
					offset.x = lilB.max.x - bigB.max.x;
				}
				else if (lilB.min.x < bigB.min.x)
				{
					offset.x = lilB.min.x - bigB.min.x;
				}

				if (lilB.max.y > bigB.max.y)
				{
					offset.y = lilB.max.y - bigB.max.y;
				}
				else if (lilB.min.y < bigB.min.y)
				{
					offset.y = lilB.min.y - bigB.min.y;
				}

				if (lilB.max.z > bigB.max.z)
				{
					offset.z = lilB.max.z - bigB.max.z;
				}
				else if (lilB.min.z < bigB.min.z)
				{
					offset.z = lilB.min.z - bigB.min.z;
				}

				return offset;

			// Determine what offset would have to be applied to lilb to move any part to within bigB
			case (BoundsTest.OffScreen):
				bool cMin = bigB.Contains (lilB.min);
				bool cMax = bigB.Contains (lilB.max);

				if (cMin || cMax) {
					return Vector3.zero;
				}

				if (lilB.min.x > bigB.max.x)
				{
					offset.x = lilB.min.x - bigB.max.x;
				}
				else if (lilB.max.x < bigB.min.x)
				{
					offset.x = lilB.max.x - bigB.min.x;
				}

				if (lilB.min.y > bigB.max.y)
				{
					offset.y = lilB.min.y - bigB.max.y;
				}
				else if (lilB.max.y < bigB.min.y)
				{
					offset.y = lilB.max.y - bigB.min.y;
				}

				if (lilB.min.z > bigB.max.z)
				{
					offset.z = lilB.min.z - bigB.max.z;
				}
				else if (lilB.max.z < bigB.min.z)
				{
					offset.z = lilB.max.z - bigB.min.z;
				}

				return offset;
		}

		return Vector3.zero;
	}

// Transform functions =========================

	public static GameObject FindTaggedParent (GameObject go)
	{
		if (go.tag != "Untagged")
		{
			return go;
		}

		if (go.transform.parent == null)
		{
			return null;
		}

		return FindTaggedParent (go.transform.parent.gameObject);
	}

	public static GameObject FindTaggedParent (Transform t)
	{
		return FindTaggedParent (t.gameObject);
	}
}
