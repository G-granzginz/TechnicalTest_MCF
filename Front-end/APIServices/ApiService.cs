namespace Front_end.APIServices
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetDataFromApiAsync()
        {
            // Create an HttpClient instance
            var client = _httpClientFactory.CreateClient();

            // Set the base address if needed
            client.BaseAddress = new Uri("https://your-backend-api.com/");

            // Make a GET request
            var response = await client.GetAsync("api/endpoint");

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            var data = await response.Content.ReadAsStringAsync();

            return data;
        }

        public async Task PostDataToApiAsync(object payload)
        {
            // Create an HttpClient instance
            var client = _httpClientFactory.CreateClient();

            // Set the base address if needed
            client.BaseAddress = new Uri("https://your-backend-api.com/");

            // Serialize the payload to JSON
            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            // Create the HTTP content
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Make a POST request
            var response = await client.PostAsync("api/endpoint", content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();
        }
    }

}
