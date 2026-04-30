using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Handle application exceptions
//
// This will be triggered if a POST request is received that does not contain a
// "Body" element. To be fair, this is unlikely as the requests come from Twilio.
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

        if (contextFeature is not null)
        {
            Console.WriteLine($"Error : {contextFeature.Error}");
            await context.Response.WriteAsJsonAsync(
                new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error",
                    MoreInfo = contextFeature.Error.Message,
                }
            );
        }
    });
});

/// This route does nothing when an incoming SMS is received.
///
/// The function's response will contain TwiML that provides no further instructions to Twilio.
/// In addition the response's status code will be an HTTP 200 OK, and the response will have
/// the Content-Type header set to "application/xml; charset=utf-8".
app.MapPost(
    "/receive/no-response",
    () =>
    {
        var response = new MessagingResponse();
        response.Append(new Message());

        return Results.Text(response.ToString(), contentType: "application/xml");
    }
);

/// This route instructs Twilio to reply to an incoming SMS.
///
/// If the request's form data contains an element named "Body" with the value
/// "never gonna", the delegate's response will contain TwiML that instructs
/// Twilio to send a reply SMS to the sender of the original SMS with a line
/// from Rick Astley's hit "Never Gonna Give You Up". Otherwise the delegate
/// sends the same, stock line from the same song.
///
/// In addition the response's status code will be an HTTP 200 OK, and the
/// response will have the Content-Type header set to "application/xml;
/// charset=utf-8".
app.MapPost(
        "/receive/with-response",
        ([FromForm(Name = "Body")] string body = "") =>
        {
            string defaultOption =
                "I just wanna tell you how I'm feeling - Gotta make you understand";
            string[] options =
            {
                "give you up",
                "let you down",
                "make you cry",
                "run around and desert you",
                "say goodbye",
                "tell a lie, and hurt you",
            };
            int index = new Random().Next(0, options.Length - 1);
            var response = new MessagingResponse();
            var message = new Message();

            message.Body(body.ToLower() == "never gonna" ? options[index] : defaultOption);
            response.Append(message);

            return Results.Text(response.ToString(), contentType: "application/xml");
        }
    )
    // Antiforgery functionality has been disabled as the application is a
    // simplistic example. This shouldn't be done in a production setting.
    .DisableAntiforgery();

app.Run();
