using Microsoft.Playwright;
using RestSharp.Authenticators;
using RestSharp;
using System.Threading;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Interfaces;

//[Parallelizable(ParallelScope.All)]
[TestFixture]
public class PlaywrightDemoTest
{

    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;

    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Playwright.CreateAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [TestCase("chromium")]
    [TestCase("firefox")]
    [TestCase("webkit")]
    public async Task MyTest (string browserType)
    {
        // Launch the specified browser
        _browser = browserType switch
        {
            "chromium" => await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }),
            "firefox" => await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false}),
            "webkit" => await _playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false}),
            _ => throw new ArgumentException("Invalid browser type", nameof(browserType))
        };
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();

        _context.ClearCookiesAsync();

        await _page.GotoAsync("https://demo.nopcommerce.com/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "Computers" }).ClickAsync();
        await _page.GetByRole(AriaRole.Heading, new() { Name = "Notebooks" }).GetByRole(AriaRole.Link).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Asus N551JK-XO076H Laptop", Exact = true }).ClickAsync();
        await _page.Locator("#add-to-cart-button-5").ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Shopping cart (1)" }).ClickAsync();

        var cartText = await _page.Locator("#shopping-cart-form").TextContentAsync();
        Assert.That(cartText, Does.Contain("$1,500.00"));

        await _page.GetByRole(AriaRole.Button, new() { Name = "Checkout" }).ClickAsync();

        var termsOfServiceVisible = await _page.GetByText("Terms of service", new() { Exact = true }).IsVisibleAsync();
        Assert.IsTrue(termsOfServiceVisible);


    }

}
