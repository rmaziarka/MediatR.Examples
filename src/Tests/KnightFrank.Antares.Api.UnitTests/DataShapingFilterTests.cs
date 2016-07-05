namespace KnightFrank.Antares.Api.UnitTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class DataShapingFilterTests
    {
        [AutoMoqData]
        [Theory]
        public void Given_EmptyExecutedContext_When_OnActionExecuted_Then_NullifyDisallowedValuesNotInvoked(
            [Frozen] Mock<INinjectInstanceResolver> ninjectInstanceResolver,
            [Frozen] Mock<IEntityMapper<Activity>> activityEntityMapper,
            [Frozen] Mock<IEntityMapper<Requirement>> requirementEntityMapper,
            [Frozen] Mock<IEntityMapper<Offer>> offerEntityMapper,
            DataShapingFilter dataShapingFilter)
        {
            // Arrange
            var emptyContext = new HttpActionExecutedContext();
            ninjectInstanceResolver.Setup(x => x.GetInstance<IEntityMapper<Activity>>()).Returns(activityEntityMapper.Object);
            ninjectInstanceResolver.Setup(x => x.GetInstance<IEntityMapper<Requirement>>()).Returns(requirementEntityMapper.Object);
            ninjectInstanceResolver.Setup(x => x.GetInstance<IEntityMapper<Offer>>()).Returns(offerEntityMapper.Object);

            // Act
            dataShapingFilter.OnActionExecuted(emptyContext);

            // Assert
            activityEntityMapper.Verify(c => c.NullifyDisallowedValues(It.IsAny<Activity>(), It.IsAny<PageType>()), Times.Never);
            requirementEntityMapper.Verify(c => c.NullifyDisallowedValues(It.IsAny<Requirement>(), It.IsAny<PageType>()), Times.Never);
            offerEntityMapper.Verify(c => c.NullifyDisallowedValues(It.IsAny<Offer>(), It.IsAny<PageType>()), Times.Never);
        }

        [AutoMoqData]
        [Theory]
        public void Given_ExecutedContextWithActivity_When_OnActionExecuted_Then_NullifyDisallowedValuesInvokedOnce(
            [Frozen] Mock<INinjectInstanceResolver> ninjectInstanceResolver, 
            [Frozen] Mock<IEntityMapper<Activity>> activityEntityMapper,
            DataShapingFilter dataShapingFilter,
            Activity activity)
        {
            // Arrange
            HttpActionExecutedContext activityContext = this.CreateContext(activity);
            ninjectInstanceResolver.Setup(x => x.GetInstance<IEntityMapper<Activity>>()).Returns(activityEntityMapper.Object);

            // Act
            dataShapingFilter.OnActionExecuted(activityContext);

            // Assert
            activityEntityMapper.Verify(c => c.NullifyDisallowedValues(activity, PageType.Details), Times.Once);
        }

        [AutoMoqData]
        [Theory]
        public void Given_ExecutedContextWithOffer_When_OnActionExecuted_Then_NullifyDisallowedValuesInvokedOnce(
            [Frozen] Mock<INinjectInstanceResolver> ninjectInstanceResolver, 
            [Frozen] Mock<IEntityMapper<Activity>> activityEntityMapper,
            DataShapingFilter dataShapingFilter,
            Offer offer,
            Activity activity)
        {
            // Arrange
            offer.Activity = activity;
            HttpActionExecutedContext activityContext = this.CreateContext(offer);
            ninjectInstanceResolver.Setup(x => x.GetInstance<IEntityMapper<Activity>>()).Returns(activityEntityMapper.Object);

            // Act
            dataShapingFilter.OnActionExecuted(activityContext);
            
            // Assert
            activityEntityMapper.Verify(c => c.NullifyDisallowedValues(activity, PageType.Details), Times.Once);
        }

        [AutoMoqData]
        [Theory]
        public void Given_ExecutedContextWithPropertyWithActivities_When_OnActionExecuted_Then_NullifyDisallowedValuesInvokedForEachActivity(
            [Frozen] Mock<INinjectInstanceResolver> ninjectInstanceResolver,
            [Frozen] Mock<IEntityMapper<Activity>> activityEntityMapper,
            DataShapingFilter dataShapingFilter,
            Property property,
            List<Activity> activities)
        {
            // Arrange
            property.Activities = activities;

            HttpActionExecutedContext activityContext = this.CreateContext(property);
            ninjectInstanceResolver.Setup(x => x.GetInstance<IEntityMapper<Activity>>()).Returns(activityEntityMapper.Object);

            // Act
            dataShapingFilter.OnActionExecuted(activityContext);

            // Assert
            activityEntityMapper.Verify(c => c.NullifyDisallowedValues(It.IsAny<Activity>(), PageType.Details), Times.Exactly(activities.Count));
        }

        private HttpActionExecutedContext CreateContext<T>(T entity)
        {
            return new HttpActionExecutedContext
            {
                ActionContext = new HttpActionContext
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = new HttpRequestMessage()
                    }
                },
                Response = new HttpResponseMessage
                {
                    Content = new ObjectContent<T>(entity, new JsonMediaTypeFormatter())
                }
            };
        }
    }
}
