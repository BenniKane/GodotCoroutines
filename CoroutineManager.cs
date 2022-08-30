using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Coroutines
{
	public static class TimeTracker
	{
		public static float DeltaTime { get; set; }
	}

	public class CoroutineManager : Node
	{
		/// Do not use. Use Coroutine.Create instead.
		public static CoroutineManager Instance { get; private set; }
		
		private List<Coroutine> coroutines = new List<Coroutine>();

		private bool _executingCoroutines;

		private Coroutine _executingCoroutine;
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				QueueFree();
			}    
		}

		public Coroutine AddCoroutine(IEnumerator root, Action onComplete = null)
		{
			Coroutine coroutine = new Coroutine(root, onComplete);

			if (_executingCoroutine != null)
			{
				_executingCoroutine.Add(coroutine);
			}
			else
			{
				_executingCoroutine = coroutine;
				coroutine.Initialize();
				_executingCoroutine = null;
				coroutines.Add(coroutine);
			}


			
			return coroutine;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			_executingCoroutines = true;

			TimeTracker.DeltaTime = delta;

			List<Coroutine> completedCoroutines = new List<Coroutine>();

			for(var index = 0; index < coroutines.Count; index++)
			{
				_executingCoroutine = coroutines[index];
				_executingCoroutine.Execute();

				if (_executingCoroutine.IsCompleted)
				{
					_executingCoroutine.Finished();
					completedCoroutines.Add(_executingCoroutine);			
				}

				_executingCoroutine = null;
			}

			coroutines = coroutines.Except(completedCoroutines).ToList();
		}

		private bool ExecuteCoroutine(IEnumerator coroutine)
		{
			if (coroutine.Current == null)
			{
				return true;	
			}

			if (coroutine.Current is IEnumerator nestedCoroutine)
			{
				return ExecuteCoroutine(nestedCoroutine);
			}
			else
			{
				return coroutine.MoveNext();
			}
		}
	}

}
