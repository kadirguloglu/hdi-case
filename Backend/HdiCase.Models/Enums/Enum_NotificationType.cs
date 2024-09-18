
using System.ComponentModel;

public enum Enum_NotificationType
{
    [Description("Email Doğrulama Bildirimi")]
    VerificationEmailNotification = 1,
    [Description("Parolamı Unuttum Bildirimi")]
    ForgotPasswordEmailNotification,
    [Description("Tanımsız")]
    Undefined = 9999
}