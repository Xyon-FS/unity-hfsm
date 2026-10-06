using System;

namespace FsmShowcase
{
    public abstract class BaseState<TContext> where TContext : class
    {
        protected readonly TContext _ctx;
        protected readonly StateMachine<TContext> _machine;
        private bool _initializingSubState;

        public bool IsRootState { get; }
        public bool IsActive { get; private set; }
        public BaseState<TContext> CurrentSubState { get; private set; }
        public BaseState<TContext> CurrentSuperState { get; private set; }

        protected BaseState(StateMachine<TContext> machine, bool isRootState)
        {
            _machine = machine ?? throw new ArgumentNullException(nameof(machine));
            _ctx = machine.Context;
            IsRootState = isRootState;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubState();

        protected void SwitchState(BaseState<TContext> newState)
        {
            _machine.SwitchState(this, newState);
        }

        protected void SetSubState(BaseState<TContext> newSubState)
        {
            if (!_initializingSubState || CurrentSubState != null)
                throw new InvalidOperationException("Set one child during InitializeSubState only.");

            ValidateTarget(newSubState, false);
            AttachSubState(newSubState);
        }

        internal void ValidateTarget(BaseState<TContext> target, bool requireRoot)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (target._machine != _machine || target.IsRootState != requireRoot ||
                target.IsActive || target.CurrentSuperState != null || target == this)
                throw new InvalidOperationException("The target must be an unattached, inactive state of the same machine and level.");
        }

        internal bool BelongsTo(StateMachine<TContext> machine)
        {
            return _machine == machine;
        }

        internal void AttachSubState(BaseState<TContext> child)
        {
            if (CurrentSubState != null)
                CurrentSubState.CurrentSuperState = null;
            CurrentSubState = child;
            child.CurrentSuperState = this;
        }

        internal void EnterStates()
        {
            IsActive = true;
            EnterState();
            _initializingSubState = true;
            try
            {
                InitializeSubState();
            }
            finally
            {
                _initializingSubState = false;
            }
            CurrentSubState?.EnterStates();
        }

        internal void UpdateStates()
        {
            int version = _machine.TransitionVersion;
            UpdateState();

            if (IsActive && version == _machine.TransitionVersion)
                CurrentSubState?.UpdateStates();
        }

        internal void ExitStates()
        {
            if (CurrentSubState != null)
            {
                CurrentSubState.ExitStates();
                CurrentSubState.CurrentSuperState = null;
                CurrentSubState = null;
            }
            ExitState();
            IsActive = false;
        }
    }
}
