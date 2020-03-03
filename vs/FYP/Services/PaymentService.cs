using FYP.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public static class PaymentService
    {
        static string paypalToken = "https://api.sandbox.paypal.com/v1/oauth2/token";
        static string paypalUrl = "https://api.sandbox.paypal.com/v2/checkout/orders";
        static string paypalOrderCheck = "https://api.sandbox.paypal.com/v2/checkout/orders";
        static string paypalClientId = "AeZG7MedZnVswapYfb92RJfTOSuXOFkBv07yjxPztdXlJHRd1eZ0JW06bQts9T4X5qjPH92KiMCeW35A";
        static string paypalClientSecret = "ELoaz46fz0X6y_VO9wDIEaZR5MLHQYk3ypl-z0ciG67RTXSvXQvAIhSVnss7WDqYd8v6IpQObkNRjrV-";

        public static async Task<PaymentServiceOutput> InitPaypalAsync(decimal amount)
        {
            PaymentServiceOutput output = new PaymentServiceOutput();
            Uri uriOrder = new Uri(paypalUrl);
            HttpClientHandler handler = new HttpClientHandler();
            StringContent queryString = null;
            HttpResponseMessage response = null;
            HttpClient client = new HttpClient(handler);

            string s = "{\"intent\": \"CAPTURE\",\"purchase_units\": [{\"amount\": {\"currency_code\": \"MYR\", \"value\": " + amount.ToString("F2") + "}}]}";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await GenerateToken());
            queryString = new StringContent(s, Encoding.UTF8, "application/json");
            queryString.Headers.Remove("Content-Type");
            queryString.Headers.Add("Content-Type", "application/json");
            response = await client.PostAsync(uriOrder, queryString).ConfigureAwait(false);

            string fullResponse = await response.Content.ReadAsStringAsync();
            dynamic responseJson = JsonConvert.DeserializeObject(fullResponse);

            output.PaymentId = responseJson.id;
            output.PaymentLink = responseJson.links[1].href;
            return (output);
        }

        public static async Task<PaymentStatusOutput> CheckPaypal(string paymentOrderId)
        {
            PaymentStatusOutput output = new PaymentStatusOutput();
            Uri uri = new Uri(paypalOrderCheck + "/" + paymentOrderId);
            HttpClientHandler handler = new HttpClientHandler();
            HttpResponseMessage response = null;
            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await GenerateToken());
            response = await client.GetAsync(uri).ConfigureAwait(false);

            string fullResponse = await response.Content.ReadAsStringAsync();
            dynamic responseJson = JsonConvert.DeserializeObject(fullResponse);

            output.Id = responseJson.id;
            output.Status = responseJson.status;
            if (output.Status.Equals("APPROVED"))
            {
                output.PayerEmail = responseJson.payer.email_address;
                output.PayerGivenName = responseJson.payer.name.given_name;
                output.PayerSurname = responseJson.payer.name.surname;
                output.PayerId = responseJson.payer.payer_id;
            }

            return output;
        }

        public static async Task<string> GenerateToken()
        {
            Uri uriToken = new Uri(paypalToken);
            HttpClientHandler handler = new HttpClientHandler();
            StringContent queryString = null;
            HttpResponseMessage response = null;
            HttpClient client = new HttpClient(handler);

            string s = "grant_type=client_credentials";
            queryString = new StringContent(s, Encoding.UTF8, "application/json");
            queryString.Headers.Remove("Content-Type");
            queryString.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var byteArray = new UTF8Encoding().GetBytes(paypalClientId + ":" + paypalClientSecret);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            response = await client.PostAsync(uriToken, queryString).ConfigureAwait(false);
            dynamic json = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return(json.access_token);
        }
    }
}
