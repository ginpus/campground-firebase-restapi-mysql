using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contracts.GeneralModels
{
    public class FirebaseIdentity
    {
        [JsonPropertyName("identities")]
        public FirebaseIdentityEmail Identities { get; set; }

        [JsonPropertyName("sign_in_provider")]
        public string SignInProvider { get; set; }
    }
}
