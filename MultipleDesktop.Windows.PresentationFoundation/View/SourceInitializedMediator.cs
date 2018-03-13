using System;
using System.Windows;

namespace MultipleDesktop.Windows.PresentationFoundation.View
{
    public class SourceInitializedMediator
    {
        public static SourceInitializedMediator Mediate(Window target, Action<Window> callback)
        {
            if (PresentationSource.FromVisual(target) != null)
            {
                callback(target);
                return new SourceInitializedMediator(target);
            }

            return new SourceInitializedMediator(target, callback);
        }

        private readonly Action<Window> _callback;

        public Window Target { get; }
        public bool MediationComplete { get; private set; }

        private SourceInitializedMediator(Window target)
        {
            Target = target;
            MediationComplete = true;
        }

        private SourceInitializedMediator(Window target, Action<Window> callback)
        {
            Target = target;
            _callback = callback;

            target.SourceInitialized += Target_SourceInitialized;
        }

        private void Target_SourceInitialized(object sender, EventArgs e)
        {
            Target.SourceInitialized -= Target_SourceInitialized;

            _callback(Target);

            MediationComplete = true;
        }
    }
}
