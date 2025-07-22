using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EcoTribe.Tests.Integration
{
    public class LoginFlowTests
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
        public async Task User_Should_Login_Successfully()
        {
            await _page.GotoAsync(_baseUrl);

            var toggleButton = await _page.QuerySelectorAsync("#side-nav-toggle");
            Assert.That(toggleButton, Is.Not.Null, "Button with ID 'side-nav-toggle' was not found on the page.");
            await toggleButton.ClickAsync();

            var loginLink = await _page.QuerySelectorAsync("a[href='/Account/Login']");
            Assert.That(loginLink, Is.Not.Null, "Log In button with href '/Account/Login' was not found.");
            await loginLink.ClickAsync();

            var emailInput = await _page.WaitForSelectorAsync("input#Email", new() { Timeout = 5000 });
            Assert.That(emailInput, Is.Not.Null, "Email input with ID 'Email' was not found within timeout.");
            await emailInput.FillAsync("slavi123@abv.bg");

            var passwordInput = await _page.WaitForSelectorAsync("input#Password", new() { Timeout = 5000 });
            Assert.That(passwordInput, Is.Not.Null, "Password input with ID 'Password' was not found within timeout.");
            await passwordInput.FillAsync("Slavi123!");

            var signInButton = _page.Locator("form button[type='submit'], input[type='submit']");
            await Task.WhenAll(
                _page.WaitForURLAsync($"{_baseUrl}/"),
                signInButton.ClickAsync()
            );

            var logoutLink = await _page.WaitForSelectorAsync("a[href='/Account/Logout']", new() { Timeout = 5000 });
            Assert.That(logoutLink, Is.Not.Null, "Logout link was not found — login may have failed.");
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
