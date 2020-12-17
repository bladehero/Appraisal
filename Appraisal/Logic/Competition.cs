using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Appraisal.Logic
{
    [DataContract]
    public class Competition
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ObservableCollection<Team> Teams { get; set; }

        [DataMember]
        public ObservableCollection<Judge> Judges { get; set; }

        [DataMember]
        public ObservableCollection<Nomination> Nominations { get; set; }

        public Competition()
        {
            Teams = new ObservableCollection<Team>();
            Judges = new ObservableCollection<Judge>();
            Nominations = new ObservableCollection<Nomination>();
        }

        public void Save(string path)
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(Competition));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, this);
            }
        }
        public static Competition Open(string path)
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(Competition));

            Competition competition = null;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                competition = (Competition)jsonFormatter.ReadObject(fs);
            }

            return competition;
        }
    }
}
