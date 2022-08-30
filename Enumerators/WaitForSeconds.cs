using Godot;
using System.Collections;

namespace Utilities.Coroutines
{
    public class WaitForSeconds : IEnumerator
    {
        private float _duration;
        private float _currentTime;
        private bool _didTimeUpdate;

        public WaitForSeconds(float duration)
        {
            _duration = duration;
        }

        public object Current => null;

        public bool MoveNext()
        {
            // if (!_didTimeUpdate)
            // {
            //     _didTimeUpdate = true;
            // }
            _currentTime += TimeTracker.DeltaTime;

            if (_currentTime < _duration)
                return false;

            return true;
        }

        public void Reset()
        {
            _didTimeUpdate = false;
        }
    }
}