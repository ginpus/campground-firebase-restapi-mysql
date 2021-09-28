namespace RestAPI.Models.RequestModels
{
    public class CampgroundCreateorUpdateRequest
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }
    }
}
