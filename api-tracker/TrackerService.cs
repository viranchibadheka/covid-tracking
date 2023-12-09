using System.Net.Http;
using System.Threading.Tasks;

public class TrackerService {
    private readonly HttpClient _httpClient;

    public TrackerService(HttpClient httpclient) {
        _httpClient = httpclient;
    }

    public async Task<HttpResponseMessage> getCovidData(string state) {
        try {
            var response = await _httpClient.GetAsync($"https://api.covidtracking.com/v1/states/{state}/daily.json");
            return response;
        }
        catch (Exception e) {
            var error = new HttpResponseMessage(statusCode: System.Net.HttpStatusCode.InternalServerError);
            error.Content = new StringContent(e.Message);
            Console.WriteLine(e);
            return error;
        }
    }
}