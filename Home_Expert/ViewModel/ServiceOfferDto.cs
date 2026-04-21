namespace Home_Expert.ViewModel
{
    public class ServiceOfferDto
    {
        public int OfferId { get; set; }
        public int RequestId { get; set; }
        public string ServiceNameAr { get; set; } = "";
        public string ServiceNameEn { get; set; } = "";
        public string ServiceTypeAr { get; set; } = "";
        public string ServiceTypeEn { get; set; } = "";
        public DateTime? PreferredTime { get; set; }
        public decimal? PriceEstimate { get; set; }
        public bool InspectionRequired { get; set; }
        public int OfferStatusId { get; set; }
        public string OfferStatusAr { get; set; } = "";
        public string OfferStatusEn { get; set; } = "";
        public int RequestStatusId { get; set; }
        public string? RequestStatusAr { get; set; }
        public string? RequestStatusEn { get; set; }

        public string CustomerNameAr { get; set; } = "";
        public string CustomerNameEn { get; set; } = "";
        public IFormFile? AttachmentFile { get; set; }
        public bool CanEditOffer { get; set; }
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
