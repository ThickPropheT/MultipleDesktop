namespace MultipleDesktop.Mvc.Desktop
{
    public class Background : IBackground
    {
        public string Path { get; set; }

        public Fit Fit { get; set; }

        public Background(string path, Fit fit)
        {
            Path = path;
            Fit = fit;
        }

        public override bool Equals(object obj)
        {
            var background = obj as Background;

            if (background == null)
                return false;

            return Path.Equals(background.Path) && Fit.Equals(background.Fit);
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode() + Fit.GetHashCode();
        }
    }
}
