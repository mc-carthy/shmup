using UnityEngine;

public class Enemy_3 : Enemy {

    private Vector3[] points;
    private float birthTime;
    private float lifeTime = 10f;

    private void Start ()
    {
        points = new Vector3 [3];
        points [0] = Pos;

        float xMin = Utilities.CamBounds.min.x + Main.Instance.EnemySpawnPadding;
        float xMax = Utilities.CamBounds.max.x - Main.Instance.EnemySpawnPadding;

        Vector3 v;

        v = Vector3.zero;
        v.x = Random.Range (xMin, xMax);
        v.y = Random.Range (2 * Utilities.CamBounds.min.y, Utilities.CamBounds.min.y);
        points [1] = v;

        v = Vector3.zero;
        v.x = Random.Range (xMin, xMax);
        v.y = Pos.y;
        points [2] = v;

        birthTime = Time.time;        
    }

    public override void Move ()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy (gameObject);
            return;
        }

        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin (u * Mathf.PI * 2);
        p01 = (1 - u) * points [0] + u * points [1]; 
        p12 = (1 - u) * points [1] + u * points [2];
        Pos = (1 - u) * p01 + u * p12; 
    }

}
