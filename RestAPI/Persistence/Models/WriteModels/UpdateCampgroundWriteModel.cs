using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models.WriteModels
{
    public class UpdateCampgroundWriteModel
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }
    }
}
