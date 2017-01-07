namespace MultipleDesktop.Mvc.Desktop
{
    public static class PropertyChangingEventArgsExtensions
    {
        public static bool ShouldRollback(this System.ComponentModel.PropertyChangingEventArgs args)
        {
            return (args as PropertyRollBackChangesEventArgs)?.Exception != null;
        }
    }
}
