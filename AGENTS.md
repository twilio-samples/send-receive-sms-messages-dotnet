# Send & receive SMS messages with Twilio and .NET

This sample demonstrates how to send and receive SMS messages using the Twilio Programmable Messaging API with .NET/C#.

## Environment Variables

Copy `.env.example` to `.env`. Never commit `.env`.

```bash
cp .env.example .env
```

| Variable | Where to find | Format |
| -------- | ------------- | ------ |
| `TWILIO_ACCOUNT_SID` | Console homepage or Admin dropdown (top right) → Account Management → Keys & Credentials → API Keys & Tokens | Starts with `AC` |
| `TWILIO_AUTH_TOKEN` | Console homepage or Admin dropdown (top right) → Account Management → Keys & Credentials → API Keys & Tokens → click to reveal | 32-char string. Treat as a password. |
| `TWILIO_PHONE_NUMBER` | Console → Phone Numbers → Manage → Active Numbers | E.164 format: `+15551234567` |
| `RECIPIENT` | The destination phone number you want to SMS | E.164 format: `+15551234567` |

## Commands

```bash
# Send an SMS
cd send-sms && dotnet run

# Receive SMS (start the webhook server)
cd receive-sms && dotnet run

# Expose the webhook server to the internet
# Requires ngrok — install and authenticate at https://ngrok.com before running
ngrok http 5169
# Set the resulting URL + /receive/with-response (or /receive/no-response) as the
# webhook in Twilio Console → Phone Numbers → Manage → Active Numbers → A Message Comes In
```

## Project Structure

- `send-sms/Program.cs` — loads `.env`, initialises the Twilio client, and sends a single SMS
- `receive-sms/Program.cs` — ASP.NET Core Minimal API with two POST webhook endpoints
- `send-sms/.env.example` — template for required credentials (copy to `.env` before running)

## Agent Boundaries

**Always:**
- Confirm `.env` is configured before running any command in `send-sms/`
- Use the Environment Variables section above to guide the user to each credential
- Confirm `dotnet run` started successfully in `receive-sms/` before asking the user to test webhooks
- Run `dotnet build` in the relevant project directory after any code change — it is the static analysis gate

**Never:**
- Run `send-sms` with missing or placeholder credentials
- Hardcode credentials or phone numbers in source files
- Commit `.env` or any file containing real credentials

## Verify It's Working

**Send:** After running `cd send-sms && dotnet run`, the console prints a Twilio Message SID (starts with `SM`). The phone number in `RECIPIENT` receives the text "This is the ship that made the Kessel Run in fourteen parsecs?"

**Receive:** After starting `receive-sms` and pointing the Twilio webhook to your ngrok URL, text "never gonna" to the number in `TWILIO_PHONE_NUMBER` and expect an SMS reply containing a line from "Never Gonna Give You Up". Any other message body returns the stock reply.

## Twilio Resources

- [Twilio Console](https://console.twilio.com) — credentials, phone numbers, webhook configuration
- [Programmable Messaging docs](https://www.twilio.com/docs/sms) — SMS sending and receiving reference
- [Twilio .NET SDK](https://www.twilio.com/docs/libraries/csharp-dotnet) — SDK reference for C#/.NET
- [TwiML for Messaging](https://www.twilio.com/docs/messaging/twiml) — TwiML verbs used in the webhook response
