using System;

namespace Contracts.ResponseModels
{
    public class ImageResponse
    {
        public Guid ImageId { get; set; }

        public Guid CampgroundId { get; set; }

        public string Url { get; set; }
    }
}
