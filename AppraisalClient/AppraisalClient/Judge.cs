using System.Runtime.Serialization;

namespace AppraisalClient
{
    [DataContract]
    public class Judge
    {
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public bool IsBusy { get; set; }

        public Judge()
        {

        }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {MiddleName}";
        }
    }
}
