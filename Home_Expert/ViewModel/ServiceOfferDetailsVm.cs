using Home_Expert.Models;

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
        public int ServiceTypeId { get; set; }
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

        // Attachment
        public bool HasAttachment { get; set; }
        public string? AttachmentFileName { get; set; }

        // ✅ تعديل: جعلها خصائص قابلة للقراءة والكتابة (with setters)
        public string StatusKey { get; set; } = "pending";
        public bool CanSubmit { get; set; }


        // خصائص المطابخ (Kitchen)
        public KitchenMeasurement? KitchenMeasurement { get; set; }
        public KitchenCostEstimate? KitchenCostEstimate { get; set; }
        public List<KitchenExport>? KitchenExports { get; set; }

        // خصائص النقل (Moving)
        public MovingRequest? MovingRequest { get; set; }
        public List<MovingOffer>? MovingOffers { get; set; }
        public List<MovingStatusLog>? MovingStatusLogs { get; set; }

        // تحديد نوع الخدمة
        public bool IsKitchenService { get; set; }
        public bool IsMovingService { get; set; }
    }
}