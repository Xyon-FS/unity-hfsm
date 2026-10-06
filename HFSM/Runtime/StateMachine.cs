using System;

namespace FsmShowcase
{
    public sealed class StateMachine<TContext> where TContext : class
    {
        private bool _changingState;
        private bool _updating;

        public TContext Context { get; }
        public BaseState<TContext> CurrentState { get; private set; }
        internal int TransitionVersion { get; private set; }

        public StateMachine(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void StartState(BaseState<TContext> initialState)
        {
            EnsureOutsideLifecycle();
            if (CurrentState != null || _updating)
                throw new InvalidOperationException("Stop the machine before starting it again.");
            if (initialState == null)
                throw new ArgumentNullException(nameof(initialState));
            if (!initialState.BelongsTo(this) || !initialState.IsRootState || initialState.IsActive)
                throw new InvalidOperationException("Start with an inactive root belonging to this machine.");

            _changingState = true;
            try
            {
                CurrentState = initialState;
                TransitionVersion++;
                initialState.EnterStates();
            }
            finally { _changingState = false; }
        }

        public void UpdateStates()
        {
            EnsureOutsideLifecycle();
            if (_updating)
                throw new InvalidOperationException("Recursive updates are not supported.");
            _updating = true;
            try { CurrentState?.UpdateStates(); }
            finally { _updating = false; }
        }

        public void Stop()
        {
            EnsureOutsideLifecycle();
            if (_updating)
                throw new InvalidOperationException("Stop outside the update callback.");
            if (CurrentState == null)
                return;

            _changingState = true;
            try
            {
                CurrentState.ExitStates();
                CurrentState = null;
                TransitionVersion++;
            }
            finally { _changingState = false; }
        }

        internal void SwitchState(BaseState<TContext> source, BaseState<TContext> target)
        {
            EnsureOutsideLifecycle();
            if (!source.IsActive)
                throw new InvalidOperationException("Only an active state can request a transition.");
            if (source == target)
                return;
            source.ValidateTarget(target, source.IsRootState);

            BaseState<TContext> parent = source.CurrentSuperState;
            _changingState = true;
            try
            {
                source.ExitStates();
                if (source.IsRootState)
                    CurrentState = target;
                else
                    parent.AttachSubState(target);

                TransitionVersion++;
                target.EnterStates();
            }
            finally { _changingState = false; }
        }

        private void EnsureOutsideLifecycle()
        {
            if (_changingState)
                throw new InvalidOperationException("Do not start, stop, update or switch during entry/exit hooks.");
        }
    }
}
