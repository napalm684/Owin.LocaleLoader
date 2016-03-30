using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Owin;

namespace Napalm684.Owin.LocaleLoader.Tests.Helpers
{
    /// <summary>
    /// Fake Login Middleware for Testing purposes only
    /// </summary>
    public class FakeLoginMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="next"></param>
        public FakeLoginMiddleware(OwinMiddleware next) : base(next)
        {
        }

        /// <summary>
        /// Invoke FakeLogin Middleware Functionality
        /// </summary>
        /// <param name="context">Owin Context</param>
        /// <returns>Task</returns>
        public override async Task Invoke(IOwinContext context)
        {
            var identity = A.Fake<IIdentity>();
            A.CallTo(() => identity.Name).Returns("username@domain.local");
            var user = new ClaimsPrincipal(identity);
            context.Request.Context.Authentication.User = user;
            await Next.Invoke(context);
        }
    }
}
