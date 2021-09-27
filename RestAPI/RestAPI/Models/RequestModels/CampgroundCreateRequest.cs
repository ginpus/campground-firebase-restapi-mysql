namespace RestAPI.Models.RequestModels
{
    public class CampgroundCreateRequest
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }
    }
}
