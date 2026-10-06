namespace FsmShowcase.Examples.Player
{
    public sealed class PlayerStateFactory
    {
        private readonly StateMachine<PlayerContext> _machine;

        public PlayerStateFactory(StateMachine<PlayerContext> machine)
        {
            _machine = machine;
        }

        public PlayerBaseState Grounded() { return new PlayerGroundedState(_machine, this); }
        public PlayerBaseState Idle() { return new PlayerIdleState(_machine, this); }
        public PlayerBaseState Walk() { return new PlayerWalkState(_machine, this); }
        public PlayerBaseState Falling() { return new PlayerFallingState(_machine, this); }
    }
}
