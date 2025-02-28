using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Ui.Test;

[Trait("Category", "E2E")]
public abstract class PageTestBase : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            ColorScheme = ColorScheme.Light,
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            BaseURL = "http://localhost:3000"
        };
    }
    
    protected async Task GotToPageAsync(string url)
    {
        var response = await Page.GotoAsync(url);
        response.ShouldNotBeNull("Failed to return a page response.");
        response.Ok.ShouldBeTrue($"Navigating to requested page {url} failed.");
    }

    protected ILocator GetByTestId(string testId)
    {
        Playwright.Selectors.SetTestIdAttribute("testid"); // For some reason vuetify doesn't like data-testid!
        return Page.GetByTestId(testId);
    }

    protected async Task ClickButton(string testId)
    {
        await Page.ClickAsync($"[testid={testId}]");
    }
    
    protected async Task EnterTextAsync(string testId, string text)
    {
        await Page.FillAsync($"[testid={testId}]", text);
    }

    protected async Task ShouldHaveTextAsync(string text)
    {
        await Expect(Page.GetByText(text)).ToBeVisibleAsync();
    }
    
    protected async Task ShouldHaveTextAsync(string testId, string text)
    {
        var descriptionLocator = GetByTestId(testId);
        await Expect(descriptionLocator).ToHaveTextAsync(text);
    }
}