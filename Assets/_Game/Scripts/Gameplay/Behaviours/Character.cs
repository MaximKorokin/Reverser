using System.Linq;
using UnityEngine;
using VContainer;

public class Character : MonoBehaviourBase
{
    [SerializeField]
    private Collider2D _groundCheck;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _jumpHeight;
    [Space]
    [SerializeField]
    private Transform _pickableParent;
    [SerializeField]
    private float _pickRange;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private bool _isGrounded;
    private float _currentHorizontalVelocity;
    private float _lastNonZeroHorizontalVelocity;

    [Inject]
    private void Construct(PlayerInputHandler playerInputHandler)
    {
        playerInputHandler.Enable();
        playerInputHandler.JumpInputRecieved += OnJumpInputRecieved;
        playerInputHandler.MoveInputRecieved += OnMoveInputRecieved;
        playerInputHandler.InteractInputRecieved += OnInteractInputRecieved;

        OnDestroying += () =>
        {
            playerInputHandler.Disable();
            playerInputHandler.JumpInputRecieved -= OnJumpInputRecieved;
            playerInputHandler.MoveInputRecieved -= OnMoveInputRecieved;
            playerInputHandler.InteractInputRecieved -= OnInteractInputRecieved;
        };
    }

    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetRequiredComponent<Rigidbody2D>();
        _animator = GetRequiredComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        _isGrounded = _groundCheck.IsTouchingLayers(LayerMask.GetMask(Constants.Layer.Ground.ToString()));
        _rigidbody.linearVelocityX = _currentHorizontalVelocity;
    }

    private void OnJumpInputRecieved()
    {
        if (_isGrounded)
        {
            _rigidbody.linearVelocityY = MyMath.CalculateVerticalVelocity(_jumpHeight, Physics2D.gravity.y, _rigidbody.gravityScale);
        }
    }

    private void OnMoveInputRecieved(float horizontalInput)
    {
        _currentHorizontalVelocity = horizontalInput * _moveSpeed;
        if (_currentHorizontalVelocity != 0)
        {
            _lastNonZeroHorizontalVelocity = _currentHorizontalVelocity;
        }
        _animator.SetInteger(Constants.AnimatorKey.Speed.ToString(), (int)(horizontalInput * 100));
    }

    private void OnInteractInputRecieved()
    {
        if (_pickableParent.childCount == 0)
        {
            var hits = Physics2D.RaycastAll((Vector2)transform.position - Vector2.right * _pickRange, Vector2.right, _pickRange * 2);
            var pickables = hits.Select(x => x.collider.GetComponent<Pickable>()).SkipNull().ToArray();

            if (pickables.Length == 0) return;

            var nearestPickable = pickables.MinBy(x => Vector2.Distance(x.transform.position, transform.position));
            nearestPickable.TryPick(_pickableParent);
        }
        else
        {
            if (!_pickableParent.GetChild(0).TryGetComponent<Pickable>(out var pickable))
            {
                Logger.Error($"Didn't expect to see non pickable in {_pickableParent.name}: {_pickableParent.GetChild(0).name}");
            }
            var placeDelta = (_lastNonZeroHorizontalVelocity > 0 ? 1 : -1) * Vector2.right;
            pickable.TryPlace(placeDelta);
        }
    }
}
