using Home_Expert.Models;

namespace Home_Expert.Resources
{
    public static class SharedResourcesKeys
    {
        // Project Info
        public const string Project_Title = "Project_Title";

        // Services
        public const string Service_Moving = "Service_Moving";
        public const string Service_Sales = "Service_Sales";
        public const string Service_Installation = "Service_Installation";

        // ServicesCard

        public const string Srv_Services = "Srv_Services";
        public const string Srv_Services_Desc = "Srv_Services_Desc";

        // Hero Section
        public const string Hero_Title = "Hero_Title";
        public const string Hero_Description = "Hero_Description";
        public const string Hero_Trust_Text = "Hero_Trust_Text";

        // Footer
        public const string Footer_Copyright = "Footer_Copyright";
        public const string Footer_Privacy = "Footer_Privacy";
        public const string Footer_Terms = "Footer_Terms";

        // Form
        public const string Form_Title = "Form_Title";
        public const string Form_Subtitle = "Form_Subtitle";

        // Steps
        public const string Step_Details = "Step_Details";
        public const string Step_Verification = "Step_Verification";

        // Labels - General
        public const string Label_FullName = "Label_FullName";
        public const string Label_CompanyName = "Label_CompanyName";
        public const string Label_Email = "Label_Email";
        public const string Label_Phone = "Label_Phone";
        public const string Label_Password = "Label_Password";

        // Labels - Bilingual Name
        public const string Label_FirstNameAr = "Label_FirstNameAr";
        public const string Label_FirstNameEn = "Label_FirstNameEn";

        // Labels - Bilingual Company
        public const string Label_CompanyNameAr = "Label_CompanyNameAr";
        public const string Label_CompanyNameEn = "Label_CompanyNameEn";

        // Labels - Bilingual Description
        public const string Label_DescriptionAr = "Label_DescriptionAr";
        public const string Label_DescriptionEn = "Label_DescriptionEn";

        // Labels - Bilingual Showroom Address
        public const string Label_ShowroomAddressAr = "Label_ShowroomAddressAr";
        public const string Label_ShowroomAddressEn = "Label_ShowroomAddressEn";

        // Labels - File Uploads
        public const string Label_ShowroomImage = "Label_ShowroomImage";
        public const string Label_CommercialFile = "Label_CommercialFile";
        public const string Label_WorkLicense = "Label_WorkLicense";

        // Placeholders - General
        public const string Placeholder_FullName = "Placeholder_FullName";
        public const string Placeholder_CompanyName = "Placeholder_CompanyName";
        public const string Placeholder_Email = "Placeholder_Email";
        public const string Placeholder_Phone = "Placeholder_Phone";

        // Placeholders - Bilingual Name
        public const string Placeholder_FirstNameAr = "Placeholder_FirstNameAr";
        public const string Placeholder_FirstNameEn = "Placeholder_FirstNameEn";

        // Placeholders - Bilingual Company
        public const string Placeholder_CompanyNameAr = "Placeholder_CompanyNameAr";
        public const string Placeholder_CompanyNameEn = "Placeholder_CompanyNameEn";

        // Placeholders - Bilingual Description
        public const string Placeholder_DescriptionAr = "Placeholder_DescriptionAr";
        public const string Placeholder_DescriptionEn = "Placeholder_DescriptionEn";

        // Placeholders - Bilingual Showroom Address
        public const string Placeholder_ShowroomAddressAr = "Placeholder_ShowroomAddressAr";
        public const string Placeholder_ShowroomAddressEn = "Placeholder_ShowroomAddressEn";

        // Other
        public const string Optional = "Optional";
        public const string Terms_Agreement = "Terms_Agreement";
        public const string Button_CreateAccount = "Button_CreateAccount";
        public const string SignIn_Link = "SignIn_Link";

        // Login
        public const string Login_Title = "Login_Title";
        public const string Login_Subtitle = "Login_Subtitle";
        public const string Login_RememberMe = "Login_RememberMe";
        public const string Login_ForgotPassword = "Login_ForgotPassword";
        public const string Button_Login = "Button_Login";
        public const string SignUp_Link = "SignUp_Link";
        public const string Login_Or = "Login_Or";
        public const string Login_Google = "Login_Google";
        public const string Login_Microsoft = "Login_Microsoft";

        // API Messages
        public const string Message_OTPSent = "Message_OTPSent";
        public const string Message_EmailExists = "Message_EmailExists";
        public const string Message_InvalidData = "Message_InvalidData";
        public const string Message_SessionExpired = "Message_SessionExpired";
        public const string Message_MaxAttemptsReached = "Message_MaxAttemptsReached";
        public const string Message_OTPIncorrect = "Message_OTPIncorrect";
        public const string Message_RegistrationSuccess = "Message_RegistrationSuccess";
        public const string Message_UserCreationFailed = "Message_UserCreationFailed";
        public const string Message_OTPResent = "Message_OTPResent";
        public const string Message_ServerError = "Message_ServerError";

        // OTP Verification
        public const string OTP_Title = "OTP_Title";
        public const string OTP_Subtitle = "OTP_Subtitle";
        public const string OTP_ResendIn = "OTP_ResendIn";
        public const string OTP_Resend = "OTP_Resend";
        public const string Button_Verify = "Button_Verify";
        public const string Back_To_Registration = "Back_To_Registration";
        public const string OTP_TimerText = "OTP_TimerText";
        public const string OTP_VerifyButton = "OTP_VerifyButton";
        public const string OTP_ResendButton = "OTP_ResendButton";
        public const string OTP_BackButton = "OTP_BackButton";

        // Service Type
        public const string Label_ServiceType = "Label_ServiceType";
        public const string Placeholder_ServiceType = "Placeholder_ServiceType";
        public const string Service_Furniture = "Service_Furniture";
        public const string Service_KitchenSales = "Service_KitchenSales";
        public const string Service_KitchenInstall = "Service_KitchenInstall";

        // Other Fields
        public const string Label_YearsExperience = "Label_YearsExperience";
        public const string Placeholder_YearsExperience = "Placeholder_YearsExperience";
        public const string Label_Description = "Label_Description";
        public const string Placeholder_Description = "Placeholder_Description";
        public const string Label_Logo = "Label_Logo";
        public const string Logo_Helper = "Logo_Helper";
        public const string Password_Helper = "Password_Helper";

        // Validation Errors - Name
        public const string Error_FirstNameRequired = "Error_FirstNameRequired";
        public const string Error_FirstNameLength = "Error_FirstNameLength";
        public const string Error_FirstArNameRequired = "Error_FirstArNameRequired";
        public const string Error_FirsEntNameRequired = "Error_FirsEntNameRequired";
        public const string Error_LastNameRequired = "Error_LastNameRequired";
        public const string Error_LastNameLength = "Error_LastNameLength";

        // Validation Errors - Auth
        public const string Error_EmailRequired = "Error_EmailRequired";
        public const string Error_EmailInvalid = "Error_EmailInvalid";
        public const string Error_PasswordRequired = "Error_PasswordRequired";
        public const string Error_PasswordLength = "Error_PasswordLength";
        public const string Error_PasswordWeak = "Error_PasswordWeak";
        public const string Error_ConfirmPasswordRequired = "Error_ConfirmPasswordRequired";
        public const string Error_PasswordMismatch = "Error_PasswordMismatch";
        public const string Error_PhoneLength = "Error_PhoneLength";
        public const string Error_PhoneInvalid = "Error_PhoneInvalid";

        // Validation Errors - Company
        public const string Error_CompanyNameRequired = "Error_CompanyNameRequired";
        public const string Error_CompanyNameLength = "Error_CompanyNameLength";
        public const string Error_CompanyNameArRequired = "Error_CompanyNameArRequired";
        public const string Error_CompanyNameEnRequired = "Error_CompanyNameEnRequired";

        // Validation Errors - Other
        public const string Error_DescriptionLength = "Error_DescriptionLength";
        public const string Error_YearsExperienceRange = "Error_YearsExperienceRange";
        public const string Error_ServiceTypeRequired = "Error_ServiceTypeRequired";
        public const string Error_ShowroomAddressLength = "Error_ShowroomAddressLength";

        // Login Errors
        public const string Error_AccountNotFound = "Error_AccountNotFound";
        public const string Error_WrongPassword = "Error_WrongPassword";
        public const string Error_AccountLocked = "Error_AccountLocked";
        public const string Error_InvalidCredentials = "Error_InvalidCredentials";

        // Pending Messages
        public const string Message_AccountNotVerified = "Message_AccountNotVerified";
        public const string Message_AccountUnderReview = "Message_AccountUnderReview";

        public const string Error_PhoneDigitsOnly = "Error_PhoneDigitsOnly";

        // Email Exists Messages
        public const string Message_EmailExistsAsCustomer = "Message_EmailExistsAsCustomer";
        public const string Message_EmailExistsAsVendor = "Message_EmailExistsAsVendor";
        public const string Message_EmailExistsAsAdmin = "Message_EmailExistsAsAdmin";

        // Login Role Errors
        public const string Error_CustomerCannotLoginAsVendor = "Error_CustomerCannotLoginAsVendor";
        public const string Error_AdminLoginRestricted = "Error_AdminLoginRestricted";
        public const string Error_NotVendorAccount = "Error_NotVendorAccount";

        // ── عام ──
        public const string Dashboard = "Dashboard";
        public const string Search = "Search";
        public const string Admin = "Admin";
        public const string Logout = "Logout";

        // ── Sidebar ──
        public const string Nav_Main = "Nav_Main";
        public const string Nav_Management = "Nav_Management";
        public const string Nav_System = "Nav_System";
        public const string Orders = "Orders";
        public const string Services = "Services";
        public const string Companies = "Companies";
        public const string Users = "Users";
        public const string Messages = "Messages";
        public const string Settings = "Settings";
        public const string SuperAdmin = "SuperAdmin";
        public const string SystemManager = "SystemManager";

        // ── Welcome Banner ──
        public const string Welcome_Title = "Welcome_Title";
        public const string Welcome_Subtitle = "Welcome_Subtitle";
        public const string Welcome_Orders = "Welcome_Orders";
        public const string Welcome_Satisfaction = "Welcome_Satisfaction";
        public const string Welcome_Active = "Welcome_Active";
        public const string Monthly_Orders = "Monthly_Orders";
        public const string Satisfaction_Rate = "Satisfaction_Rate";
        public const string Active_Now = "Active_Now";

        // ── Section Titles ──
        public const string Main_Services = "Main_Services";
        public const string Recent_Orders = "Recent_Orders";
        public const string Top_Services = "Top_Services";
        public const string View_All = "View_All";
        public const string View_Details = "View_Details";

        // ── Service Cards ──
        public const string Srv_Active = "Srv_Active";
        public const string Srv_New = "Srv_New";
        public const string Srv_TotalOrders = "Srv_TotalOrders";
        public const string Srv_ActiveOrders = "Srv_ActiveOrders";
        public const string Srv_Sales = "Srv_Sales";
        public const string Srv_MonthProgress = "Srv_MonthProgress";

        // ── Service Names ──
        public const string Srv_FurnitureSale = "Srv_FurnitureSale";
        public const string Srv_FurnitureMoving = "Srv_FurnitureMoving";
        public const string Srv_KitchenSale = "Srv_KitchenSale";
        public const string Srv_KitchenInstall = "Srv_KitchenInstall";
        public const string Srv_FurnitureInstall = "Srv_FurnitureInstall";
        public const string Srv_Maintenance = "Srv_Maintenance";

        // ── Service Descriptions ──
        public const string Srv_FurnitureSale_Desc = "Srv_FurnitureSale_Desc";
        public const string Srv_FurnitureMoving_Desc = "Srv_FurnitureMoving_Desc";
        public const string Srv_KitchenSale_Desc = "Srv_KitchenSale_Desc";
        public const string Srv_KitchenInstall_Desc = "Srv_KitchenInstall_Desc";
        public const string Srv_FurnitureInstall_Desc = "Srv_FurnitureInstall_Desc";
        public const string Srv_Maintenance_Desc = "Srv_Maintenance_Desc";

        // ── Orders Table ──
        public const string Col_Service = "Col_Service";
        public const string Col_Customer = "Col_Customer";
        public const string Col_Status = "Col_Status";
        public const string Col_Amount = "Col_Amount";

        // ── Status ──
        public const string Status_InProgress = "Status_InProgress";
        public const string Status_Done = "Status_Done";
        public const string Status_Pending = "Status_Pending";
        public const string Status_Cancelled = "Status_Cancelled";  // ── Filters ──
        public const string Filter_AllStatus = "Filter_AllStatus";
        public const string Filter_AllTime = "Filter_AllTime";
        public const string Filter_Today = "Filter_Today";
        public const string Filter_ThisWeek = "Filter_ThisWeek";
        public const string Filter_ThisMonth = "Filter_ThisMonth";
        public const string Export = "Export";

        // ── Table columns ──
        public const string Col_OrderNo = "Col_OrderNo";
        public const string Col_Date = "Col_Date";
        public const string Col_Actions = "Col_Actions";
        public const string Total_Orders = "Total_Orders";
        public const string Showing = "Showing";
        public const string Of = "Of";

        // ── Nav / Actions ──
        public const string Back = "Back";
        public const string Details = "Details";
        public const string Edit = "Edit";
        public const string Delete = "Delete";
        public const string Cancel = "Cancel";
        public const string Save_Order = "Save_Order";
        public const string Save_Draft = "Save_Draft";
        public const string Save_Changes = "Save_Changes";
        public const string Delete_Confirm = "Delete_Confirm";
        public const string Add_New_Order = "Add_New_Order";

        // ── Page subtitles ──
        public const string Orders_FurnitureSale = "Orders_FurnitureSale";
        public const string Orders_FurnitureSale_Sub = "Orders_FurnitureSale_Sub";
        public const string Add_Order_Sub = "Add_Order_Sub";
        public const string Edit_Order = "Edit_Order";
        public const string Edit_Order_Sub = "Edit_Order_Sub";
        public const string Order_Details = "Order_Details";

        // ── Order form fields ──
        public const string Service_Type = "Service_Type";
        public const string Select_Service = "Select_Service";
        public const string Product_Name = "Product_Name";
        public const string Product_Name_Placeholder = "Product_Name_Placeholder";
        public const string Product_Sofa = "Product_Sofa";
        public const string Quantity = "Quantity";
        public const string Amount_SAR = "Amount_SAR";
        public const string Currency = "Currency";
        public const string Order_Date = "Order_Date";
        public const string Delivery_Date = "Delivery_Date";
        public const string Payment_Method = "Payment_Method";
        public const string Bank_Transfer = "Bank_Transfer";
        public const string Cash = "Cash";
        public const string Credit_Card = "Credit_Card";
        public const string Mada = "Mada";
        public const string Notes = "Notes";
        public const string Notes_Placeholder = "Notes_Placeholder";
        public const string Product_Images = "Product_Images";
        public const string Upload_Label = "Upload_Label";
        public const string Upload_Hint = "Upload_Hint";

        // ── Customer fields ──
        public const string Customer_Name = "Customer_Name";
        public const string Customer_Info = "Customer_Info";
        public const string Customer_Since = "Customer_Since";
        public const string Active_Customer = "Active_Customer";
        public const string Total_Customer_Orders = "Total_Customer_Orders";
        public const string Full_Name = "Full_Name";
        public const string Phone = "Phone";
        public const string Email = "Email";
        public const string City = "City";
        public const string City_Riyadh = "City_Riyadh";
        public const string City_Jeddah = "City_Jeddah";
        public const string City_Dammam = "City_Dammam";
        public const string City_Mecca = "City_Mecca";
        public const string Address = "Address";
        public const string Address_Placeholder = "Address_Placeholder";
        public const string Sample_Address = "Sample_Address";
        public const string Sample_Notes = "Sample_Notes";

        // ── Detail page ──
        public const string Order_Info = "Order_Info";
        public const string Edit_Order_Info = "Edit_Order_Info";
        public const string Edit_Customer_Info = "Edit_Customer_Info";
        public const string Rating = "Rating";
        public const string Order_Timeline = "Order_Timeline";
        public const string Timeline_Received = "Timeline_Received";
        public const string Timeline_Paid = "Timeline_Paid";
        public const string Timeline_Shipping = "Timeline_Shipping";
        public const string Timeline_Delivery = "Timeline_Delivery";
        public const string Expected = "Expected";
        public const string Quick_Actions = "Quick_Actions";
        public const string Mark_Complete = "Mark_Complete";
        public const string Print_Invoice = "Print_Invoice";
        public const string Send_Customer = "Send_Customer";
        public const string Cancel_Order = "Cancel_Order";

        // ── Sidebar tips ──
        public const string Input_Tips = "Input_Tips";
        public const string Tip_Required = "Tip_Required";
        public const string Tip_Phone = "Tip_Phone";
        public const string Tip_Notes = "Tip_Notes";
        public const string Tip_Images = "Tip_Images";
        public const string Tip_Notify = "Tip_Notify";
        public const string Quick_Stats = "Quick_Stats";
        public const string Today_Orders = "Today_Orders";
        public const string Month_Orders = "Month_Orders";
        public const string Top_Service = "Top_Service";

        // ── Edit sidebar ──
        public const string Order_Meta = "Order_Meta";
        public const string Created_At = "Created_At";
        public const string Last_Modified = "Last_Modified";
        public const string Today = "Today";
        public const string Warnings = "Warnings";
        public const string Warning_Complete = "Warning_Complete";
        public const string Warning_Amount = "Warning_Amount";
        public const string Warning_Log = "Warning_Log";
        public const string Edit_Warning = "Edit_Warning";
        // Pending Vendors
        public const string PendingVendors_Title = "PendingVendors_Title";
        public const string PendingVendors_Count = "PendingVendors_Count";
        public const string PendingVendors_Description = "PendingVendors_Description";
        public const string PendingVendors_ViewAll = "PendingVendors_ViewAll";

        // Vendors Table
        public const string Vendors_CompanyName = "Vendors_CompanyName";
        public const string Vendors_Email = "Vendors_Email";
        public const string Vendors_Phone = "Vendors_Phone";
        public const string Vendors_ServiceType = "Vendors_ServiceType";
        public const string Vendors_Experience = "Vendors_Experience";
        public const string Vendors_SubmittedDate = "Vendors_SubmittedDate";
        public const string Vendors_Status = "Vendors_Status";
        public const string Vendors_Actions = "Vendors_Actions";
        public const string Vendors_Accept = "Vendors_Accept";
        public const string Vendors_Reject = "Vendors_Reject";
        public const string Vendors_ViewDetails = "Vendors_ViewDetails";

        // Vendor Details
        public const string Vendor_Details_Title = "Vendor_Details_Title";
        public const string Vendor_Logo = "Vendor_Logo";
        public const string Vendor_Description = "Vendor_Description";
        public const string Vendor_ShowroomAddress = "Vendor_ShowroomAddress";
        public const string Vendor_ShowroomImage = "Vendor_ShowroomImage";
        public const string Vendor_Documents = "Vendor_Documents";
        public const string Vendor_CommercialReg = "Vendor_CommercialReg";
        public const string Vendor_WorkLicense = "Vendor_WorkLicense";

        // Rejection
        public const string Rejection_Title = "Rejection_Title";
        public const string Rejection_Reason = "Rejection_Reason";
        public const string Rejection_ReasonAr = "Rejection_ReasonAr";
        public const string Rejection_ReasonEn = "Rejection_ReasonEn";
        public const string Rejection_Submit = "Rejection_Submit";
        public const string Rejection_Cancel = "Rejection_Cancel";

        // Messages
        public const string Message_VendorAccepted = "Message_VendorAccepted";
        public const string Message_VendorRejected = "Message_VendorRejected";
        public const string Message_EmailSent = "Message_EmailSent";
        public const string Message_ConfirmAccept = "Message_ConfirmAccept";
        public const string Message_ConfirmReject = "Message_ConfirmReject";

        // Status
        public const string Status_Approved = "Status_Approved";
        public const string Status_Rejected = "Status_Rejected";


        // ── Dashboard Cards ──
        // ── Products Card ──
        public const string Products_Title = "Products_Title";
        public const string Products_Desc = "Products_Desc";
        public const string Products_Active = "Products_Active";
        public const string Products_Total = "Products_Total";
        public const string Products_New = "Products_New";
        public const string Products_Sales = "Products_Sales";

        // ── Ads Card ──
        public const string Ads_Title = "Ads_Title";
        public const string Ads_Desc = "Ads_Desc";
        public const string Ads_Active = "Ads_Active";
        public const string Ads_Running = "Ads_Running";
        public const string Ads_Pending = "Ads_Pending";
        public const string Ads_Views = "Ads_Views";

        // ── Look Up Card ──
        public const string Lookup_Title = "Lookup_Title";
        public const string Lookup_Desc = "Lookup_Desc";
        public const string Lookup = "Lookup";                  // للbadge
        public const string Lookup_LastQuery = "Lookup_LastQuery";
        public const string Start_Lookup = "Start_Lookup";     // زر البداية





        // ── Ads indix ──
        public const string Ads_Description = "Ads_Description";       // وصف الصفحة
        public const string Ads_Total = "Ads_Total";                   // إجمالي الإعلانات
        public const string Ads_Inactive = "Ads_Inactive";             // الإعلانات غير النشطة
        public const string Ads_AddNew = "Ads_AddNew";                 // زر إضافة إعلان جديد
        public const string Ads_Text = "Ads_Text";                     // نص الإعلان
        public const string Ads_Image = "Ads_Image";                   // صورة الإعلان
        public const string Ads_Status = "Ads_Status";                 // حالة الإعلان
        public const string Ads_Actions = "Ads_Actions";               // عمود الإجراءات
        public const string ToggleStatus = "ToggleStatus";             // زر تفعيل/تعطيل



        // ── Ads Form Fields ──
        public const string Add_New_Ad = "Add_New_Ad";                 // عنوان الصفحة "إضافة إعلان جديد"
        public const string Add_Ad_Sub = "Add_Ad_Sub";                 // وصف الصفحة "يمكنك إضافة إعلان جديد هنا"

        public const string Ad_Info = "Ad_Info";                       // قسم معلومات الإعلان
        public const string Ad_Title_Ar = "Ad_Title_Ar";               // عنوان الإعلان بالعربي
        public const string Ad_Title_En = "Ad_Title_En";               // عنوان الإعلان بالإنجليزي
        public const string Ad_Text_Ar = "Ad_Text_Ar";                 // نص الإعلان بالعربي
        public const string Ad_Text_En = "Ad_Text_En";                 // نص الإعلان بالإنجليزي
        public const string Ad_Image = "Ad_Image";                     // عنوان قسم رفع صورة الإعلان

        public const string Ad_Title_Ar_Placeholder = "Ad_Title_Ar_Placeholder"; // placeholder للعنوان بالعربي
        public const string Ad_Title_En_Placeholder = "Ad_Title_En_Placeholder"; // placeholder للعنوان بالإنجليزي
        public const string Ad_Text_Ar_Placeholder = "Ad_Text_Ar_Placeholder";   // placeholder للنص بالعربي
        public const string Ad_Text_En_Placeholder = "Ad_Text_En_Placeholder";   // placeholder للنص بالإنجليزي         
        public const string Save_Ad = "Save_Ad";
        public const string Ads = "Ads";   // للإشارة العامة إلى صفحة الإعلانات
        public const string Edit_Ad = "Edit_Ad";         // عنوان الصفحة "تعديل الإعلان"
        public const string Edit_Ad_Sub = "Edit_Ad_Sub"; // وصف الصفحة "يمكنك تعديل بيانات الإعلان هنا"



        // ── Services index ──
        public const string Services_Title = "Services_Title";             // عنوان صفحة الخدمات
        public const string Services_Description = "Services_Description"; // وصف صفحة الخدمات
        public const string Services_Total = "Services_Total";             // إجمالي الخدمات
        public const string Services_Active = "Services_Active";           // الخدمات النشطة
        public const string Services_Inactive = "Services_Inactive";       // الخدمات غير النشطة
        public const string Services_AddNew = "Services_AddNew";           // زر إضافة خدمة جديد
        public const string Services_Name = "Services_Name";               // اسم الخدمة
        public const string Services_Type = "Services_Type";               // نوع الخدمة
        public const string Services_Category = "Services_Category";       // تصنيف الخدمة
        public const string Services_Status = "Services_Status";           // حالة الخدمة
        public const string Services_Actions = "Services_Actions";         // عمود الإجراءات للخدمات
        public const string ToggleServiceStatus = "ToggleServiceStatus";
        public const string Services_Image = "Services_Image";             // عمود صورة الخدمة



        // ── Services Form Fields ──
        public const string Services_Info = "Services_Info";
        public const string Services_NameAr = "Services_NameAr";
        public const string Services_NameAr_Placeholder = "Services_NameAr_Placeholder";
        public const string Services_NameEn = "Services_NameEn";
        public const string Services_NameEn_Placeholder = "Services_NameEn_Placeholder";
        public const string Select_Type = "Select_Type";
        public const string Select_Category = "Select_Category";
        public const string Save = "Save";
        public const string Services_Edit = "Services_Edit";
        public const string Total_Services = "Total_Services";


        // ── Products index ──
       
        public const string Products_Description = "Products_Description";   // وصف الصفح
        public const string Products_Inactive = "Products_Inactive";         // المنتجات غير النشطة
        public const string Products_AddNew = "Products_AddNew";             // زر إضافة منتج جديد
        public const string Products_Name = "Products_Name";                 // اسم المنتج
        public const string Products_NameAr = "Products_NameAr";             // اسم المنتج بالعربية
        public const string Products_NameEn = "Products_NameEn";             // اسم المنتج بالإنجليزية
        public const string Products_DescriptionField = "Products_DescriptionField"; // حقل الوصف
        public const string Products_PriceRange = "Products_PriceRange";     // نطاق السعر
        public const string Products_Image = "Products_Image";               // عمود الصور
        public const string Products_Status = "Products_Status";             // حالة المنتج
        public const string Products_Actions = "Products_Actions";           // عمود الإجراءات
        public const string Products_Edit = "Products_Edit";                 // زر تعديل
        public const string Products_Delete = "Products_Delete";             // زر حذف
        public const string ToggleProductStatus = "ToggleProductStatus";    // زر تفعيل/تعطيل
        public const string Products_ActiveBadge = "Products_ActiveBadge";   // badge للمنتج نشط
        public const string Products_InactiveBadge = "Products_InactiveBadge"; // badge للمنتج غير نشط
                                                                               // ── Products Related ──
        public const string Products_Company = "Products_Company"; // اسم الشركة/البائع للمنتج











        // Products  Form Fields ──
        public const string Products_NameAr_Placeholder = "Products_NameAr_Placeholder"; // هذا المفتاح مفقود
        public const string Products_NameEn_Placeholder = "Products_NameEn_Placeholder"; // هذا المفتاح مفقود
        public const string Products_DescriptionField_Placeholder = "Products_DescriptionField_Placeholder";
        public const string Products_Category = "Products_Category";
        public const string Min = "Min";
        public const string Max = "Max";
        public const string Products_Info = "Products_Info"; // قسم معلومات المنتج
        public const string Total_Products = "Total_Products";







        // ── Company Types index ──
        public const string CompanyTypes_Title = "CompanyTypes_Title";               // عنوان الصفحة
        public const string CompanyTypes_Description = "CompanyTypes_Description";   // وصف الصفحة
        public const string CompanyTypes_Total = "CompanyTypes_Total";               // إجمالي الأنواع
        public const string CompanyTypes_Active = "CompanyTypes_Active";             // النشطة
        public const string CompanyTypes_Inactive = "CompanyTypes_Inactive";         // غير النشطة
        public const string CompanyTypes_AddNew = "CompanyTypes_AddNew";             // زر إضافة نوع جديد
        public const string CompanyTypes_Name = "CompanyTypes_Name";                 // اسم النوع
        public const string CompanyTypes_DescriptionField = "CompanyTypes_DescriptionField"; // الوصف
        public const string CompanyTypes_Status = "CompanyTypes_Status";             // حالة النوع
        public const string CompanyTypes_Actions = "CompanyTypes_Actions";           // عمود الإجراءات
        public const string CompanyTypes_ActiveBadge = "CompanyTypes_ActiveBadge";   // badge نشط
        public const string CompanyTypes_InactiveBadge = "CompanyTypes_InactiveBadge"; // badge غير نشط


        // Company Types   Form Fields ──
        public const string CompanyTypes_NameAr = "CompanyTypes_NameAr";
        public const string CompanyTypes_NameAr_Placeholder = "CompanyTypes_NameAr_Placeholder";
        public const string CompanyTypes_NameEn = "CompanyTypes_NameEn";
        public const string CompanyTypes_NameEn_Placeholder = "CompanyTypes_NameEn_Placeholder";
        public const string CompanyTypes_Image = "CompanyTypes_Image"; // صورة النوع
        public const string Current_Image = "Current_Image";        // النص للصورة الحالية
        public const string CompanyTypes_Edit = "CompanyTypes_Edit"; // النص لزر تعديل نوع الشركة



        // ── Services Page Keys ──
        public const string Services_Add = "Services_Add";                       // زر إضافة خدمة جديد
        public const string AddService = "AddService";                            // نص زر إضافة خدمة
        public const string AddService_Description = "AddService_Description";   // وصف صفحة إضافة خدمة
        public const string AvailableServices = "AvailableServices";             // نص عدد الخدمات الكلي
        public const string Active = "Active";                                   // حالة نشط
        public const string Inactive = "Inactive";                               // حالة غير نشط
        public const string Service = "Service";                                 // عمود اسم الخدمة
        public const string Category = "Category";                               // عمود تصنيف الخدمة
        public const string Type = "Type";                                       // عمود نوع الخدمة
        public const string Status = "Status";                                   // عمود الحالة
        public const string Actions = "Actions";                                 // عمود الإجراءات
        public const string Services_Add_Description = "Services_Add_Description"; // وصف صفحة إضافة خدمة
        public const string Services_Available = "Services_Available";             // نص عدد الخدمات المتاحة






        // Resource Keys  كارد
        public const string CompanyServices_Title = "CompanyServices_Title";         // اسم الكارد
        public const string CompanyServices_Desc = "CompanyServices_Desc";           // وصف الكارد
        public const string Srv_TotalServices = "Srv_TotalServices";                 // إجمالي الخدمات
        public const string Srv_ActiveServices = "Srv_ActiveServices";               // الخدمات النشطة
        public const string Srv_ServiceSales = "Srv_ServiceSales";                   // مبيعات الخدمات
    }
}
