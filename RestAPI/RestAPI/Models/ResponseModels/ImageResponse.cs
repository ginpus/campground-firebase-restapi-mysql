using System;

namespace RestAPI.Models.ResponseModels
{
    public class ImageResponse
    {
        public Guid ImageId { get; set; }

        public Guid CampgroundId { get; set; }

        public string Url { get; set; }
    }
}
