namespace Home_Expert.Security
{
    //1 الخطوة الاولى : انشاء كلاس الاوبشنز
    public sealed class SecureIdOptions
    {
        // اسم باراميتر التوكن في الـ URL
        public string TokenQueryKey { get; set; } = "rid";

        // اسم باراميتر الـ id الموجود حاليًا بمشروعك
        public string IdQueryKey { get; set; } = "id";

        // TTL افتراضي
        public TimeSpan DefaultTtl { get; set; } = TimeSpan.FromMinutes(15);

        // لو بدك تربط التوكن بالمستخدم الحالي
        public bool BindToUser { get; set; } = true;
    }

}
