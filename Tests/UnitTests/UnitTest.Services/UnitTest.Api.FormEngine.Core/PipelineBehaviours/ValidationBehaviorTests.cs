using Api.FormEngine.Core.PipelineBehaviours;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.PipelineBehaviours
{
    /// <summary>
    /// ValidationBehavior Tests
    /// </summary>
    public class ValidationBehaviorTests
    {
        private readonly CancellationToken cancellationToken = CancellationToken.None;
        private readonly RequestHandlerDelegate<Guid> requestHandlerDelegate = Mock.Create<RequestHandlerDelegate<Guid>>();
        private readonly IRequest<Guid> command = Mock.Create<IRequest<Guid>>();


        /// <summary>
        /// Test to confirm all validators executed
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task ValidationPipelineExecutesAllValidators()
        {
            // Arrange
            var validator1 = Mock.Create<AbstractValidator<IRequest<Guid>>>();
            Mock.Arrange(() => validator1.Validate(Arg.IsAny<ValidationContext<IRequest<Guid>>>())).Returns(new ValidationResult());

            var validator2 = Mock.Create<AbstractValidator<IRequest<Guid>>>();
            Mock.Arrange(() => validator2.Validate(Arg.IsAny<ValidationContext<IRequest<Guid>>>())).Returns(new ValidationResult());

            var validators = new[]
            {
                validator1,
                validator2
            };
            var sut = CreateSut(validators);

            // Act
            await sut.Handle(command, cancellationToken, requestHandlerDelegate);

            // Assert
            foreach (var validator in validators)
            {
                Mock.Assert(() => validator.Validate(Arg.IsAny<ValidationContext<IRequest<Guid>>>()), Occurs.Once());
            }

        }

        /// <summary>
        /// Test to confirm validation exception thrown when validation fails
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task ValidationPipelineThrowsExceptionWhenValidationFails()
        {
            // Arrange
            var validator = Mock.Create<AbstractValidator<IRequest<Guid>>>();
            var failure = new ValidationFailure("TestProperty", "Test Property Validation Failed");
            var result = new ValidationResult(new[] { failure });
            Mock.Arrange(() => validator.Validate(Arg.IsAny<ValidationContext<IRequest<Guid>>>())).Returns(result);

            var validators = new[]
            {
                validator
            };
            var sut = CreateSut(validators);

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ValidationException>(() => sut.Handle(command, cancellationToken, requestHandlerDelegate));
            ex.Message.Should().Contain(failure.ErrorMessage);
        }


        private ValidationBehavior<IRequest<Guid>, Guid> CreateSut(IEnumerable<IValidator<IRequest<Guid>>> validators)
        {
            return new ValidationBehavior<IRequest<Guid>, Guid>(validators);
        }
    }
}