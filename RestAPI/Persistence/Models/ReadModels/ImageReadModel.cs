using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models.ReadModels
{
    public class ImageReadModel
    {
        public Guid ImageId { get; set; }

        public Guid CampgroundId { get; set; }

        public string Url { get; set; }

    }
}
