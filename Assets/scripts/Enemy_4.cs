using UnityEngine;

[System.SerializableAttribute]
public class Part {

    // [SerializeField]
    // private string name;
    // public string Name { get; }
    // [SerializeField]
    // private float health;
    // public float Health { get; set; }
    // [SerializeField]
    // private string [] protectedBy;
    // public string[] ProtectedBy { get; }
    // private GameObject go;
    // public GameObject Go { get; set; }
    // private Material mat;
    // public Material Mat { get; set; }

    public string name;
    public float health;
    public string [] protectedBy;
    public GameObject go;
    public Material mat;
}

public class Enemy_4 : Enemy {

    // [Serial izeField]
    public Part [] parts;
    private Vector3 [] points;
    private float timeStart;
    private float duration = 4f;

    private void Start ()
    {
        points = new Vector3 [2];
        points [0] = Pos;
        points [1] = Pos;

        InitMovement ();

        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find (prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer> ().material;
            }
        }
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

    private void OnCollisionEnter (Collision other)
    {
        GameObject otherGo = other.gameObject;

        switch (otherGo.tag)
        {
            case ("projectileHero"):
                Projectile p = otherGo.GetComponent<Projectile> ();
                bounds.center = transform.position + BoundsCenterOffset;

                if (bounds.extents == Vector3.zero || Utilities.ScreenBoundsCheck (bounds, BoundsTest.OffScreen) != Vector3.zero)
                {
                    Destroy (otherGo);
                    break;
                }

                GameObject goHit = other.contacts [0].thisCollider.gameObject;
                Part prtHit = FindPart (goHit);

                if (prtHit == null)
                {
                    goHit = other.contacts [0].otherCollider.gameObject;
                    prtHit = FindPart (goHit);
                }

                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy)
                    {
                        if (!Destroyed (s))
                        {
                            Destroy (otherGo);
                            return;
                        }
                    }
                }

                prtHit.health -= Main.W_DEFS [p.Type].damageOnHit;
                ShowLocalizedDamage (prtHit.mat);

                if (prtHit.health <= 0)
                {
                    prtHit.go.SetActive (false);
                }

                bool allDestroyed = true;

                foreach (Part prt in parts)
                {
                    if (!Destroyed (prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }

                if (allDestroyed)
                {
                    Main.Instance.ShipDestroyed (this);
                    Destroy (gameObject);
                }

                Destroy (otherGo);
                break;
        }
    }

    private Part FindPart (string n)
    {
        foreach (Part prt in parts)
        {
            if (prt.name == n)
            {
                return prt;
            }
        }
        return null;
    }

    private Part FindPart (GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go)
            {
                return prt;
            }
        }
        return null;
    }

    private bool Destroyed (GameObject go)
    {
        return Destroyed (FindPart (go));
    }

    private bool Destroyed (string n)
    {
        return Destroyed (FindPart (n));
    }

    private bool Destroyed (Part prt)
    {
        if (prt == null)
        {
            return true;
        }
        return (prt.health <= 0);
    }

    private void ShowLocalizedDamage (Material m)
    {
        m.color = Color.red;
        remainingDamageFrames = ShowDamageForFrames;
    }

}
