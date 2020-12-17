namespace Appraisal.Logic
{
    public class Nomination
    {
        public string Name { get; set; }
        public int MinMark { get; set; }
        public int MaxMark { get; set; }

        public override string ToString()
        {
            return $"{Name} ({MinMark} ... {MaxMark})";
        }
    }
}
