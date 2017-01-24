namespace MultipleDesktop.Mvc.Configuration
{
    public struct FilePath
    {
        private string _value;

        public bool HasValue => !string.IsNullOrEmpty(_value);
        public string Value => _value ?? string.Empty;

        public FilePath(string value)
        {
            _value = value;
        }

        public static implicit operator FilePath(string s)
        {
            return new FilePath(s);
        }

        public static implicit operator string(FilePath p)
        {
            return p.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
            {
                return Equals(Value, (string)obj);
            }

            if (obj is FilePath)
            {
                return Equals(Value, ((FilePath)obj).Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
