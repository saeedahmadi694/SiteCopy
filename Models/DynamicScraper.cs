namespace SiteCopy.Models;
using PuppeteerSharp;
using System.Threading.Tasks;

public class DynamicScraper
{
    public async Task<string> ScrapePageAsync(string url)
    {
		try
		{
            // Download browser if needed
            var installedBrowser = await new BrowserFetcher().DownloadAsync();

            // Launch Puppeteer and open a new page
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false // Set to false if you want to see the browser in action
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            // Wait for the dynamic content to load (tweak the selector or wait time as needed)
            await page.WaitForSelectorAsync("body");

            // Extract the entire page content
            var content = await page.GetContentAsync();

            await browser.CloseAsync();

            return content;
        }
		catch (Exception e)
		{

			throw;
		}
    }
}

