namespace KnightFrank.Antares.Domain.UnitTests.Property.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Property.Queries;
    using KnightFrank.Antares.Domain.Property.QueryHandlers;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("PropertyAttributesQueryHandler")]
    [Trait("FeatureTitle", "Property")]
    public class PropertyAttributesQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectQuery_When_Handle_Then_ShouldReturnQueryResult(
        [Frozen] Mock<IReadGenericRepository<PropertyAttributeFormDefinition>> propertyAttributeFormDefinitionRepository,
        PropertyAttributesQuery query,
        PropertyAttributesQueryHandler handler)
        {
            propertyAttributeFormDefinitionRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<PropertyAttributeFormDefinition, object>>>()))
                .Returns(new List<PropertyAttributeFormDefinition>
                {
                    new PropertyAttributeFormDefinition
                    {
                        Attribute = new Dal.Model.Attribute.Attribute(),
                        PropertyAttributeForm = new PropertyAttributeForm
                        {
                            CountryId = query.CountryId,
                            PropertyTypeId = query.PropertyTypeId
                        }
                    }
                }.AsQueryable());

            // Act
            var result = handler.Handle(query);

            // Assert
            propertyAttributeFormDefinitionRepository.Verify(p => p.GetWithInclude(
                It.IsAny<Expression<Func<PropertyAttributeFormDefinition, object>>>()),
                Times.Once());

            Assert.True(result.Attributes.Any());
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectQuery_When_Handle_Then_ShouldReturnDomainException(
            [Frozen] Mock<IReadGenericRepository<PropertyAttributeFormDefinition>> propertyAttributeFormDefinitionRepository,
            PropertyAttributesQuery query,
            PropertyAttributesQueryHandler handler)
        {
            propertyAttributeFormDefinitionRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<PropertyAttributeFormDefinition, object>>>()))
                .Returns(new List<PropertyAttributeFormDefinition>().AsQueryable());

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => handler.Handle(query));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectQuery_When_Handle_Then_ShouldReturnQueryResultOrdered(
        [Frozen] Mock<IReadGenericRepository<PropertyAttributeFormDefinition>> propertyAttributeFormDefinitionRepository,
        PropertyAttributesQuery query,
        PropertyAttributesQueryHandler handler)
        {
            propertyAttributeFormDefinitionRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<PropertyAttributeFormDefinition, object>>>()))
                .Returns(new List<PropertyAttributeFormDefinition>
                {
                    new PropertyAttributeFormDefinition
                    {
                        Attribute = new Dal.Model.Attribute.Attribute(),
                        PropertyAttributeForm = new PropertyAttributeForm
                        {
                            CountryId = query.CountryId,
                            PropertyTypeId = query.PropertyTypeId
                        },
                        Order = 2
                    },
                    new PropertyAttributeFormDefinition
                    {
                       Attribute = new Dal.Model.Attribute.Attribute(),
                        PropertyAttributeForm = new PropertyAttributeForm
                        {
                            CountryId = query.CountryId,
                            PropertyTypeId = query.PropertyTypeId
                        },
                        Order = 1
                    }
                }.AsQueryable());

            // Act
            var result = handler.Handle(query);

            // Assert
            Assert.Equal(1, result.Attributes[0].Order);
        }
    }
}
