using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace FlaskServerOpenAI.Pages
{
    public class IndexModel : PageModel
    {
        public string Response { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return Page();
            }

            string model = "davinci"; // replace with your desired model
            int maxTokens = 100; // replace with your desired max tokens
            string apiKey = "<your OpenAI API key>"; // replace with your OpenAI API key

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                string data = JsonConvert.SerializeObject(new
                {
                    prompt = prompt,
                    max_tokens = maxTokens
                });

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/prompt", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                Response = jsonResponse.choices[0].text;
            }

            return Page();
        }
    }
}
