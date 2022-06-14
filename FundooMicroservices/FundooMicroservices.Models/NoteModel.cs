using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Models
{
    public class NoteModel
    {
        [JsonProperty("id")]
        public string NoteId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("colour")]
        public string Colour { get; set; }

        [JsonProperty("date")]
        public DateTime CreationDate { get; set; }


        [JsonProperty("pin")]
        public bool IsPin { get; set; }

        [JsonProperty("archive")]
        public bool IsArchive { get; set; }

        [JsonProperty("trash")]
        public bool IsTrash { get; set; }

        public List<string> Collaborators { get; set; }
    }
}
