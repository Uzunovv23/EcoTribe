using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EcoTribe.Tests.Integration
{
    public class EventCreationFlowTests
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
        public async Task Organizator_Should_Create_Event_Successfully()
        {
            await _page.GotoAsync(_baseUrl);

            var toggleButton = await _page.WaitForSelectorAsync("#side-nav-toggle");
            Assert.That(toggleButton, Is.Not.Null, "Button with ID 'side-nav-toggle' was not found.");
            await toggleButton.ClickAsync();

            var loginLink = await _page.WaitForSelectorAsync("a[href='/Account/Login']");
            Assert.That(loginLink, Is.Not.Null, "Login link was not found.");
            await loginLink.ClickAsync();

            var emailInput = await _page.WaitForSelectorAsync("input#Email");
            Assert.That(emailInput, Is.Not.Null, "Email input was not found.");
            await emailInput.FillAsync("organizator@example.com");

            var passwordInput = await _page.WaitForSelectorAsync("input#Password");
            Assert.That(passwordInput, Is.Not.Null, "Password input was not found.");
            await passwordInput.FillAsync("Organizator123!");

            var signInButton = _page.Locator("form button[type='submit'], input[type='submit']");
            await Task.WhenAll(
                _page.WaitForNavigationAsync(new PageWaitForNavigationOptions { UrlString = $"{_baseUrl}/" }),
                signInButton.ClickAsync()
            );

            var findEventsButton = await _page.WaitForSelectorAsync("#find-event");
            Assert.That(findEventsButton, Is.Not.Null, "Find Events button not found.");
            await findEventsButton.ClickAsync();

            var createEventButton = await _page.WaitForSelectorAsync("#create-event");
            Assert.That(createEventButton, Is.Not.Null, "Create Event button not found.");
            await createEventButton.ClickAsync();

            await (await _page.WaitForSelectorAsync("#Name")).FillAsync("Eco Beach Cleanup");

            var typeSelect = await _page.WaitForSelectorAsync("#Type");
            Assert.That(typeSelect, Is.Not.Null, "Type dropdown not found.");
            await typeSelect.SelectOptionAsync(new SelectOptionValue { Label = "Cleanup" });

            await (await _page.WaitForSelectorAsync("#Description")).FillAsync("A coastal cleanup for preserving marine life.");

            await (await _page.WaitForSelectorAsync("#Start")).FillAsync("2025-07-25T10:00");
            await (await _page.WaitForSelectorAsync("#End")).FillAsync("2025-07-25T13:00");

            var map = await _page.WaitForSelectorAsync("#map");
            Assert.That(map, Is.Not.Null, "Map element not found.");
            await map.ClickAsync();

            var cityInput = await _page.WaitForSelectorAsync("#CityInput");
            await _page.WaitForFunctionAsync("el => el && el.value.length > 0", cityInput, new PageWaitForFunctionOptions
            {
                Timeout = 5000 
            });

            await (await _page.WaitForSelectorAsync("#RequiredVolunteers")).FillAsync("10");

            var submitButton = _page.Locator("form button[type='submit']");
            await Task.WhenAll(
                _page.WaitForURLAsync(url => url.Contains("/Event")),
                submitButton.ClickAsync()
            );

            Assert.That(_page.Url, Does.Contain("/Event"), "Event was not created or user was not redirected to the events page.");
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
