using Xunit;

namespace ServiceDirectory.Ui.Test;

public class HomePageTests : PageTestBase
{
    [Fact]
    public async Task LaunchingHomePage_HasPageDescription()
    {
        await GotToPageAsync("/");
        
        await ShouldHaveTextAsync(
            "description",
            "Use the search tools below to find services within a certain distance of the entered postcode. " +
            "These will be ordered from the closest to furthest.");
    }

    [Fact]
    public async Task PostcodeWithNoServicesAvailable_Search_ReturnsNoServices()
    {
        await GotToPageAsync("/");
        await EnterTextAsync("postcode-field", "BN1 9BT");
        
        await ClickButton("search-button");
        
        await ShouldHaveTextAsync("No services found");
    }
    
    [Fact]
    public async Task PostcodeWithServicesAvailable_Search_ReturnsServices()
    {
        await GotToPageAsync("/");
        await EnterTextAsync("postcode-field", "BS3 2EJ");
        
        await ClickButton("search-button");
        
        await ShouldHaveTextAsync("ANDYSMANCLUB");
        await ShouldHaveTextAsync("Autism Advice - Bristol");
        await ShouldHaveTextAsync("Activity Group - Redfield (St Anne's)");
    }
}