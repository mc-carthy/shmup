using UnityEngine;

public class Enemy_4 : Enemy {

    private Vector3 [] points;
    private float timeStart;
    private float duration = 4f;

    private void Start ()
    {
        points = new Vector3 [2];
        points [0] = Pos;
        points [1] = Pos;

        InitMovement ();
    }

    private void InitMovement ()
    {
        Vector3 p1 = Vector3.zero;
        float esp = Main.Instance.EnemySpawnPadding;
        Bounds cBounds = Utilities.CamBounds;

        p1.x = Random.Range (cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range (cBounds.min.y + esp, cBounds.max.y - esp);

        points [0] = points [1];
        points [1] = p1;

        timeStart = Time.time;
    }

    public override void Move ()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement ();
            u = 0;
        }

        u = 1 - Mathf.Pow (1 - u, 2);

        Pos = (1 - u) * points [0] + u * points [1];
    }

}
