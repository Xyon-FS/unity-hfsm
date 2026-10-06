namespace FsmShowcase.Examples.Player
{
    public sealed class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(StateMachine<PlayerContext> machine,
            PlayerStateFactory factory) : base(machine, factory, false) { }

        public override void EnterState() { _ctx.HorizontalSpeed = 0f; }
        public override void ExitState() { }
        public override void InitializeSubState() { }
        public override void UpdateState() { CheckSwitchStates(); }

        public override void CheckSwitchStates()
        {
            if (_ctx.MoveInput != 0f)
                SwitchState(_factory.Walk());
        }
    }
}
