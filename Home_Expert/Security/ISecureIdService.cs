namespace Home_Expert.Security
{
    //Service Interface

    //3 الخطوة الثالثة: انشاء انترفيس للسيرفس اللي رح يكون مسؤول عن تشفير وفك تشفير التوكن
    public interface ISecureIdService
    {
        string Protect(int id, string scope, string? userId = null, TimeSpan? ttl = null);
        int Unprotect(string token, string expectedScope, string? expectedUserId, out DateTimeOffset expiresAt);
    }

}
