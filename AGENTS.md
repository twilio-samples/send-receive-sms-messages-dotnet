# AGENTS.md

Welcome to the Send and receive SMS messages repository.
This file contains the main points for new contributors.

## Repository overview

- **Source code**: `send-sms` and `receive-sms` contain the implementation.
- **Documentation**: README.md contains the project's documentation.
- **PR template**: `.github/PULL_REQUEST_TEMPLATE/pull_request_template.md` describes the information every PR must include.

## Project knowledge

### Repository structure

When suggesting file paths or navigation, follow this structure:

```bash
send-receive-sms-messages-php/
.
├── .github/
│   ├── ISSUE_TEMPLATE/
│   │   ├── bug_report.md             # A GitHub template for reporting bugs
│   │   ├── feature_request.md        # A GitHub template for requesting new features
│   │   └── question.md               # A GitHub template for asking questions about the project
│   ├── PULL_REQUEST_TEMPLATE/
│   │   └── pull_request_template.md  # A GitHub template for creating pull requests
    └── copilot-instructions.md       # This file
├── AGENTS.md                         # Project guidance for most Agents, except for Claude and Copilot
├── CLAUDE.md                         # Project guidance for Claude Code
├── CONTRIBUTING.md                   # Instructions for contributing to the project
├── LICENSE.md                        # The project's license (MIT)
├── README.md                         # Main landing page with table of contents
├── send-sms                          # A small .NET Minimal API app that shows how to send an SMS
└── receive-sms                       # A small .NET Minimal API app that shows how to reply to an SMS
```

### Tech stack

- .NET 10 SDK

## Prerequisites

To run the app, the following is required:

- .NET 10 SDK
- [ngrok][ngrok] and a free ngrok account
- A [Twilio account][twilio_signup] with an active phone number that can send SMS

## Set up instructions

### Send an SMS

In the _send-sms_ directory:

1. Rename the `.env.example` file to `.env`
1. Go to the [Twilio Console][twilio_console] and find your **Account SID**, **Auth Token**, and Twilio phone number.
1. Copy and paste those values into the placeholders in the `.env` file `TWILIO_ACCOUNT_SID`, `TWILIO_AUTH_TOKEN`, and `SENDER`, respectively.
1. Set your phone number, in [E.164 format][e164_format] as the value of `RECIPIENT` in _.env_
   Save the file.

### Receive an SMS

In the _receive-sms_ directory:

1. Start your ngrok server:

   ```bash
   ngrok http 8080
   ```

1. Go to the [Active numbers][active_numbers] page in the Twilio Console.
1. Click your Twilio phone number.
1. Go to the **Configure** tab and find the **Messaging Configuration** section.
1. In the **A call comes in** row, select the **Webhook** option.
1. Paste your ngrok **Forwarding** URL in the **URL** field followed by "/receive/".
   For example, if your ngrok console shows Forwarding "<https://1aaa-123-45-678-910.ngrok-free.app>", enter "<https://1aaa-123-45-678-910.ngrok-free.app/receive/>".
   - To receive an SMS **without** responding to it, append "no-response" to the URL
   - To receive an SMS and respond to it, append "with-response" to the URL
1. Click **Save configuration**.
1. Start the app

   ```bash
   cargo run
   ```

## Commands you can use

**Lint the documentation:** `markdownlint-cli2 README.md`
**Check the code:**

- Check the _send-sms_ app: `cd send-sms && dotnet build`
- Check the _receive-sms_ app: `cd receive-sms && dotnet build`

**Run the code:**

- Run the _send-sms_ app: `cd send-sms && dotnet run`
- Run the _receive-sms_ app: `cd receive-sms && dotnet run`

## Boundaries

- ✅ **Always do:** Follow the style examples, run `dotnet build` for source files
- ⚠️ **Ask first:** Before modifying existing files in a major way
- 🚫 **Never do:** Modify code in `send-sms/` or `receive-sms`, edit config files, commit secrets

## Code Style Guidelines

The code style for this project follows the [.NET Style Guide][dotnet-style-guide].

## Commit Messages and Pull Requests

- Follow [the Chris Beams style of commit messages][chris-beams-commit-message].
  Commit messages should be concise and written in the imperative mood.
  Small, focused commits are preferred.

### Pull request expectations

PRs should use the template located at `.github/PULL_REQUEST_TEMPLATE/pull_request_template.md`.
Provide a summary, test plan and issue number if applicable, then check that:

- Every pull request answers:
  - What changed?
  - Why?
  - What are the breaking changes?
  - What is the server PR (if the change requires a coordinated server update)?
- New tests are added when needed.
- Documentation is updated.
- The full test suite passes.
- Comments should be complete sentences and end with a period.

## What reviewers look for

- Tests covering new behaviour.
- Consistent style: code formatted with [Clippy][cargo-clippy] and use statements sorted.
- Clear documentation for any public API changes.
- Clean history and a helpful PR description.

[active_numbers]: https://console.twilio.com/us1/develop/phone-numbers/manage/incoming
[cargo-clippy]: https://doc.rust-lang.org/stable/clippy/usage.html
[chris-beams-commit-message]: http://chris.beams.io/posts/git-commit/
[e164_format]: https://www.twilio.com/docs/glossary/what-e164
[ngrok]: https://ngrok.com/
[dotnet-style-guide]: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions#style-guidelines
[twilio_console]: https://console.twilio.com
[twilio_signup]: https://www.twilio.com/try-twilio
