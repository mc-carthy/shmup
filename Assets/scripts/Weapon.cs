using UnityEngine;

public enum WeaponType {
    None,
    Blaster,
    Spread,
    Phaser,
    Missile,
    Laser,
    Shield
}

[System.SerializableAttribute]
public class WeaponDefinition {
    public WeaponType type = WeaponType.None;
    public string letter;
    public Color collarColor = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit;
    public float continuousDamage;
    public float delayBetweenShots;
    public float projectileVelocity;
}

public class Weapon : MonoBehaviour {

	static public Transform PROJECTILE_ANCHOR;


    [SerializeField]
    private WeaponType type = WeaponType.Blaster;
    public WeaponType Type
    {
        get { return type; }
        set 
        {
            SetType (value);
        }
    }
    public WeaponDefinition def;
    private GameObject collar;
    public float lastShot;

    private void Awake ()
    {
        collar = transform.Find ("collar").gameObject;
    }

    private void Start ()
    {
        SetType (type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject ("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
            PROJECTILE_ANCHOR.gameObject.tag = "projectileHero";
        }

        GameObject parentGo = transform.parent.gameObject;
        if (parentGo.tag == "hero")
        {
            Hero.Instance.FireDelegate += Fire;
        }
    }

    public void SetType (WeaponType wt)
    {
        type = wt;
        if (type == WeaponType.None)
        {
            gameObject.SetActive (false);
            return;
        }
        else
        {
            gameObject.SetActive (true);
        }

        def = Main.GetWeaponDefinition (type);
        collar.GetComponent<Renderer> ().material.color = def.collarColor;
        lastShot = 0f;
    }

    public void Fire ()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (Time.time - lastShot < def.delayBetweenShots)
        {
            return;
        }

        Projectile p;
        switch (type)
        {
            case (WeaponType.Blaster):
                p = MakeProjectile ();
                p.GetComponent<Rigidbody> ().velocity = Vector3.up * def.projectileVelocity;
                break;
            
            case (WeaponType.Spread):
                p = MakeProjectile ();
                p.GetComponent<Rigidbody> ().velocity = Vector3.up * def.projectileVelocity;
                p = MakeProjectile ();
                p.GetComponent<Rigidbody> ().velocity = new Vector3 (-0.2f, 0.9f, 0) * def.projectileVelocity;
                p = MakeProjectile ();
                p.GetComponent<Rigidbody> ().velocity = new Vector3 (0.2f, 0.9f, 0) * def.projectileVelocity;
                break;
        }
    }

    public Projectile MakeProjectile ()
    {
        GameObject go = Instantiate (def.projectilePrefab) as GameObject;

        if (transform.parent.gameObject.tag == "hero")
        {
            go.tag = "projectileHero";
            go.layer = LayerMask.NameToLayer("projectileHero");
        }
        else
        {
            go.tag = "projectileEnemy";
            go.layer = LayerMask.NameToLayer("projectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;

        Projectile p = go.GetComponent<Projectile> ();
        p.Type = type;
        lastShot = Time.time;
        return p;
    }

}
