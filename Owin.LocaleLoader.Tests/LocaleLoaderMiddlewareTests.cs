using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Napalm684.Owin.LocaleLoader.Dependencies;
using Napalm684.Owin.LocaleLoader.Services;
using Napalm684.Owin.LocaleLoader.Tests.Helpers;
using Owin;

namespace Napalm684.Owin.LocaleLoader.Tests
{
    /// <summary>
    /// Locale Loader Middleware Unit Tests
    /// </summary>
    [TestClass]
    public class LocaleLoaderMiddlewareTests
    {
        private ILocaleService _fakeLocaleService;

        /// <summary>
        /// Initialize items for testing
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _fakeLocaleService = A.Fake<FakeLocaleService>();
            A.CallTo(() => _fakeLocaleService.GetLocale(A<object[]>.Ignored)).CallsBaseMethod();
        }

        /// <summary>
        /// Test using a fake locale service as the locale provider
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task LocaleLoaderMiddleware_Service()
        {
            //Arrange
            var resolver = A.Fake<IDependencyResolver>();
            A.CallTo(() => resolver.GetService(typeof(ILocaleService))).Returns(_fakeLocaleService);

            using (var server = TestServer.Create(app =>
            {
                app.Use(typeof(FakeLoginMiddleware));

                app.UseLocaleLoader(new LocaleLoaderOptions()
                {
                    LocaleMappings =
                    {
                        { LocaleLoaderConstants.Placeholder, LocaleLoaderConstants.Actual }
                    },
                    DependencyResolver = resolver
                });

                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync(ctx.Request.Path.Value);
                });
            }))
            {
                //Act
                var response = await server.CreateRequest(LocaleLoaderConstants.FilePath + LocaleLoaderConstants.Placeholder).GetAsync();

                //Assert
                A.CallTo(() => resolver.GetService(typeof(ILocaleService))).MustHaveHappened(Repeated.Exactly.Once);
                A.CallTo(() => _fakeLocaleService.GetLocale(A<object[]>.That.Matches(x => x.Length == 1 && (string)x[0] == LocaleLoaderConstants.UserName))).MustHaveHappened(Repeated.Exactly.Once);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
                Assert.AreEqual(LocaleLoaderConstants.FilePath + String.Format(LocaleLoaderConstants.Actual, LocaleLoaderConstants.LocaleSpecific), await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Test using a fake locale service as the locale provider
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task LocaleLoaderMiddleware_Service_Additional_Parameters()
        {
            //Arrange
            var resolver = A.Fake<IDependencyResolver>();
            A.CallTo(() => resolver.GetService(typeof(ILocaleService))).Returns(_fakeLocaleService);

            using (var server = TestServer.Create(app =>
            {
                app.Use(typeof(FakeLoginMiddleware));

                app.UseLocaleLoader(new LocaleLoaderOptions()
                {
                    LocaleMappings =
                    {
                        { LocaleLoaderConstants.Placeholder, LocaleLoaderConstants.Actual }
                    },
                    DependencyResolver = resolver,
                    Parameters = new object[]{ LocaleLoaderConstants.TestParameter }
                });

                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync(ctx.Request.Path.Value);
                });
            }))
            {
                //Act
                var response = await server.CreateRequest(LocaleLoaderConstants.FilePath + LocaleLoaderConstants.Placeholder).GetAsync();

                //Assert
                A.CallTo(() => resolver.GetService(typeof(ILocaleService))).MustHaveHappened(Repeated.Exactly.Once);
                A.CallTo(() => _fakeLocaleService.GetLocale(A<object[]>.That.Matches(x => x.Length == 2 && 
                                                            (string)x[0] == LocaleLoaderConstants.UserName &&
                                                            (string)x[1] == LocaleLoaderConstants.TestParameter))).MustHaveHappened(Repeated.Exactly.Once);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
                Assert.AreEqual(LocaleLoaderConstants.FilePath + String.Format(LocaleLoaderConstants.Actual, LocaleLoaderConstants.LocaleSpecific), await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Test using a browser accept-language header value (full) as the locale provider
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task LocaleLoaderMiddleware_Browser()
        {
            //Arrange
            using (var server = TestServer.Create(app =>
            {
                app.UseLocaleLoader(LocaleLoaderConstants.Placeholder, LocaleLoaderConstants.Actual);

                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync(ctx.Request.Path.Value);
                });
            }))
            {
                //Act
                HttpResponseMessage response =
                    await server.CreateRequest(LocaleLoaderConstants.FilePath + LocaleLoaderConstants.Placeholder)
                                    .AddHeader(LocaleLoaderConstants.AcceptLanguge, LocaleLoaderConstants.FullLocale)
                                    .GetAsync();

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
                Assert.AreEqual(LocaleLoaderConstants.FilePath + String.Format(LocaleLoaderConstants.Actual, LocaleLoaderConstants.LocaleSpecific), await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Test using a browser accept-language header value (generic) as the locale provider
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task LocaleLoaderMiddleware_Browser_Generic_AcceptLanguage()
        {
            //Arrange
            using (var server = TestServer.Create(app =>
            {
                app.UseLocaleLoader(LocaleLoaderConstants.Placeholder, LocaleLoaderConstants.Actual);

                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync(ctx.Request.Path.Value);
                });
            }))
            {
                //Act
                HttpResponseMessage response =
                    await server.CreateRequest(LocaleLoaderConstants.FilePath + LocaleLoaderConstants.Placeholder)
                                    .AddHeader(LocaleLoaderConstants.AcceptLanguge, LocaleLoaderConstants.LocaleGeneric)
                                    .GetAsync();

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
                Assert.AreEqual(LocaleLoaderConstants.FilePath + String.Format(LocaleLoaderConstants.Actual, LocaleLoaderConstants.LocaleGenericFinal), await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Test making a request that is not mapped to ensure it is not altered
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task LocaleLoaderMiddleware_Unmapped_Path()
        {
            //Arrange
            using (var server = TestServer.Create(app =>
            {
                app.UseLocaleLoader(LocaleLoaderConstants.Placeholder, LocaleLoaderConstants.Actual);

                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync(ctx.Request.Path.Value);
                });
            }))
            {
                //Act
                HttpResponseMessage response =
                    await server.CreateRequest(LocaleLoaderConstants.FilePath + LocaleLoaderConstants.UnmappedPlaceholder)
                                    .AddHeader(LocaleLoaderConstants.AcceptLanguge, LocaleLoaderConstants.FullLocale)
                                    .GetAsync();

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
                Assert.AreEqual(LocaleLoaderConstants.FilePath + LocaleLoaderConstants.UnmappedPlaceholder, await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Test making a request that is not logged in to ensure null exception does not occur when
        /// passing the user name parameter to the locale service
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task LocaleLoaderMiddleware_NullAuthentication()
        {
            //Arrange
            var resolver = A.Fake<IDependencyResolver>();
            A.CallTo(() => resolver.GetService(typeof(ILocaleService))).Returns(_fakeLocaleService);

            using (var server = TestServer.Create(app =>
            {
                app.UseLocaleLoader(new LocaleLoaderOptions
                {
                    LocaleMappings =
                    {
                        { LocaleLoaderConstants.Placeholder, LocaleLoaderConstants.Actual }
                    },
                    DependencyResolver = resolver
                });

                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync(ctx.Request.Path.Value);
                });
            }))
            {
                //Act
                HttpResponseMessage response =
                    await server.CreateRequest(LocaleLoaderConstants.FilePath + LocaleLoaderConstants.Placeholder)
                                    .AddHeader(LocaleLoaderConstants.AcceptLanguge, LocaleLoaderConstants.FullLocale)
                                    .GetAsync();

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
                Assert.AreEqual(LocaleLoaderConstants.FilePath + String.Format(LocaleLoaderConstants.Actual, LocaleLoaderConstants.LocaleSpecific), await response.Content.ReadAsStringAsync());
            }
        }
    }
}
