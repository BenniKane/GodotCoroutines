using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Utilities.Coroutines
{
    public class Coroutine
    {
        public static Coroutine Create(IEnumerator root, Action onComplete = null)
		{
			return CoroutineManager.Instance.AddCoroutine(root, onComplete);
		}

        private Coroutine _subroutine;
        
        private IEnumerator _routine;

        private Action _onComplete;

        public bool IsCompleted { get; private set; }

        public Coroutine(IEnumerator root, Action onComplete = null)
        {
            _routine = root;
            _onComplete = onComplete;
        }

        public void Execute()
        {
            if (IsCompleted)
                return;

            if (_subroutine != null)
            {
                _subroutine.Execute();
                
                if (_subroutine.IsCompleted)
                {
                    _subroutine.Finished();
                    _subroutine = null;
                }
            }
            else
            {
                if (_routine.Current is IEnumerator enumerator)
                {
                    if (enumerator.MoveNext())
                    {
                        if (!_routine.MoveNext())
                        {
                            IsCompleted = true;
                        }
                    }
                }
                else if (!_routine.MoveNext())
                {
                    IsCompleted = true;
                }
            }
        }

        public void Initialize()
        {
            _routine.MoveNext();
        }

        public void Add(Coroutine subroutine)
        {
            if (_subroutine != null)
            {
                _subroutine.Add(subroutine);
            }
            else
            {
                _subroutine = subroutine;
                _subroutine.Initialize();
            }
        }

        public void Cancel()
        {
            GD.Print("Coroutine Canceled!");
            IsCompleted = true;
        }

        public void Finished()
        {
            _onComplete?.Invoke();
        }
    }
}