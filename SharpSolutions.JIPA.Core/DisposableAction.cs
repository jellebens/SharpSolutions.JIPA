using System;

namespace SharpSolutions.JIPA.Core
{
    internal class DisposableAction: IDisposable
    {
        private readonly Action action;
        private bool isDisposed;

        public DisposableAction(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            this.action = action;
        }

        public void Dispose()
        {
            if (isDisposed) return;
            action();
            isDisposed = true;
        }
    }
}