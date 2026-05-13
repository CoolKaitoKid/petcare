using System.Net.Http.Json;

namespace petcare;

public static class EmailOtpService
{
    private const string ServiceId = "service_ucsd03j";
    private const string TemplateId = "template_ktbxgr8";
    private const string PublicKey = "URb4mSQOBRT0vZ21E";

    public static async Task<(bool Success, string Message)> SendOtpAsync(string email, string otpCode)
    {
        try
        {
            using HttpClient client = new HttpClient();

            string expiryTime = DateTime.Now
                .AddMinutes(15)
                .ToString("hh:mm tt");

            var payload = new
            {
                service_id = ServiceId,
                template_id = TemplateId,
                user_id = PublicKey,
                template_params = new
                {
                    to_email = email,
                    passcode = otpCode,
                    time = expiryTime
                }
            };

            var response = await client.PostAsJsonAsync(
                "https://api.emailjs.com/api/v1.0/email/send",
                payload
            );

            string responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return (true, "OTP sent successfully.");
            }

            return (false, $"EmailJS failed: {(int)response.StatusCode} - {responseText}");
        }
        catch (Exception ex)
        {
            return (false, $"Error sending OTP: {ex.Message}");
        }
    }
}