using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CanonRotation : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    public Vector3 _maxRotation;
    public Vector3 _minRotation;
    private float _offset = -51.6f;
    public GameObject ShootPoint;
    public GameObject Bullet;
    public float ProjectileSpeed = 1;
    public float MaxSpeed;
    public float MinSpeed;
    public GameObject PotencyBar;
    private float _initialScaleX;
    private Vector2 _distanceBetweenMouseAndPlayer;
    private bool isRaising = false;
    [SerializeField] private float _multiplier = 10f;

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void Awake()
    {
        _initialScaleX = PotencyBar.transform.localScale.x;
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
    }
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //obtenir el valor del click del cursor (Fer amb new input system)
        _distanceBetweenMouseAndPlayer = mousePos.normalized; //obtenir el vector distància entre el canó i el cursor
        var ang = (Mathf.Atan2(_distanceBetweenMouseAndPlayer.y, _distanceBetweenMouseAndPlayer.x) * 180f / Mathf.PI + _offset);
        if (ang < _maxRotation.z && ang > _minRotation.z)
        {
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
        
        if (context.started)
        {
            isRaising = true;
        }
        if (context.canceled)
        {
            var projectile = Instantiate(Bullet, transform.position, Quaternion.identity); //canviar la posició on s'instancia
            if(ProjectileSpeed > MaxSpeed)
            {
                ProjectileSpeed = MaxSpeed;
            }
            projectile.GetComponent<Rigidbody2D>().linearVelocity = _distanceBetweenMouseAndPlayer * ProjectileSpeed;
            ProjectileSpeed = 0f;
            isRaising = false;
        }

    }
}
