using CliverSystem.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace CliverSystem.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly string API_URL;
        private readonly IOptions<StripeKeys> _stripeKeys;

        public AccountController(IConfiguration configuration, IOptions<StripeKeys> stripeKeys)
        {
            API_URL = configuration["API_Url"];
            _stripeKeys = stripeKeys;
        }
        [HttpGet("balance/success")]
        public IActionResult Success([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            var userId = session.ClientReferenceId;

            return Content($"<html><body><h1>Thanks for your order, {userId}!</h1></body></html>", "text/html");
        }

        [HttpGet("balance/cancel")]
        public IActionResult Cancel()
        {

            return Content($"<html><body><h1>You just cancel payment process</h1></body></html>", "text/html");
        }

        [HttpPost("{id}/balance")]
        public async Task<IActionResult> UpdateBalance([FromRoute] string id, [FromBody] long balance)
        {
            StripeConfiguration.ApiKey = _stripeKeys.Value.SecretKey;

            var options = new SessionCreateOptions
            {
                SuccessUrl = API_URL + "/api/account/balance/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = API_URL + "/api/account/balance/cancel",
                ClientReferenceId = id,
                LineItems = new List<SessionLineItemOptions>
                {
                new SessionLineItemOptions
                {
                  Quantity = 1,
                  PriceData = new SessionLineItemPriceDataOptions
                  {
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                          Name = "Cliver balance",
                          Description = "Update balance"
                      },
                      Currency = "VND",
                      UnitAmount = balance,
                  }
                },
                },
                Mode = "payment",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Redirect(session.Url);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string endpointSecret = _stripeKeys.Value.SigningSecret;
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);

                //stripeEvent.Data.Object.
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("A successful payment for {0} was made.", paymentIntent!.Amount);
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                else if(stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    Console.WriteLine("Successful session event type: {0}", session?.ClientReferenceId ?? "NULL");
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
