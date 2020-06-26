using Api.Admin.Core.Commands.AzureUser;
using Api.Admin.Core.Handlers.AzureUser;
using Api.Admin.Core.ViewModels;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Helpers;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.AzureUser
{
    [ExcludeFromCodeCoverage]
    public class GetAzureUserHandlerTests
    {
        private readonly IB2CGraphClient mockB2CGraphClient = Mock.Create<IB2CGraphClient>();
        private readonly CancellationToken cancellationToken = new CancellationToken();
        private readonly GetAzureUserHandler sut;

        public GetAzureUserHandlerTests()
        {
            this.sut = new GetAzureUserHandler(mockB2CGraphClient);
        }

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Fact]
        public async Task HandleSuccess()
        {
            // Arrange
            var request = new AzureUserHtmlUrl
            {
                Active = true
            };

            var expected = GetAzureUserDataViewModel();

            Mock.Arrange(() => mockB2CGraphClient.GetAllUsersAsync(request.Active)).Returns(Task.FromResult(GetExpectedUserListAsJson())).MustBeCalled();

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Asset
            actual.Should().BeEquivalentTo(expected.Value);
        }

        private string GetExpectedUserListAsJson()
        {
            return JsonConvert.SerializeObject(GetAzureUserDataViewModel());
        }

        private AzureUserDataViewModel GetAzureUserDataViewModel()
        {
            return new AzureUserDataViewModel
            {
                Value = new List<AzureUserObject>
                {
                    {
                        new AzureUserObject
                        {
                            AccountEnabled = true,
                            DisplayName = "DisplayName",
                            Emailid = "EmailId",
                            ObjectId = "ObjectId",
                            SignInNames = new List<SignInNames>
                            {
                                {
                                    new SignInNames
                                    {
                                        Type = "Type",
                                        Value = "Value"
                                    }
                                }
                            },
                            UserPrincipalName = "UserPrincipalName"
                        }
                    }
                }
            };
        }
    }
}
