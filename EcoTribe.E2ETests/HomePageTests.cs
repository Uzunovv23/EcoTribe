using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.IO;

namespace EcoTribe.Tests.Integration
{
    public class HomePageTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        private string _baseUrl;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _baseUrl = config["TestServer:BaseUrl"];
            if (string.IsNullOrWhiteSpace(_baseUrl))
            {
                throw new Exception("BaseUrl not found in testsettings.json.");
            }

            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(_baseUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Web server is running, but returned {response.StatusCode}.");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to connect to running web server at {_baseUrl}. Is it started?", ex);
            }

            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            var context = await _browser.NewContextAsync();
            _page = await context.NewPageAsync();
        }

        [Test]
        public async Task MainPage_Should_LoadSuccessfully()
        {
            await _page.GotoAsync(_baseUrl);
            var content = await _page.ContentAsync();
            Assert.That(content, Does.Contain("EcoTribe")); 
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
