using System.Text.Json.Serialization;

namespace Vaja4NalogaTest
{

    public class Zaloznik
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Knjiga> Knjige { get; set; }
    }

}