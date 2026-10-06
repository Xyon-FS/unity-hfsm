namespace FsmShowcase.Examples.Player
{
    public sealed class PlayerFallingState : PlayerBaseState
    {
        public PlayerFallingState(StateMachine<PlayerContext> machine,
            PlayerStateFactory factory) : base(machine, factory, true) { }

        public override void EnterState()
        {
            _ctx.HorizontalSpeed = _ctx.MoveInput * _ctx.MoveSpeed;
        }

        public override void ExitState() { }
        public override void InitializeSubState() { }

        public override void UpdateState()
        {
            if (_ctx.IsGrounded)
            {
                CheckSwitchStates();
                return;
            }
            _ctx.HorizontalSpeed = _ctx.MoveInput * _ctx.MoveSpeed;
            _ctx.VerticalSpeed += _ctx.Gravity * _ctx.DeltaTime;
        }

        public override void CheckSwitchStates()
        {
            if (_ctx.IsGrounded)
                SwitchState(_factory.Grounded());
        }
    }
}
