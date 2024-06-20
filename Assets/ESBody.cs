using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ESBody : MonoBehaviour
{
    [SerializeField]
    private int _currentHealth = 0;
    [SerializeField]
    private int _maxHealth = 3;
    [SerializeField]
    private float _speed = 0.5f;
    [SerializeField]
    private float _initialSpeed = 1f;
    [SerializeField]
    private float _maxSpeed = 1.0f;
    [SerializeField]
    private float _acceleration = 0.1f;
    [SerializeField]
    private float _dodgeAcceleration = 0.1f;

    [SerializeField]
    private float _currentWaitTime = 0;
    [SerializeField]
    private float _maxWaitTime = 0.5f;
    [SerializeField]
    private float _normalShotVelocity = -50.0f;
    [SerializeField]
    private float _maxFollowDistance = 8f;

    private bool _startMoving;
    private bool _IsCountingDown;
    private bool _playerDetected;
    [SerializeField]
    private float _raycastLength = 10f; //Adjust based on your game's scale
    [SerializeField]
    private LayerMask _raycastLayerMask; //To specify which layers the RayCast should check
    [SerializeField]
    private float _shotCooldown = 1.0f; //Cooldown duration between shots
    private float _timeSinceLastShot = 0.0f; //Timer to track time since last shot
    [SerializeField]
    private float _dodgeSpeed = 2.5f; //Speed at which the enemy dodges
    [SerializeField]
    private float _dodgeDetectionRadius = 0.2f; //Detection radius for dodging
    [SerializeField]
    private LayerMask _projectileLayerMask; //Layer mask to detect projectiles
    [SerializeField]
    private float _rotationSpeed = 10f; //Degrees per second
    private Coroutine sweepRaysCoroutine = null;
    [SerializeField]
    private float _alignmentAngleThreshold = 10f; //degrees
    private bool isDodging = false;
    private float targetDodgeSpeed;

    public bool StartMovingCheck { get { return _startMoving; } set { _startMoving = value; } }

    public bool IsCountingDown { get { return _IsCountingDown; } set { _IsCountingDown = value; } }

    private PlayerMainManger playerMainManager;

    void Start()
    {
        _timeSinceLastShot = 0;
    }

    public void Initialize()
    {
        playerMainManager = FindObjectOfType<PlayerMainManger>();
        _maxHealth = 3;
        _currentHealth = _maxHealth;
        //_speed = 0.05f;

        _currentWaitTime = 0;
        _startMoving = false;
        _IsCountingDown = false;
        _playerDetected = false;
        sweepRaysCoroutine = null;

        _projectileLayerMask = 1 << LayerMask.NameToLayer("PlayerBullets");
        _raycastLayerMask = -1;

        gameObject.SetActive(false);
        transform.GetChild(1).GetChild(0).GetComponent<Collider2D>().enabled = false;

    }

    public void BeginObject()
    {
        _maxHealth = 3;
        _currentHealth = _maxHealth;
        //_speed = 0.05f;

        _currentWaitTime = 0;
        _startMoving = true;
        _IsCountingDown = false;

        Debug.Log("BEGINOBJ");

        transform.GetChild(1).GetChild(0).GetComponent<Collider2D>().enabled = true;

        gameObject.SetActive(true);
    }

    public void UpdateScript()
    {
                Debug.Log("UPDATEESBODY");

        if (_IsCountingDown)
        {
            if (_currentWaitTime == _maxWaitTime)
            {
                _currentWaitTime = 0;
                _IsCountingDown = false;
                _startMoving = true;
            }
            else
            {
                _currentWaitTime += Time.deltaTime;

                if (_currentWaitTime > _maxWaitTime)
                {
                    _currentWaitTime = _maxWaitTime;
                }
            }
        }


        Vector3 shipPosition = playerMainManager.ShipTransform.position;
        Vector3 directionToPlayer = (shipPosition - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(-transform.up, directionToPlayer);

        if (angleToPlayer < _alignmentAngleThreshold && _timeSinceLastShot >= _shotCooldown)
        {
            ShootBeam();
            _timeSinceLastShot = 0.0f;
        }

        if (_timeSinceLastShot < _shotCooldown)
        {
            _timeSinceLastShot += Time.deltaTime;
        }

        Vector3 moveVector = transform.position;

        BulletDodge(ref moveVector);


        if (_startMoving && !_playerDetected)
        {
            Move();
        }

        //CheckBoundry();
    }

    private void CheckBoundry()
    {
        if (transform.position.y > 10 || transform.position.y < -10)
        {
            RemoveESShip();
        }
        else if (transform.position.x > 11 || transform.position.x < -11)
        {
            RemoveESShip();
        }
    }


private void Move()
{
    Vector3 moveVector = transform.position;
    bool dodged = BulletDodge(ref moveVector);

    if (!dodged)
    {
        // Apply vertical movement only if not dodging
        moveVector.y += -_initialSpeed * Time.deltaTime;
        transform.position = moveVector;
    }

    if (sweepRaysCoroutine == null)
    {
        sweepRaysCoroutine = StartCoroutine(SweepRays());
    }
}

    private IEnumerator SweepRays()
    {
        float detectionRadius = 5f;
        float coneAngle = 90f;
        float sweepSpeed = 30f; //Adjusted for smoother sweep
        bool sweepingRight = true;

        float currentAngle = -coneAngle / 2;

        while (!_playerDetected)
        {
            float rayAngle = currentAngle + transform.eulerAngles.z;
            Vector2 rayDirection = Quaternion.AngleAxis(rayAngle, Vector3.forward) * Vector2.down;
            Vector3 offsetPosition = Quaternion.Euler(0, 0, transform.eulerAngles.z) * new Vector3(0, -0.5f, 0);
            Vector3 rayStartPosition = transform.position + offsetPosition;

            RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, rayDirection, detectionRadius, _raycastLayerMask);
            Debug.DrawRay(rayStartPosition, rayDirection * detectionRadius, Color.red, 0.01f);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                _playerDetected = true;
                Debug.Log($"Player detected at angle {rayAngle} within cone, moving towards player.");
                StartCoroutine(MoveTowardsPlayer());
                break; //Exit the loop if the player is detected
            }

            //Update the current angle for the sweep direction
            currentAngle += (sweepingRight ? sweepSpeed : -sweepSpeed) * Time.deltaTime;

            //Check if the sweep has reached the limit of the cone angle
            if (currentAngle >= coneAngle / 2 || currentAngle <= -coneAngle / 2)
            {
                //Reverse the sweep direction
                sweepingRight = !sweepingRight;
            }

            yield return null; //Wait until the next frame to continue the sweep
        }

        if (_playerDetected)
        {
            StartCoroutine(MoveTowardsPlayer());
        }
        sweepRaysCoroutine = null;
    }

    private IEnumerator MoveTowardsPlayer()
    {
        Vector3 initialTargetPosition = playerMainManager.ShipTransform.position; //Lock on the initial position at detection
        float updateInterval = 0.2f; //Interval to find the new location of the player ship
        float nextUpdateTime = Time.time + updateInterval;
        float currentSpeed = _speed; //Start moving at the enemy's base speed

        while (Vector3.Distance(transform.position, initialTargetPosition) < _maxFollowDistance)
        {
            if (Time.time >= nextUpdateTime)
            {
                //Update target position to the player's current position at intervals
                initialTargetPosition = playerMainManager.ShipTransform.position;
                nextUpdateTime = Time.time + updateInterval;
            }
            Vector3 directionToPlayer = initialTargetPosition - transform.position;
            //Calculate the angle to rotate towards
            //adjusted for enemy sprite orientation
            float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + 90;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
            //Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            //Calculate direction towards the target position
            Vector3 direction = (initialTargetPosition - transform.position).normalized;
            //Accelerate towards the target position
            currentSpeed += _acceleration * Time.deltaTime;
            //Move towards the target position
            transform.position += direction * currentSpeed * Time.deltaTime;

            yield return null;
        }
        _playerDetected = false;
        if (sweepRaysCoroutine == null)
        {
            sweepRaysCoroutine = StartCoroutine(SweepRays());
        }
    }

private bool BulletDodge(ref Vector3 moveVector)
{
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _dodgeDetectionRadius, _projectileLayerMask);
    bool dodgedThisFrame = false;

    foreach (var hit in hits)
    {
        if (Random.value <= 0.8f) // 80% chance to dodge
        {
            if (!isDodging)
            {
                isDodging = true;
                targetDodgeSpeed = Random.Range(_dodgeSpeed, _maxSpeed);
            }

            // Randomly choose dodge direction
            float dodgeDirection = Random.value < 0.5f ? -1 : 1;

            // Apply dodge speed in the chosen direction
            moveVector.x += dodgeDirection * targetDodgeSpeed * Time.deltaTime;
            dodgedThisFrame = true;
            break;
        }
    }

    if (isDodging && !dodgedThisFrame)
    {
        isDodging = false;
    }

    if (isDodging)
    {
        // Apply the dodge movement
        transform.position += new Vector3(moveVector.x, 0, 0) * Time.deltaTime;
    }

    return dodgedThisFrame;
}
    public void RemoveObject()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Bullet Body") && _currentHealth > 0)
        {
            BulletBody bullet = collision.GetComponent<BulletBody>();

            TakeDamage(bullet.DealDamage());

            bullet.RemoveBullet();
        }
    }

    private void TakeDamage(int Amount)
    {
        _currentHealth -= Mathf.Abs(Amount);

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            EventSystemReference.Instance.ExplostionRequestEventHandler.Invoke(transform.position);

            EventSystemReference.Instance.ESPutObjectBackToSleepEventHandler.Invoke(this);

            transform.GetChild(1).GetChild(0).GetComponent<Collider2D>().enabled = false;

            EventSystemReference.Instance.SendScoreToPlayerEventHandler.Invoke(1);

            gameObject.SetActive(false);
        }
    }

    private void ShootBeam()
    {
        BulletContiner continer = new BulletContiner();


        continer.position = transform.position + transform.up * -0.5f;
        continer.bulletType = BulletType.EnemyNormalBullet;
        continer.damage = 5;
        continer.beamColor = Color.red;
        continer.speed = _normalShotVelocity;
        continer.direction = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;




        EventSystemReference.Instance.BulletRequestEventHandler.Invoke(continer);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void RemoveESShip()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        EventSystemReference.Instance.ESPutObjectBackToSleepEventHandler.Invoke(this);
    }

}