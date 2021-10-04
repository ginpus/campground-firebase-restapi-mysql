using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ResponseModels
{
    public class ShortCampgroundResponseModel
    {
        public Guid CampgroundId { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string DefaultImageUrl { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
