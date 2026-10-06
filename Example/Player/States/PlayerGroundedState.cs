namespace FsmShowcase.Examples.Player
{
    public sealed class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(StateMachine<PlayerContext> machine,
            PlayerStateFactory factory) : base(machine, factory, true) { }

        public override void EnterState()
        {
            _ctx.VerticalSpeed = 0f;
        }

        public override void InitializeSubState()
        {
            SetSubState(_ctx.MoveInput == 0f ? _factory.Idle() : _factory.Walk());
        }

        public override void UpdateState() { CheckSwitchStates(); }
        public override void ExitState() { }

        public override void CheckSwitchStates()
        {
            if (!_ctx.IsGrounded)
                SwitchState(_factory.Falling());
        }
    }
}
