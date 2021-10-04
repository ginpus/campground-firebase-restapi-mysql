using System.Text.Json.Serialization;

namespace Domain.Client.Models.ResponseModels
{
    public class ClientSignInUserResponse
    {
        [JsonPropertyName("localId")]
        public string LocalId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        /*[JsonPropertyName("displayName")]
        public string DisplayName { get; set; }*/

        [JsonPropertyName("idToken")]
        public string IdToken { get; set; }

        /*[JsonPropertyName("registered")]
        public bool Registered { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expiresIn")]
        public int ExpiresIn { get; set; }

        public override string ToString()
        {
            return $"localId: {LocalId} \n email: {Email} \n displayName:{DisplayName} \n idToken: {IdToken} \n registered: {Registered} \n refreshToken: {RefreshToken} \n expiresIn: {ExpiresIn}";
        }*/
    }
}
