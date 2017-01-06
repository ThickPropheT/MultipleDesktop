namespace MultipleDesktop.Mvc.Desktop
{
    public interface IBackground
    {
        string Path { get; set; }

        Fit Fit { get; }
    }
}
