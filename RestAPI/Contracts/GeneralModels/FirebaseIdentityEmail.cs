using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contracts.GeneralModels
{
    public class FirebaseIdentityEmail
    {
        [JsonPropertyName("email")]

        public IEnumerable<string> Email { get; set; }
    }
}
