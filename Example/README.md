# Unity player example

This optional example depends on the separate **HFSM** folder. Its scripts belong to the `FsmShowcase.Example` assembly and the `FsmShowcase.Examples.Player` namespace.

It demonstrates a Unity component owning the HFSM, a typed context, a factory and a hierarchical branch:

```text
Grounded
  Idle or Walk
Falling
```

## Import and test

1. Copy both `HFSM` and `Example` into your Unity project's `Assets` folder.
2. Wait for Unity to import and compile the scripts.
3. Create an empty GameObject and add **Player State Machine**.
4. Enter Play Mode and select that GameObject.
5. Change **Is Grounded** and **Move Input** in the Inspector to drive transitions.

No camera, Rigidbody, collider or input bindings are needed. Input and grounded status are manually supplied. The example calculates speeds and **does not move the GameObject**.

| Inspector action | Expected state/output |
| --- | --- |
| Start grounded with Move Input at 0 | `PlayerGroundedState > PlayerIdleState`; horizontal speed 0. |
| Set Move Input to 1 | `PlayerGroundedState > PlayerWalkState`; horizontal speed 5 with default settings. |
| Set Move Input to -1 | Remain in Walk; horizontal speed -5. |
| Set Move Input to 0 | Return to Idle; horizontal speed 0. |
| Uncheck Is Grounded | `PlayerFallingState`, without a child; vertical speed decreases each subsequent frame. |
| Check Is Grounded | Return to Grounded with Idle or Walk selected from current input; vertical speed resets to 0. |
| Disable the component | The branch exits and its display shows Stopped. |
| Enable the component | A fresh branch starts from current inputs. |

**Runtime state** fields are automatically overwritten. They show state names and movement outputs. `Stopped` and `None` are display labels used only by this example; they are not states or transition conditions. Play Mode Inspector changes normally revert when Play Mode ends.

## Responsibilities

- `PlayerStateMachine`: a `MonoBehaviour` that owns the context, machine and factory, supplies input values and displays runtime state.
- `PlayerContext`: input values, configuration and calculated speeds.
- `PlayerStateFactory`: a plain C# class that creates player states on transitions.
- `PlayerBaseState`: gives player states access to their factory.
- `PlayerGroundedState`: selects Idle or Walk on each entry and handles ground loss.
- `PlayerIdleState` and `PlayerWalkState`: grounded movement outputs and child transitions.
- `PlayerFallingState`: integrates gravity and handles landing.

The component creates dependencies in `Awake`, starts a root in `OnEnable`, ticks in `Update` and stops in `OnDisable`. It supplies `Time.deltaTime` through the context.

`SetInput(float horizontalInput, bool isGrounded)` allows another component to supply the example inputs. `CurrentState`, `HorizontalSpeed` and `VerticalSpeed` expose runtime outputs.

The factory creates new states on transitions, following the original implementation. The display refreshes state names when the active branch changes. These choices belong to the example, not the reusable skeleton.

Jump, collisions, actual movement, slopes, coyote time, acceleration and animations are outside this example's scope. A playable demo can be added later.
