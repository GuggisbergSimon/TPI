//some parts of the code taken from catlikecoding

//todo better snap player to ground if on moving platform descending
//todo tweak values - wip

using System.Collections;
using Gravity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerInputSpace;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 10f, maxSprintSpeed = 20f;

    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f, maxSprintAcceleration = 20f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)] private float jumpHeight = 2f;
    [SerializeField, Range(0, 5)] private int maxAirJumps;
    [SerializeField, Range(0, 90)] private float maxGroundAngle = 25f, maxStairsAngle = 50f;
    [SerializeField, Range(0f, 100f)] private float maxSnapSpeed = 100f;
    [SerializeField, Min(0f)] private float probeDistance = 1f;
    [SerializeField] private LayerMask probeMask = -1, stairsMask = -1;
    [Space] [Space] [SerializeField] private Vector2 mouseSensitivity = new Vector2(1000f, 1000f);
    [SerializeField, Range(0f, 90f)] private float minAngleLook = 70f, maxAngleLook = 70f;
    [SerializeField] private Camera cam;
    [SerializeField] private bool invertAxisX, invertAxisY;
    [SerializeField] private float maxDistanceInteractable = 3f;
    [SerializeField] private LayerMask layerInteractable = -1;
    [SerializeField] private float maxTimeThrow = 3f, throwStrength = 10f;
    [SerializeField] private float fallMultiplier = 0.5f, lowJumpMultiplier = 0.5f;
    [SerializeField] private bool enablePushbackThrow;

    private Rigidbody _body, _connectedBody, _previousConnectedBody;
    private Vector3 _playerInput;
    private Vector3 _velocity, _connectionVelocity;
    private Vector3 _connectionWorldPosition, _connectionLocalPosition;
    private Vector3 _upAxis, _rightAxis, _forwardAxis;
    private bool _desiredJump;
    private Vector3 _contactNormal, _steepNormal;
    private int _groundContactCount, _steepContactCount;
    private int _jumpPhase;
    private float _minGroundDotProduct, _minStairsDotProduct;
    private int _stepsSinceLastGrounded, _stepsSinceLastJump;

    private enum PlayerState
    {
        Pause,
        Idle
    }

    private PlayerState _state = PlayerState.Idle;
    private Vector3 _rotationCam;
    private ItemGrab _grabbedItem;
    private Coroutine _throwCoroutine;

    public bool InvertAxisX
    {
        get => invertAxisX;
        set => invertAxisX = value;
    }

    public bool InvertAxisY
    {
        get => invertAxisY;
        set => invertAxisY = value;
    }

    public Vector2 MouseSensitivity => mouseSensitivity;

    private bool OnGround => _groundContactCount > 0;
    private bool OnSteep => _steepContactCount > 0;

    #region Unity Functions

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _body.useGravity = false;
        OnValidate();
    }

    private void Update()
    {
        if (_state == PlayerState.Pause)
        {
            return;
        }

        CheckInteraction();
        CheckGrab();
        CheckDrop();
        AdjustCursor();
        AdjustRotationCam();

        //opens the pause menu
        if (Input.GetButtonDown("Cancel"))
        {
            Pause(true);
            GameManager.Instance.UIManager.Pause(true);
        }


        //adjusts and clamps players input to 1, to avoid fastest movement in diagonal
        _playerInput.x = Input.GetAxis("Horizontal");
        _playerInput.y = Input.GetAxis("Vertical");
        _playerInput.z = 0f;
        _playerInput = Vector3.ClampMagnitude(_playerInput, 1f);

        //adjusts the axis to match depending on player input space
        if (playerInputSpace)
        {
            _rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, _upAxis);
            _forwardAxis =
                ProjectDirectionOnPlane(playerInputSpace.forward, _upAxis);
        }
        else
        {
            _rightAxis = ProjectDirectionOnPlane(Vector3.right, _upAxis);
            _forwardAxis = ProjectDirectionOnPlane(Vector3.forward, _upAxis);
        }

        //once jump button pushed, the input stays saved as is until the jump is being dealt with in FixedUpdate
        _desiredJump |= Input.GetButtonDown("Jump");

        if (Input.GetButtonDown("Throw") && _grabbedItem != null)
        {
            if (_throwCoroutine != null)
            {
                StopCoroutine(_throwCoroutine);
            }

            maxTimeThrow = maxTimeThrow <= 0f ? StaticsValues.SMALLEST_INT : maxTimeThrow;
            _throwCoroutine = StartCoroutine(Throwing(0f, 1f, 1 / maxTimeThrow));
        }
    }

    private void FixedUpdate()
    {
        //applies the gravity, changing the up axis
        Vector3 gravity = CustomGravity.GetGravity(_body.position, out _upAxis);

        UpdateState();
        AdjustVelocity();
        AdjustRotationBody();

        //applies the jump if the jump button has been pressed since last FixedUpdate
        if (_desiredJump)
        {
            _desiredJump = false;
            Jump(gravity);
        }

        //applies velocity depending of surface being touched
        if (OnGround && _velocity.sqrMagnitude < 0.01f)
        {
            _velocity += _contactNormal * (Vector3.Dot(gravity, _contactNormal) * Time.deltaTime);
        }
        else
        {
            _velocity += gravity * Time.deltaTime;
        }

        //applies fallMultiplier
        float dotVelocityUp = Vector3.Dot(Vector3.Project(_velocity, _upAxis), _upAxis);

        if (dotVelocityUp < 0 && !OnGround && !OnSteep)
        {
            /*if (!_isFalling && !_isGrounded)
            {
                _isFalling = true;
            }*/
            _velocity += (fallMultiplier - 1) * Time.deltaTime * gravity;
        }
        //applies lowJumpMultiplier
        else if (dotVelocityUp > 0 && !Input.GetButton("Jump") && !OnGround && !OnSteep)
        {
            _velocity += (lowJumpMultiplier - 1) * Time.deltaTime * gravity;
        }

        _body.velocity = _velocity;
        ClearState();
    }

    private void OnCollisionEnter(Collision other)
    {
        EvaluateCollision(other);
    }

    private void OnCollisionStay(Collision other)
    {
        EvaluateCollision(other);
    }

    #endregion

    #region Custom Public Functions

    /// <summary>
    /// Pauses the player's behaviour
    /// </summary>
    /// <param name="pause">Wether the player's behaviour is paused, or not</param>
    public void Pause(bool pause)
    {
        _state = pause ? PlayerState.Pause : PlayerState.Idle;
        if (!pause)
        {
            CheckDrop();
        }
    }

    /// <summary>
    /// Changes sensitivity of x-axis based on a percentage, going from 500f to 1000f
    /// </summary>
    /// <param name="sensitivityPercent">The percentage</param>
    public void ChangeSensitivityX(float sensitivityPercent)
    {
        mouseSensitivity = Vector2.right * (250f + 250f * sensitivityPercent) + Vector2.up * mouseSensitivity.y;
    }

    /// <summary>
    /// Changes sensitivity of y-axis based on a percentage, going from 500f to 1000f
    /// </summary>
    /// <param name="sensitivityPercent">The percentage</param>
    public void ChangeSensitivityY(float sensitivityPercent)
    {
        mouseSensitivity = Vector2.up * (250f + 250f * sensitivityPercent) + Vector2.right * mouseSensitivity.x;
    }

    #endregion

    #region Custom Private Functions

    private void AdjustRotationCam()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity.x * Time.deltaTime * (invertAxisX ? -1 : 1);
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity.y * Time.deltaTime * (invertAxisY ? -1 : 1);
        _rotationCam.x = Mathf.Clamp(_rotationCam.x - mouseY, -minAngleLook, maxAngleLook);
        _rotationCam.y = (_rotationCam.y + mouseX) % 360;
        cam.transform.localRotation = Quaternion.Euler(_rotationCam.x, _rotationCam.y, 0f);
    }

    private void AdjustRotationBody()
    {
        //todo smoothen rotation cam if change of gravity (see gravity cube for ex)

        //the rotation applied due to the gravity
        Quaternion gravityRot = Quaternion.FromToRotation(transform.up, _upAxis);
        //we move the body the number of angle the camera has turned then readjust the camera to 0
        //since fixedupdate comes before update, it is soon resolved
        //we use Vector3.up instead of _upAxis since we've already taken in the rotation of the body
        Quaternion yRot = Quaternion.AngleAxis(_rotationCam.y, Vector3.up);
        _body.MoveRotation(gravityRot * _body.rotation * yRot);
        _rotationCam.y = 0f;
    }

    private void AdjustVelocity()
    {
        float acceleration = OnGround
            ? Input.GetButton("Sprint") ? maxSprintAcceleration : maxAcceleration
            : maxAirAcceleration;
        float speed = OnGround && Input.GetButton("Sprint") ? maxSprintSpeed : maxSpeed;
        Vector3 xAxis = _rightAxis;
        Vector3 zAxis = _forwardAxis;
        
        xAxis = ProjectDirectionOnPlane(xAxis, _contactNormal);
        zAxis = ProjectDirectionOnPlane(zAxis, _contactNormal);

        Vector3 relativeVelocity = _velocity - _connectionVelocity;
        float currentX = Vector3.Dot(relativeVelocity, xAxis);
        float currentZ = Vector3.Dot(relativeVelocity, zAxis);

        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, _playerInput.x * speed, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, _playerInput.y * speed, maxSpeedChange);

        _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    private void AdjustCursor()
    {
        if (_grabbedItem != null)
        {
            Vector3 diff = _grabbedItem.transform.position - cam.transform.position;
            if (Vector3.Angle(diff, cam.transform.forward) > cam.fieldOfView)
            {
                Vector3 camForward = cam.transform.forward;
                Vector3 project = Vector3.ProjectOnPlane(diff, camForward);
                GameManager.Instance.UIManager.AdjustCursor(true,
                    Vector3.SignedAngle(cam.transform.up, project, camForward));
            }
            else
            {
                GameManager.Instance.UIManager.AdjustCursor(false, 0f);
            }
        }
    }

    private void Jump(Vector3 gravity)
    {
        //the jump is always directed in the upAxis, can be changed to the ground normal but more interesting for physics based games
        //jumpPhase describes the number of times the player has jumped
        Vector3 jumpDirection;
        /*if (OnGround)
        {
            jumpDirection = _contactNormal;
        }
        else if (OnSteep)
        {
            jumpDirection = _steepNormal;
            _jumpPhase = 0;
        }*/
        if (OnGround || OnSteep)
        {
            jumpDirection = _upAxis;
            if (OnSteep)
            {
                _jumpPhase = 0;
            }
        }
        else if (maxAirJumps > 0 && _jumpPhase <= maxAirJumps)
        {
            if (_jumpPhase == 0)
            {
                _jumpPhase = 1;
            }

            jumpDirection = _contactNormal;
        }
        else
        {
            return;
        }

        _stepsSinceLastJump = 0;
        _jumpPhase += 1;
        float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);

        jumpDirection = (jumpDirection + _upAxis).normalized;
        float alignedSpeed = Vector3.Dot(_velocity, jumpDirection);
        if (alignedSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }

        _velocity += jumpDirection * jumpSpeed;
        _velocity += jumpDirection * jumpSpeed;
    }

    private void EvaluateCollision(Collision collision)
    {
        int layer = collision.gameObject.layer;
        float minDot = GetMinDot(layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(_upAxis, normal);
            //checks if the player is on ground/steep/stairs with an angle lower than maxGroundAngle/maxStairsAngle
            if (upDot >= minDot)
            {
                _groundContactCount += 1;
                _contactNormal += normal;
                _connectedBody = collision.rigidbody;
            }
            else
            {
                if (upDot > -0.01f)
                {
                    _steepContactCount += 1;
                    _steepNormal += normal;
                    //if the player is not on ground, then it is connected to a wall/ceiling/steep
                    if (_groundContactCount == 0)
                    {
                        _connectedBody = collision.rigidbody;
                    }
                }
            }
        }
    }

    private void OnValidate()
    {
        _minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        _minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
    }

    private void UpdateState()
    {
        _stepsSinceLastGrounded += 1;
        _stepsSinceLastJump += 1;
        _velocity = _body.velocity;
        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            _stepsSinceLastGrounded = 0;
            if (_stepsSinceLastJump > 1)
            {
                _jumpPhase = 0;
            }

            if (_groundContactCount > 1)
            {
                _contactNormal.Normalize();
            }
        }
        else
        {
            _contactNormal = _upAxis;
        }

        if (_connectedBody)
        {
            if (_connectedBody.isKinematic || _connectedBody.mass >= _body.mass)
            {
                UpdateConnectionState();
            }
        }
    }

    private void ClearState()
    {
        _groundContactCount = _steepContactCount = 0;
        _contactNormal = _steepNormal = Vector3.zero;
        _connectionVelocity = Vector3.zero;
        _previousConnectedBody = _connectedBody;
        _connectedBody = null;
    }

    private void UpdateConnectionState()
    {
        if (_connectedBody == _previousConnectedBody)
        {
            Vector3 connectionMovement =
                _connectedBody.transform.TransformPoint(_connectionLocalPosition) -
                _connectionWorldPosition;
            _connectionVelocity = connectionMovement / Time.deltaTime;
        }

        _connectionWorldPosition = _body.position;
        _connectionLocalPosition = _connectedBody.transform.InverseTransformPoint(
            _connectionWorldPosition
        );
    }

    private void CheckInteraction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, maxDistanceInteractable,
                layerInteractable) && hit.collider.transform.CompareTag("Item"))
            {
                ItemInteract i = hit.collider.transform.GetComponent<ItemInteract>();
                if (i != null)
                {
                    i.Interact();
                }
            }
        }
    }

    private void CheckGrab()
    {
        if (Input.GetButtonDown("Grab") && _grabbedItem == null)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, maxDistanceInteractable,
                layerInteractable) && hit.transform.CompareTag("Item"))
            {
                ItemGrab g = hit.transform.GetComponent<ItemGrab>();
                if (g != null)
                {
                    g.Grab(cam.transform);
                    _grabbedItem = g;
                }
            }
        }
    }

    private void CheckDrop()
    {
        if (Input.GetButtonUp("Grab") && _grabbedItem != null)
        {
            Drop();
        }
    }

    private void Drop()
    {
        GameManager.Instance.UIManager.AdjustCursor(false, 0f);
        _grabbedItem.Drop(_body.velocity);
        _grabbedItem = null;
    }
    
    private bool SnapToGround()
    {
        if (_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <= 2)
        {
            return false;
        }

        float speed = _velocity.magnitude;
        if (speed > maxSnapSpeed)
        {
            return false;
        }

        if (!Physics.Raycast(_body.position, -_upAxis, out RaycastHit hit, probeDistance, probeMask,
            QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        float upDot = Vector3.Dot(_upAxis, hit.normal);
        if (upDot < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        _groundContactCount = 1;
        _contactNormal = hit.normal;
        float dot = Vector3.Dot(_velocity, hit.normal);
        if (dot > 0f)
        {
            _velocity = (_velocity - hit.normal * dot).normalized * speed;
        }

        _connectedBody = hit.rigidbody;
        return true;
    }

    private bool CheckSteepContacts()
    {
        if (_steepContactCount > 1)
        {
            _steepNormal.Normalize();
            float upDot = Vector3.Dot(_upAxis, _steepNormal);
            if (upDot >= _minGroundDotProduct)
            {
                _steepContactCount = 0;
                _groundContactCount = 1;
                _contactNormal = _steepNormal;
                return true;
            }
        }

        return false;
    }

    private Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    private float GetMinDot(int layer)
    {
        //using bit representation to check if the layer is part of the mask, or not
        return (stairsMask & (1 << layer)) == 0 ? _minGroundDotProduct : _minStairsDotProduct;
    }

    private void Throw(float percent)
    {
        if (_grabbedItem == null) return;
        _grabbedItem.Throw(percent * throwStrength);
        Drop();
        if (enablePushbackThrow)
        {
            _body.AddForce(-throwStrength * percent * cam.transform.forward.normalized);
        }

    }

    private IEnumerator Throwing(float a, float b, float speed)
    {
        GameManager.Instance.UIManager.LoadingFill(a);
        for (float t = 0; t < 1f; t += Time.deltaTime * speed)
        {
            if (Input.GetButtonUp("Throw"))
            {
                Throw(Mathf.Lerp(a, b, t));
                break;
            }

            GameManager.Instance.UIManager.LoadingFill(Mathf.Lerp(a, b, t));
            yield return null;
        }

        GameManager.Instance.UIManager.LoadingFill(0f);
        Throw(1f);
    }

    #endregion
}