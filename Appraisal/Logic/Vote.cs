namespace Appraisal.Logic
{
    public class Vote
    {
        public Judge Judge { get; set; }
        public Nomination Nomination { get; set; }
        public Team Team { get; set; }
        public int Value { get; set; }
    }
}
