# HFSM skeleton

Import this entire folder into `Assets` to use the hierarchical finite state machine. `FsmShowcase.HFSM.asmdef` defines a standalone assembly without engine references. Namespace: `FsmShowcase`.

## Configure and use

1. Define your context: a class holding the data and dependencies needed by your states.
2. Derive state classes from `BaseState<YourContext>`.
3. Pass `true` to the base constructor for root states and `false` for child states.
4. In your Unity component, create the context and `StateMachine<YourContext>` in `Awake`.
5. Supply initial context data and call `StartState(yourInitialRoot)` in `OnEnable`.
6. Supply current context data and call `UpdateStates()` in `Update`, or `FixedUpdate` if your behavior requires physics ticks.
7. Call `Stop()` in `OnDisable` to exit the active branch.

If you define states in a custom assembly, add an assembly reference to `FsmShowcase.HFSM`. Scripts in Unity's default assembly can use it directly.

Only your Unity component is attached to a GameObject. States and the machine are ordinary C# objects. A factory is optional: keep it alongside your own states when it helps organize their construction.

## State hooks

| Hook | Responsibility |
| --- | --- |
| `EnterState` | Initialize behavior on activation. |
| `UpdateState` | Execute one tick of behavior. |
| `ExitState` | Clean up on deactivation. |
| `CheckSwitchStates` | Express transition conditions; call explicitly from `UpdateState`. |
| `InitializeSubState` | Select the initial child with `SetSubState`, or leave empty for a leaf. |

Constructors store dependencies. Child selection happens on entry through `InitializeSubState`, which runs on every entry, including when a state instance is reused.

Use the protected `SwitchState(nextState)` method for transitions. Return after switching when more code follows: a transition does not terminate the calling C# method.

`_ctx` exposes the typed context to a state. `_machine` exposes its owning machine. `CurrentState`, `CurrentSubState`, `CurrentSuperState` and `IsActive` allow the owning application to inspect the active hierarchy.

## Lifecycle rules

- Each active state has at most one active child. Deeper nesting is supported.
- Entry runs parent first, then child. Exit runs child first, then parent.
- Updates run parent first. After a branch changes, its replacement enters immediately and starts updating on the next tick.
- Root transitions replace the root. Child transitions preserve ancestors.
- New branch references are assigned before entry hooks execute.
- Targets must belong to the same machine, be inactive and unattached, and have the correct root/child role. Invalid targets are rejected before exit.
- Switching to the same instance does nothing.
- Updating a stopped machine does nothing. It can be started again with an inactive root.
- Start, stop, update and transitions must not be triggered from entry, exit or child initialization. Recursive updates are rejected.

The HFSM is synchronous and intended to be driven on Unity's main thread. Lifecycle hooks should not throw: exceptions propagate without rollback of a partially completed lifecycle.

The skeleton performs no input polling, physics, movement, presentation or logging. Context data, state construction and state-specific behavior belong to the application using it.

For a concrete Unity implementation, optionally import the separate `Example` folder and follow its README.
