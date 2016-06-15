namespace KnightFrank.Antares.Domain.UnitTests.Property.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Enum;
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

    [Collection("PropertyTypesQueryHandler")]
    [Trait("FeatureTitle", "Property")]
    public class PropertyTypesQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectQuery_When_Handle_Then_ShouldReturnQueryResult(
        [Frozen] Mock<IReadGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository,
        [Frozen] Mock<IReadGenericRepository<PropertyTypeLocalised>> propertyTypeLocalisedRepository,
        PropertyTypeQuery query,
        PropertyTypeQueryHandler handler)
        {
            PropertyType parentPropertyType  = new PropertyType { ParentId = null }; 
            
            propertyTypeDefinitionRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<PropertyTypeDefinition, object>>>()))
                .Returns(new List<PropertyTypeDefinition>
                {
                    new PropertyTypeDefinition
                    {
                        Division = new EnumTypeItem { Code = query.DivisionCode},
                        Country = new Country { IsoCode = query.CountryCode},
                        PropertyType = parentPropertyType,
                    }
                }.AsQueryable());

            propertyTypeLocalisedRepository.Setup(x => x.Get())
                .Returns(new List<PropertyTypeLocalised>
                {
                    new PropertyTypeLocalised
                    {
                        Locale = new Locale { IsoCode = query.LocaleCode},
                        PropertyType = parentPropertyType
                    }
                }.AsQueryable());

            // Act
            var result = handler.Handle(query);

            // Assert
            propertyTypeDefinitionRepository.Verify(p => p.GetWithInclude(
                It.IsAny<Expression<Func<PropertyTypeDefinition, object>>>()), 
                Times.Once());

            propertyTypeLocalisedRepository.Verify(p => p.Get(),
               Times.Once());

            Assert.True(result.PropertyTypes.Any());
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectQuery_When_Handle_Then_ShouldReturnDomainException(
        [Frozen] Mock<IReadGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository,
        [Frozen] Mock<IReadGenericRepository<PropertyTypeLocalised>> propertyTypeLocalisedRepository,
        PropertyTypeQuery query,
        PropertyTypeQueryHandler handler)
        {
            propertyTypeDefinitionRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<PropertyTypeDefinition, object>>>()))
                .Returns(new List<PropertyTypeDefinition>().AsQueryable());

            propertyTypeLocalisedRepository.Setup(x => x.Get())
                .Returns(new List<PropertyTypeLocalised>().AsQueryable());

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => handler.Handle(query));
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectQuery_When_Handle_Then_ShouldReturnQueryResultOrdered(
        [Frozen] Mock<IReadGenericRepository<PropertyTypeDefinition>> propertyTypeDefinitionRepository,
        [Frozen] Mock<IReadGenericRepository<PropertyTypeLocalised>> propertyTypeLocalisedRepository,
        PropertyTypeQuery query,
        PropertyTypeQueryHandler handler)
        {
            PropertyType parentPropertyType = new PropertyType { ParentId = null };

            propertyTypeDefinitionRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<PropertyTypeDefinition, object>>>()))
                .Returns(new List<PropertyTypeDefinition>
                {
                    new PropertyTypeDefinition
                    {
                        Division = new EnumTypeItem { Code = query.DivisionCode},
                        Country = new Country { IsoCode = query.CountryCode},
                        PropertyType = parentPropertyType,
                        Order = 2
                    },
                    new PropertyTypeDefinition
                    {
                        Division = new EnumTypeItem { Code = query.DivisionCode},
                        Country = new Country { IsoCode = query.CountryCode},
                        PropertyType = parentPropertyType,
                        Order = 1
                    }
                }.AsQueryable());

            propertyTypeLocalisedRepository.Setup(x => x.Get())
                .Returns(new List<PropertyTypeLocalised>
                {
                    new PropertyTypeLocalised
                    {
                        Locale = new Locale { IsoCode = query.LocaleCode},
                        PropertyType = parentPropertyType
                    }
                }.AsQueryable());

            // Act
            var result = handler.Handle(query);

            // Assert
            Assert.Equal(1, result.PropertyTypes[0].Order);
        }
    }
}
