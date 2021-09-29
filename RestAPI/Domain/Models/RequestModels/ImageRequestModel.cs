using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RequestModels
{
    public class ImageRequestModel
    {
        public Guid ImageId { get; set; }

        public Guid CampgroundId { get; set; }

        public string Url { get; set; }
    }
}
