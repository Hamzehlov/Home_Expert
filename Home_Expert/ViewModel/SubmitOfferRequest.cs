namespace Home_Expert.ViewModel
{
    public class SubmitOfferRequest
    {
        public int OfferId { get; set; }
        public decimal PriceEstimate { get; set; }
        public bool InspectionRequired { get; set; }
    }
}
