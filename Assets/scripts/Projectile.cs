using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private WeaponType type;
    public WeaponType Type { get; set; }

    private Collider col;
    private Renderer ren;

	private void Awake ()
    {
        col = GetComponent<Collider> ();
        ren = GetComponent<Renderer> ();
        InvokeRepeating ("CheckOffScreen", 2f, 2f);
    }

    public void SetType (WeaponType eType)
    {
        type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition (type);
        ren.material.color = def.projectileColor;
    }

    private void CheckOffScreen ()
    {
        if (Utilities.ScreenBoundsCheck (col.bounds, BoundsTest.OffScreen) != Vector3.zero)
        {
            Destroy (gameObject);
        }
    }

}
