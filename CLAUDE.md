# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

**Build (also acts as lint — warnings are treated as errors):**

```bash
cd send-sms && dotnet build
cd receive-sms && dotnet build
```

**Run:**

```bash
cd send-sms && dotnet run
cd receive-sms && dotnet run
```

**Lint documentation:**

```bash
markdownlint-cli2 README.md
```

There are no automated tests in this project.

## Architecture

Two independent .NET 10 console/web apps, each in its own directory with its own `.csproj`. Neither references the other.

**`send-sms/`** — Console app (`OutputType: Exe`). Loads credentials from `.env` via `dotenv.net`, initialises `TwilioClient`, and sends a single hardcoded SMS via `MessageResource.Create`. Entry point: `Program.cs` (`SendSMS.Main`).

**`receive-sms/`** — ASP.NET Core Minimal API (`Microsoft.NET.Sdk.Web`). Exposes two POST endpoints that Twilio calls as webhooks when an SMS arrives:

- `POST /receive/no-response` — acknowledges the message with empty TwiML.
- `POST /receive/with-response` — reads the form field `Body` and replies with a Rick Astley lyric via TwiML (`Twilio.TwiML.Messaging`). Antiforgery is disabled (`.DisableAntiforgery()`), which is intentional for this demo.

Both projects target `net10.0`, enable nullable reference types, and treat all analysis warnings as errors (`CodeAnalysisTreatWarningsAsErrors`, `TreatWarningsAsErrors`).

## Credentials

`send-sms` reads four environment variables from `.env` (copy `.env.example` → `.env`):

- `TWILIO_ACCOUNT_SID`
- `TWILIO_AUTH_TOKEN`
- `TWILIO_PHONE_NUMBER` (sender)
- `RECIPIENT` (destination, E.164 format)

`receive-sms` needs no credentials — it only responds to inbound Twilio webhooks. Expose it via ngrok (`ngrok http 8080`) and configure the forwarding URL in the Twilio Console.

## Boundaries

- **Ask before** making any significant change to `send-sms/Program.cs` or `receive-sms/Program.cs`.
- **Never** commit `.env` or any file containing real credentials.
- **Never** skip `dotnet build` — it is the static analysis gate.
