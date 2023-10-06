
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Craft
{
    public class CraftStateSequence
    {
        public enum PartID
        {
            Head = 0, Body = 1,Eye = 2, Mouth = 3, Background = 4, Monster = 5 
        }
        private LinkedList<Sequence> _craftStateSequence = new LinkedList<Sequence>();
        private LinkedListNode<Sequence> _currentState;

        public CraftStateSequence(
            Sequence first, Sequence second,
            Sequence third, Sequence fourth, Sequence fifth, Sequence sixth)
        {
            _craftStateSequence.AddLast(first);
            _craftStateSequence.AddLast(second);
            _craftStateSequence.AddLast(third);
            _craftStateSequence.AddLast(fourth);
            _craftStateSequence.AddLast(fifth);
            _craftStateSequence.AddLast(sixth);
            _currentState = _craftStateSequence.First;
        }


        public Sequence CurrentState => _currentState.Value;

        public void Next()
        {
            _currentState.Value.Exit();
            _currentState = _currentState.Next;
            _currentState.Value.Enter();
        }

        public void Before()
        {
            _currentState.Value.Exit();
            _currentState = _currentState.Previous;
            if (_currentState.Previous == null)
                _currentState.Value.Enter();
        }

        public void SetCurrentSequence(Sequence newState)
        {
            _currentState.Value.Exit();
            _currentState = _craftStateSequence.Find(newState);
            _currentState.Value.Enter();
        }


        public LinkedListNode<Sequence> First
        {
            get{
                return _craftStateSequence.First;
            }
        }

        public bool IsLastState
        {
            get
            {
                return _currentState.Next == null;
            }
        }
    }
}