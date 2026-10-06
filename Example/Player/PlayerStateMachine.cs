using UnityEngine;

namespace FsmShowcase.Examples.Player
{
    // Inspector-driven example. Input and grounded status are supplied manually;
    // it demonstrates state behavior without a movement controller or a scene.
    [DisallowMultipleComponent]
    public sealed class PlayerStateMachine : MonoBehaviour
    {
        [Header("Example inputs (editable in Play Mode)")]
        [SerializeField] private bool _isGrounded = true;
        [SerializeField, Range(-1f, 1f)] private float _moveInput;

        [Header("Movement values")]
        [SerializeField, Min(0f)] private float _moveSpeed = 5f;
        [SerializeField] private float _gravity = -20f;

        [Header("Runtime state (updated automatically)")]
        [SerializeField] private string _activeRoot = "Stopped";
        [SerializeField] private string _activeSubState = "None";
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private float _verticalSpeed;

        private PlayerContext _context;
        private StateMachine<PlayerContext> _machine;
        private PlayerStateFactory _states;
        private BaseState<PlayerContext> _displayedRoot;
        private BaseState<PlayerContext> _displayedChild;

        public BaseState<PlayerContext> CurrentState => _machine?.CurrentState;
        public float HorizontalSpeed => _context?.HorizontalSpeed ?? 0f;
        public float VerticalSpeed => _context?.VerticalSpeed ?? 0f;

        private void Awake()
        {
            _context = new PlayerContext();
            _machine = new StateMachine<PlayerContext>(_context);
            _states = new PlayerStateFactory(_machine);
        }

        private void OnEnable()
        {
            SyncContext();
            _machine.StartState(_isGrounded ? _states.Grounded() : _states.Falling());
            RefreshInspector();
        }

        private void Update()
        {
            SyncContext();
            _machine.UpdateStates();
            RefreshInspector();
        }

        private void OnDisable()
        {
            if (_machine == null)
                return;

            _machine.Stop();
            _context.HorizontalSpeed = 0f;
            _context.VerticalSpeed = 0f;
            _displayedRoot = null;
            _displayedChild = null;
            _activeRoot = "Stopped";
            _activeSubState = "None";
            _horizontalSpeed = 0f;
            _verticalSpeed = 0f;
        }

        public void SetInput(float horizontalInput, bool isGrounded)
        {
            _moveInput = Mathf.Clamp(horizontalInput, -1f, 1f);
            _isGrounded = isGrounded;
        }

        private void SyncContext()
        {
            _context.IsGrounded = _isGrounded;
            _context.MoveInput = Mathf.Clamp(_moveInput, -1f, 1f);
            _context.MoveSpeed = _moveSpeed;
            _context.Gravity = _gravity;
            _context.DeltaTime = Time.deltaTime;
        }

        private void RefreshInspector()
        {
            var root = _machine.CurrentState;
            var child = root?.CurrentSubState;

            if (_displayedRoot != root)
            {
                _displayedRoot = root;
                _activeRoot = root == null ? "Stopped" : root.GetType().Name;
            }
            if (_displayedChild != child)
            {
                _displayedChild = child;
                _activeSubState = child == null ? "None" : child.GetType().Name;
            }

            _horizontalSpeed = _context.HorizontalSpeed;
            _verticalSpeed = _context.VerticalSpeed;
        }
    }
}
