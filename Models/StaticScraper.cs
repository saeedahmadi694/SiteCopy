using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

public class StaticScraper
{
    private readonly HttpClient _httpClient;

    public StaticScraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> ScrapePageAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return ParseHtml(content);
    }

    private string ParseHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Extract the content as needed. For example, you can fetch all paragraphs:
        var paragraphs = doc.DocumentNode.SelectNodes("//p");

        string extractedText = string.Empty;
        if (paragraphs != null)
        {
            foreach (var p in paragraphs)
            {
                extractedText += p.InnerText + "\n";
            }
        }

        return extractedText;
    }
}
