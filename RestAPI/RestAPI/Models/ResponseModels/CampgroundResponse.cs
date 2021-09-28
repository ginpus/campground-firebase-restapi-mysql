using System;

namespace RestAPI.Models.ResponseModels
{
    public class CampgroundResponse
    {
        public Guid CampgroundId { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
