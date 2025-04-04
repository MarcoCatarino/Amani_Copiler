namespace Core.Models
{
    public class Symbol
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsUsed { get; set; } = false;

        public Symbol(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
