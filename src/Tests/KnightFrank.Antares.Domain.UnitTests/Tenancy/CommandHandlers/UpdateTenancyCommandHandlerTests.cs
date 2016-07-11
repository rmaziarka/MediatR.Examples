namespace KnightFrank.Antares.Domain.UnitTests.Tenancy.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Tenancy.CommandHandlers;
    using KnightFrank.Antares.Domain.Tenancy.Commands;
    using KnightFrank.Antares.Domain.Tenancy.Relations;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class UpdateTenancyCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldUpdateTenancy(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Tenancy>> tenancyRepository,
            [Frozen] Mock<ITenancyReferenceMapper<TenancyTerm>> termMapper,
            [Frozen] Mock<IAttributeValidator<Tuple<Domain.Common.Enums.TenancyType, Domain.Common.Enums.RequirementType>>> attributeValidator,
            Requirement requirement,
            RequirementType requirementType,
            TenancyType tenancyType,
            Tenancy tenancy,
            UpdateTenancyCommand command,
            UpdateTenancyCommandHandler handler)
        {
            // Arrange
            tenancyType.EnumCode = Domain.Common.Enums.RequirementType.ResidentialLetting.ToString();
            requirementType.EnumCode = Domain.Common.Enums.RequirementType.ResidentialLetting.ToString();
            requirement.RequirementType = requirementType;
            tenancy.Requirement = requirement;
            tenancy.TenancyType = tenancyType;

            tenancyRepository.Setup(
                x => x.GetWithInclude(
                    It.IsAny<Expression<Func<Tenancy, bool>>>(),
                    It.IsAny<Expression<Func<Tenancy, object>>[]>())).Returns(new[] { tenancy });

            // Act
            Guid result = handler.Handle(command);

            // Assert
            attributeValidator.Verify(x => x.Validate(
                PageType.Update,
                It.Is<Tuple<Domain.Common.Enums.TenancyType, Domain.Common.Enums.RequirementType>>(t => t.Item1 == Domain.Common.Enums.TenancyType.ResidentialLetting && t.Item2 == Domain.Common.Enums.RequirementType.ResidentialLetting),
                command));

            termMapper.Verify(x => x.ValidateAndAssign(command, tenancy));
            tenancyRepository.Verify(x => x.Save(), Times.Once);

            result.Should().Be(tenancy.Id);
        }
    }
}