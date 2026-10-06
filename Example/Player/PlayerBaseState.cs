namespace FsmShowcase.Examples.Player
{
    public abstract class PlayerBaseState : BaseState<PlayerContext>
    {
        protected readonly PlayerStateFactory _factory;

        protected PlayerBaseState(StateMachine<PlayerContext> machine,
            PlayerStateFactory factory, bool isRootState) : base(machine, isRootState)
        {
            _factory = factory;
        }
    }
}
