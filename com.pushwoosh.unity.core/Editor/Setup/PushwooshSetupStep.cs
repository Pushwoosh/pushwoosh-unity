#if UNITY_EDITOR
namespace PushwooshSDK.Editor
{
    public abstract class PushwooshSetupStep
    {
        public abstract string Summary { get; }
        public abstract string Details { get; }
        public abstract bool IsRequired { get; }

        public bool IsStepCompleted
        {
            get
            {
                if (!_shouldCheckForCompletion)
                    return _isComplete;

                _isComplete = _getIsStepCompleted();
                _shouldCheckForCompletion = false;
                return _isComplete;
            }
        }

        public void RunStep()
        {
            if (IsStepCompleted)
                return;

            _runStep();
            _shouldCheckForCompletion = true;
        }

        protected abstract bool _getIsStepCompleted();
        protected abstract void _runStep();

        private bool _isComplete = false;
        protected bool _shouldCheckForCompletion = true;
    }
}
#endif
