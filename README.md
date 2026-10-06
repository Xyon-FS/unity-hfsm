# Unity Hierarchical FSM Showcase

A small hierarchical finite state machine extracted from the player movement architecture of **Pump Down the Flame**.

```text
HFSM/                        Reusable skeleton; import this folder to use the HFSM
  Runtime/
    BaseState.cs
    StateMachine.cs
    FsmShowcase.HFSM.asmdef
  README.md                  Setup, API and lifecycle rules
Example/                     Optional Unity usage example; depends on HFSM
  Player/
    PlayerStateMachine.cs
    PlayerContext.cs
    PlayerBaseState.cs
    PlayerStateFactory.cs
    States/
  FsmShowcase.Example.asmdef
  README.md                  Inspector testing instructions
```

Copy **HFSM** into your Unity project's `Assets` folder to use the skeleton. Copy **Example** as well to try the included player example.

The assemblies enforce a single dependency direction: **Example → HFSM**. The HFSM has no dependency on the example, player behavior, Inspector displays or Unity engine APIs. It is driven by your own Unity component.

Target Unity version: **2022.3**. This repository contains importable scripts, without scenes, playable demos or external packages. Unity generates asset metadata when importing the folders.

See [HFSM documentation](HFSM/README.md) and [example instructions](Example/README.md).

Licensed under the [MIT License](LICENSE).
