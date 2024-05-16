using Microsoft.Playwright;

[Parallelizable(ParallelScope.Self)]
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
        _browser = await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Test]
    public async Task MyTest()
    {
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

        // Final (dummy) assertion
        Assert.AreEqual(0, 0);
    }
}
