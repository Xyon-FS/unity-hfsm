namespace FsmShowcase.Examples.Player
{
    public sealed class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(StateMachine<PlayerContext> machine,
            PlayerStateFactory factory) : base(machine, factory, false) { }

        public override void EnterState() { ApplyMovement(); }
        public override void ExitState() { }
        public override void InitializeSubState() { }

        public override void UpdateState()
        {
            ApplyMovement();
            CheckSwitchStates();
        }

        public override void CheckSwitchStates()
        {
            if (_ctx.MoveInput == 0f)
                SwitchState(_factory.Idle());
        }

        private void ApplyMovement()
        {
            _ctx.HorizontalSpeed = _ctx.MoveInput * _ctx.MoveSpeed;
        }
    }
}
