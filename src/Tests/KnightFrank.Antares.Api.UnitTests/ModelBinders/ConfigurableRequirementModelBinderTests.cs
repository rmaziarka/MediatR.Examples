using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Api.UnitTests.ModelBinders
{
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Metadata;
    using System.Web.Http.ModelBinding;

    using FluentAssertions;

    using FluentValidation;

    using KnightFrank.Antares.Api.ModelBinders;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Tests.Common.Extensions.AutoFixture.Attributes;

    using ModelBinders;

    using Moq;

    using Xunit;

    [Collection("ConfigurableRequirementModelBinder")]
    [Trait("FeatureTitle", "Requirements")]
    public class ConfigurableRequirementModelBinderTest
    {
        [Theory]
        [AutoMoqData]
        public void Given_ConfigurableRequirementModelBinder_When_PageTypeActionArgument_Is_Null_Then_ThrowsValidationException(
            HttpActionContext httpActionContext,
            Mock<ModelBindingContext> bindingContextMock)
        {
            // Arrange 
            var modelBinder = new ConfigurableRequirementModelBinder();
            httpActionContext.ActionArguments["pageType"] = null;

            // Act
            Action action = () => modelBinder.BindModel(httpActionContext, bindingContextMock.Object);

            // Assert
            action.ShouldThrow<ValidationException>();
        }

        [Theory]
        [InlineAutoMoqData(PageType.Create, typeof(CreateRequirementCommand))]
        [InlineAutoMoqData(PageType.Details, typeof(Requirement))]
        public void Given_ConfigurableRequirementModelBinder_When_PageTypeActionArgument_Is_NotNull_Then_ReturnsPropertyType(
            PageType pageType,
            Type expectedModelType,
            HttpActionContext httpActionContext,
            Mock<ModelBindingContext> bindingContextMock,
            ModelMetadata metadata)
        {
            // Arrange 
            bindingContextMock.Object.ModelMetadata = metadata;
            var modelBinder = new ConfigurableRequirementModelBinder();

            httpActionContext.ActionArguments["pageType"] = pageType;
            httpActionContext.Request.Content = new StringContent("{}");

            // Act 
            modelBinder.BindModel(httpActionContext, bindingContextMock.Object);

            // Assert
            bindingContextMock.Object.Model.GetType().Should().Be(expectedModelType);
        }
    }

}
