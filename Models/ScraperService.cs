namespace SiteCopy.Models;
using PuppeteerSharp;
using System.Threading.Tasks;
public class ScraperService
{
    //private readonly StaticScraper _staticScraper;
    //private readonly DynamicScraper _dynamicScraper;

    //public ScraperService(HttpClient httpClient)
    //{
    //    _staticScraper = new StaticScraper(httpClient);
    //    _dynamicScraper = new DynamicScraper();
    //}

    //public async Task<string> ScrapeAsync(string url, bool isDynamic)
    //{
    //    if (isDynamic)
    //    {
    //        return await _dynamicScraper.ScrapePageAsync(url);
    //    }
    //    else
    //    {
    //        return await _staticScraper.ScrapePageAsync(url);
    //    }
    //}

    public async Task CloneSpaAsync(string url, string outputDir)
    {
        // Download the browser if needed
        await new BrowserFetcher().DownloadAsync();

        // Launch the browser
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = false // Set to false if you want to see the browser in action
        });
        var page = await browser.NewPageAsync();

        // Navigate to the SPA
        await page.GoToAsync(url);

        // Wait for the content to load (adjust the selector if needed)
        await page.WaitForSelectorAsync("body");

        // Get the content of the page
        var content = await page.GetContentAsync();

        // Save the HTML content to a file
        Directory.CreateDirectory(outputDir);
        await File.WriteAllTextAsync(Path.Combine(outputDir, "index.html"), content);

        // Extract and save additional resources
        await SaveResourcesAsync(page, outputDir);

        await browser.CloseAsync();
    }

    private async Task SaveResourcesAsync(IPage page, string outputDir)
    {
        var resourceUrls = await page.EvaluateExpressionAsync<string[]>(@"
            Array.from(document.querySelectorAll('link[href], script[src], img[src]')).map(el => el.src || el.href)
        ");

        foreach (var resourceUrl in resourceUrls)
        {
            if (string.IsNullOrEmpty(resourceUrl)) continue;

            var fileName = Path.GetFileName(resourceUrl);
            var resourcePath = Path.Combine(outputDir, fileName);

            try
            {
                // Download and save each resource
                using var httpClient = new HttpClient();
                var resourceData = await httpClient.GetByteArrayAsync(resourceUrl);
                await File.WriteAllBytesAsync(resourcePath, resourceData);
            }
            catch (Exception)
            {
                //
            }
        }
    }
}

