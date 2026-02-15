namespace Home_Expert.Helpers
{
    public static class CultureHelper
    {
        public static bool IsRightToLeft()
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        }
    }
}
