﻿using Enemies.StateMachines.States;

namespace Enemies.StateMachines
{
    public class PersonStateMachine
    {
        public PersonStateMachine(CharacterState defaultState)
        {
            CurrentState = defaultState;
        }

        public CharacterState CurrentState { get; set; }

        public void ChangeState(CharacterState newState)
        {
            if (!newState.CanEnter() || !CurrentState.IsCompleted) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void ForcedChangeState(CharacterState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void Execute() => CurrentState.Execute();
        public void FixedExecute() => CurrentState.FixedExecute();
    }
}