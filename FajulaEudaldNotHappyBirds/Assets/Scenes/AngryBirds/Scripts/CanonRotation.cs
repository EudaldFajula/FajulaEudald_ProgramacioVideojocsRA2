using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CanonRotation : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public Vector3 _maxRotation;
    public Vector3 _minRotation;
    private float _offset = -51.6f;
    public GameObject ShootPoint;
    public GameObject Bullet;
    public float ProjectileSpeed = 0;
    public float MaxSpeed;
    public float MinSpeed;
    public GameObject PotencyBar;
    private float _initialScaleX;
    private Vector2 _distanceBetweenMouseAndPlayer;
    private bool isRaising = false;
    [SerializeField] private float _multiplier = 10f;
    private void Awake()
    {
        _initialScaleX = PotencyBar.transform.localScale.x;
    }
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //obtenir el valor del click del cursor (Fer amb new input system)
        _distanceBetweenMouseAndPlayer = mousePos.normalized; //obtenir el vector distància entre el canó i el cursor
        while (transform.rotation.z > _maxRotation.z && transform.rotation.z < _minRotation.z)
        {
            var ang = (Mathf.Atan2(_distanceBetweenMouseAndPlayer.y, _distanceBetweenMouseAndPlayer.x) * 180f / Mathf.PI + _offset);
            transform.rotation = Quaternion.Euler(0, 0, ang); //en quin dels tres eixos va l'angle?
        }
        if (isRaising)
        {
            ProjectileSpeed = Time.deltaTime * _multiplier + ProjectileSpeed; //acotar entre dos valors (mirar variables)
            CalculateBarScale();
        }

        CalculateBarScale();

    }
    public void CalculateBarScale()
    {
        PotencyBar.transform.localScale = new Vector3(Mathf.Lerp(0, _initialScaleX, ProjectileSpeed / MaxSpeed),
            transform.localScale.y,
            transform.localScale.z);
    }
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        Debug.Log("Hola");
        if (context.started)
        {
            isRaising = true;
        }
        if (context.canceled)
        {
            var projectile = Instantiate(Bullet, transform.position, Quaternion.identity); //canviar la posició on s'instancia
            while (context.action.IsInProgress() && projectile.GetComponent<Rigidbody2D>().linearVelocity.magnitude != MaxSpeed)
            {
                projectile.GetComponent<Rigidbody2D>().linearVelocity = _distanceBetweenMouseAndPlayer * ProjectileSpeed;
            }
            ProjectileSpeed = 0f;
            isRaising = false;
        }

    }
}
