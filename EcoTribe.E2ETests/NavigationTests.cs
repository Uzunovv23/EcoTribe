using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EcoTribe.Tests.Integration
{
    public class NavigationTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private string _baseUrl;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "testsettings.json"), optional: false)
                .Build();

            _baseUrl = config["TestServer:BaseUrl"];
            if (string.IsNullOrWhiteSpace(_baseUrl))
                throw new Exception("BaseUrl not found in testsettings.json.");

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(_baseUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Web server responded with status code: {response.StatusCode}");

            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false 
            });

            var context = await _browser.NewContextAsync();
            _page = await context.NewPageAsync();
        }

        [Test]
        public async Task SideNavToggle_Should_BeClickable()
        {
            await _page.GotoAsync(_baseUrl);

            var toggleButton = await _page.QuerySelectorAsync("#side-nav-toggle");
            Assert.That(toggleButton, Is.Not.Null, "Button with ID 'side-nav-toggle' was not found on the page.");

            await toggleButton.ClickAsync();

            Assert.Pass("Button was clicked successfully.");
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _page?.CloseAsync();
            await _browser?.CloseAsync();
            _playwright?.Dispose();
        }
    }
}
