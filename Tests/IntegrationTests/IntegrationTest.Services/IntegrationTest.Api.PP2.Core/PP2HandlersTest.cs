using Api.PP2.Core.Commands.Forms;
using Api.PP2.Core.Handlers.Forms;
using AutoMapper;
using IntegrationTest.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TQ.Data.PlanningPortal;
using Xunit;

namespace IntegrationTest.Api.PP2.Core
{
    /// <summary>
    /// class for PP2 handler unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PP2HandlersTest
    {
        private readonly IMediator mediatr;
        private readonly IMapper mapper;
        private readonly PlanningPortalContext planningPortalContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PP2HandlersTest"/> class.
        /// </summary>
        public PP2HandlersTest()
        {
            var serviceProvider = IntegrationHelper.GiveServiceProvider();
            planningPortalContext = serviceProvider.GetRequiredService<PlanningPortalContext>();
            mediatr = serviceProvider.GetRequiredService<IMediator>();
            mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        /// <summary>
        /// Method for submit question set test
        /// </summary>
        /// <returns>rue if success</returns>
        [Fact]
        public async Task Handle_SubmitQuestionSetResponse()
        {
            var cmd = new SubmitQuestionSetResponse
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationName = "Integration Test App",
                QuestionSetId = Guid.NewGuid(),
                Response = "{}",
                Country = "England"
            };

            var handler = new SubmitQuestionSetResponseHandler(mediatr, mapper, planningPortalContext);

            var actual = await handler.Handle(cmd, System.Threading.CancellationToken.None);

            Assert.IsType<Guid>(actual);
        }
    }
}