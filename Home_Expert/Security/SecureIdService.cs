using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Security;
using System.Text.Json;

namespace Home_Expert.Security
{
    //Service Implementation(DataProtection + Expiry + URL-safe)
    //هذا يحقق: تشفير + سلامة + Expiry بدون DB، وبأداء ممتاز.

    //4 الخطوة الرابعة: انشاء السيرفس اللي رح ينفذ الانترفيس باستخدام Data Protection API
    public sealed class SecureIdService : ISecureIdService
    {
        private readonly ITimeLimitedDataProtector _protector;
        private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

        public SecureIdService(IDataProtectionProvider dp)
        {
            // Purpose مهم لفصل هذا البروتكتر عن غيره
            _protector = dp.CreateProtector("SecureId.v1").ToTimeLimitedDataProtector();
        }

        public string Protect(int id, string scope, string? userId = null, TimeSpan? ttl = null)
        {
            var payload = new SecureIdPayload(id, scope, userId);
            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(payload, JsonOpts);

            // Protect bytes with expiration
            var protectedBytes = _protector.Protect(jsonBytes, ttl ?? TimeSpan.FromMinutes(15));

            // URL-safe
            return WebEncoders.Base64UrlEncode(protectedBytes);
        }

        public int Unprotect(string token, string expectedScope, string? expectedUserId, out DateTimeOffset expiresAt)
        {
            byte[] protectedBytes;
            try
            {
                protectedBytes = WebEncoders.Base64UrlDecode(token);
            }
            catch
            {
                throw new SecurityException("Invalid token encoding.");
            }

            byte[] jsonBytes;
            try
            {
                jsonBytes = _protector.Unprotect(protectedBytes, out expiresAt);
            }
            catch
            {
                // يشمل: token manipulated أو انتهت صلاحيته أو wrong key ring
                throw new SecurityException("Invalid or expired token.");
            }

            SecureIdPayload? payload;
            try
            {
                payload = JsonSerializer.Deserialize<SecureIdPayload>(jsonBytes, JsonOpts);
            }
            catch
            {
                throw new SecurityException("Invalid token payload.");
            }

            if (payload is null)
                throw new SecurityException("Invalid token payload.");

            if (!string.Equals(payload.Scope, expectedScope, StringComparison.Ordinal))
                throw new SecurityException("Scope mismatch.");

            if (expectedUserId != null && !string.Equals(payload.UserId, expectedUserId, StringComparison.Ordinal))
                throw new SecurityException("User binding mismatch.");

            return payload.Id;
        }
    }

}
