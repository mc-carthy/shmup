using UnityEngine;

public class PowerUp : MonoBehaviour {

    public WeaponType type;
	private Vector2 rotMinMax = new Vector2 (15f, 90f);
    private Vector2 driftMinMax = new Vector2 (0.25f, 2f);
    private float lifeTime = 6f;
    private float fadeTime = 4f;
    private GameObject cube;
    private TextMesh letter;
    private Vector3 rotPerSecond;
    private float birthTime;
    private Rigidbody rb;
    private Renderer cubeRen;
    private Collider cubeCol;

    private void Awake ()
    {
        cube = transform.Find("Cube").gameObject;
        cubeRen = cube.GetComponent<Renderer> ();
        cubeCol = cube.GetComponent<Collider> ();
        letter = GetComponent<TextMesh> ();
        rb = GetComponent<Rigidbody> ();

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize ();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rb.velocity = vel;

        transform.rotation = Quaternion.identity;
        rotPerSecond = new Vector3 (
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y)
        );

        InvokeRepeating ("CheckOffScreen", 2f, 2f);

        birthTime = Time.time;
    }

    private void Update ()
    {
        cube.transform.rotation = Quaternion.Euler (rotPerSecond * Time.time);

        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        if (u >= 1)
        {
            Destroy (gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = cubeRen.material.color;
            c.a = 1f - u;
            cubeRen.material.color = c;
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
    }

    public void SetType (WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition (wt);
        cubeRen.material.color = def.collarColor;
        letter.text = def.letter;
        type = wt;
    }

    public void AbsorbedBy (GameObject target)
    {
        Destroy (gameObject);
    }

    private void CheckOffScreen ()
    {
        if (Utilities.ScreenBoundsCheck (cubeCol.bounds, BoundsTest.OffScreen) != Vector3.zero)
        {
            Destroy (gameObject);
        }
    }

}
