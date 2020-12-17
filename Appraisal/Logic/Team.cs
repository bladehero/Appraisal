namespace Appraisal.Logic
{
    public class Team
    {
        public string Name { get; set; }
        public string Alias { get; set; }

        public override string ToString()
        {
            return $"{Name} — \"{Alias}\"";
        }
    }
}
