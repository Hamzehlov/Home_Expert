using Microsoft.AspNetCore.Mvc;

namespace Home_Expert.Security
{
    //Scope: لمنع إعادة استخدام توكن(Controller/Action) في مكان آخر.

    //UserId: (اختياري) لمنع مشاركة الرابط بين المستخدمين.

    //2 الخطوة الثانية: انشاء ريكورد يحمل البيانات اللي هنشفّرها جوا التوكن
    public sealed record SecureIdPayload(
     int Id,
     string Scope,
     string? UserId // اختياري
 );

}
