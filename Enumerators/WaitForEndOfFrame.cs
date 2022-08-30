
using System.Collections;

namespace Utilities.Coroutines
{
    public class WaitForEndOfFrame : IEnumerator
    {
        public object Current => null;

        public bool MoveNext()
        {
            return true;
        }

        public void Reset()
        {
            
        }
    }
}