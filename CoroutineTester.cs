using System.Collections;
using System.Threading.Tasks;
using Godot;

namespace Utilities.Coroutines.Testing
{
	public class CoroutineTester : Spatial
	{
		private Coroutine cameraSpinCoroutine;

		public override void _Ready()
		{
			Coroutine.Create(Execute());

			cameraSpinCoroutine = Coroutine.Create(RotateCamera(7.5f, "Camera Root"));
			
			Coroutine.Create(RotateCamera(5f, "Subobject"), () => 
			{
				GD.Print("Arrow Subobject Rotate On Complete Callback. Canceling Camera Spin.");
				cameraSpinCoroutine.Cancel();
			});
		}

		private IEnumerator RotateCamera(float totalTime, string nodeName)
		{
			float currentTime = 0f;
			Spatial cameraMount = GetNode<Spatial>(nodeName);
			
			while (currentTime < totalTime)
			{
				currentTime += TimeTracker.DeltaTime;

				cameraMount.Rotation = new Vector3(0f, Mathf.Deg2Rad(Mathf.Lerp(0f, 360f, currentTime / totalTime)), 0f);

				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator Execute()
		{
			GD.Print("Execute Step 1 - Yields to Secondary Execution");

			yield return CoroutineManager.Instance.AddCoroutine(SecondaryExecute());
			
			for(var i = 0; i < 10; i++)
			{
				GD.Print($"Execute Repeating: {i} - Each Yielding to End of Frame");
				yield return new WaitForEndOfFrame();
			}

			GD.Print("Execute Step 2 - Yields to WaitForSeconds(3)");

			yield return new WaitForSeconds(3f);
			
			GD.Print("Execute Step 3 - Yields to WaitForSeconds(5)");
			
			yield return new WaitForSeconds(5f);

			GD.Print("Execute Finished - No More Yields");
		}

		private IEnumerator SecondaryExecute()
		{
			GD.Print("Secondary Execute 1 - Yields to WaitForSeconds(2)");

			yield return new WaitForSeconds(2f);
		
			GD.Print("Secondary Execute 2 - Yields to WaitForSeconds(2)");
			
			yield return new WaitForSeconds(2f);

			GD.Print("Secondary Execute Finished - No More Yields");
		}
	}
}
