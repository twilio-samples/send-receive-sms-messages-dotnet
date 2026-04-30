using dotenv.net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

class SendSMS
{
    static void Main(string[] args)
    {
        // Load .env from the application directory into the system environment
        DotEnv.Load();

        // Initialise the required variables from the accompanying environment variables
        string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID") ?? "";
        string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN") ?? "";
        string twilioPhoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER") ?? "";
        string recipientPhoneNumber = Environment.GetEnvironmentVariable("RECIPIENT") ?? "";

        // Send an SMS to the recipient
        TwilioClient.Init(accountSid, authToken);
        var to = new PhoneNumber(recipientPhoneNumber);
        var message = MessageResource.Create(
            to,
            from: new PhoneNumber(twilioPhoneNumber),
            body: "This is the ship that made the Kessel Run in fourteen parsecs?"
        );

        Console.WriteLine(message.Sid);
    }
}
