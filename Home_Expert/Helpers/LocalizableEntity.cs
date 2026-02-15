using System.Globalization;

namespace Home_Expert.Helpers
{
    public class LocalizableEntity
    {
        public string Localize(string nameAr, string nameEn)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            if (culture.TwoLetterISOLanguageName.ToLower().Equals("ar-JO"))
                return nameAr;
            return nameEn;
        }
    }
}
