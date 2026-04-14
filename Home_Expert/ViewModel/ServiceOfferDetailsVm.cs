namespace Home_Expert.ViewModel
{
    public class ServiceOfferDetailsVm
    {
        public int OfferId { get; set; }
        public int RequestId { get; set; }
        public decimal? PriceEstimate { get; set; }
        public bool InspectionRequired { get; set; }
        public int OfferStatusId { get; set; }
        public string OfferStatusAr { get; set; } = "";
        public string OfferStatusEn { get; set; } = "";

        // Service
        public string ServiceNameAr { get; set; } = "";
        public string ServiceNameEn { get; set; } = "";
        public string ServiceTypeAr { get; set; } = "";
        public string ServiceTypeEn { get; set; } = "";
        public string ServiceCatAr { get; set; } = "";
        public string ServiceCatEn { get; set; } = "";

        // Request
        public DateTime? PreferredTime { get; set; }
        public int RequestStatusId { get; set; }
        public string RequestStatusAr { get; set; } = "";
        public string RequestStatusEn { get; set; } = "";

        // Customer
        public string CustomerNameAr { get; set; } = "";
        public string CustomerNameEn { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public string CustomerCity { get; set; } = "";
        public string CustomerArea { get; set; } = "";
        public double? CustomerLatitude { get; set; }
        public double? CustomerLongitude { get; set; }

        // Helpers
        public string StatusKey => OfferStatusId switch
        {
            86 => "pending",
            87 => "submitted",
            88 => "accepted",
            89 => "rejected",
            _ => "other"
        };
        public bool CanSubmit => OfferStatusId == 86 || OfferStatusId == 87;
    }

}
