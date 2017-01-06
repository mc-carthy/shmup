using UnityEngine;

public class Enemy_2 : Enemy {

	private Vector3[] points;
    private float birthTime;
    private float lifeTime = 10f;
    private float sinEccentricity = 0.6f;

    private void Start ()
    {
        points = new Vector3 [2];

        Vector3 cbMin = Utilities.CamBounds.min;
        Vector3 cbMax = Utilities.CamBounds.max;

        Vector3 v = Vector3.zero;
        v.x = cbMin.x - Main.Instance.EnemySpawnPadding;
        v.y = Random.Range (cbMin.y, cbMax.y);
        points [0] = v;

        v = Vector3.zero;
        v.x = cbMax.x + Main.Instance.EnemySpawnPadding;
        v.y = Random.Range (cbMin.y, cbMax.y);
        points [1] = v;

        if (Random.value < 0.5f)
        {
            points [0].x *= -1;
            points [1].x *= -1;
        }

        birthTime = Time.time;
    }

    public override void Move ()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy (gameObject);
        }

        u = u + sinEccentricity * (Mathf.Sin (u * Mathf.PI * 2));

        Pos = (1 - u) * points [0] + u * points [1];
    }

}
