using Twilio;

namespace Kamirion.RepairShop.Communication.Infrastructure;

public static class TwilioClientInitializer
{
    public static void Initialize(string accountSid, string authToken)
    {
        TwilioClient.Init(accountSid, authToken);
    }
}
