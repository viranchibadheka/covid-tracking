using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

[ApiController]
[Route("")]
public class CovidController : ControllerBase
{
    private readonly TrackerService _trackerService;

    public CovidController(TrackerService trackerService) {
        _trackerService = trackerService;
    }

    [HttpGet("covid-data/{datetime}")]
    public async Task<ActionResult> GetCovidDataByDate(string datetime)
    {
        if (DateTime.TryParseExact(datetime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        {
            try
            {
                var response = await _trackerService.getCovidData("az");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonNode.Parse(content)?.AsArray();

                    var filteredData = json?
                        .Where(obj =>
                        {
                            var date = obj?["date"]?.ToString();
                            return date?.CompareTo(datetime) < 0 && date.CompareTo(parsedDate.AddDays(-8).ToString("yyyyMMdd")) > 0;
                        })
                        .ToList();

                    var refactoredJson = new List<AZStats>();

                    foreach (var obj in filteredData!)
                    {
                        var date = obj?["date"]?.ToString();
                        var positive = obj?["positive"]?.ToString();
                        var negative = obj?["negative"]?.ToString();
                        var azStats = new AZStats
                        {
                            date = date!,
                            positive = positive!,
                            negative = negative!
                        };
                        refactoredJson.Add(azStats);
                    }

                    return Ok(refactoredJson);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.Content);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        else
        {
            return BadRequest("Invalid date format. Please use the format 'yyyyMMdd'.");
        }
    }

    
    [HttpPost("covid-data")]
    public async Task<ActionResult> getCovidDataByRange([FromBody] JsonNode postData) {
        var startDate = postData["startDate"]?.ToString();
        var endDate = postData["endDate"]?.ToString();
        var state = postData["state"]?.ToString();

        var apiResponse = await _trackerService.getCovidData(state!);

        if(apiResponse.IsSuccessStatusCode) {
            var content = await apiResponse.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(content)?.AsArray();
            var filteredJson = json?.Where(obj =>
                {
                    var date = obj?["date"]?.ToString();
                    return date?.CompareTo(startDate)>=0 && date.CompareTo(endDate)<=0;
                }
            ).ToList();

            var refactoredJson = new List<AZStats>();

            foreach(var obj in filteredJson!) {
                var date = obj!["date"]?.ToString();
                var positive = obj["positive"]==null? "0": obj["positive"]?.ToString();
                var negative = obj["negative"]==null? "0": obj["negative"]?.ToString();
                var azStats = new AZStats
                {
                    date = date!,
                    positive = positive!,
                    negative = negative!
                };
                refactoredJson.Add(azStats);
            }

            return Ok(refactoredJson);
        }
        else{
            return StatusCode((int)apiResponse.StatusCode, apiResponse.Content);
        }
    }

}