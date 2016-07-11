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
    using KnightFrank.Antares.Domain.Tenancy.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Tenancy.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;
    public class CreateTenancyCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldSaveTenancy(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            [Frozen] Mock<IGenericRepository<TenancyType>> tenancyTypeRepository,
            [Frozen] Mock<IGenericRepository<Tenancy>> tenancyRepository,
            [Frozen] Mock<ITenancyReferenceMapper<TenancyTerm>> termMapper,
            [Frozen] Mock<IReferenceMapper<CreateTenancyCommand, Tenancy, Contact>> tenancyContactsMapper,
            [Frozen] Mock<IAttributeValidator<Tuple<Domain.Common.Enums.TenancyType, Domain.Common.Enums.RequirementType>>> attributeValidator,
            Requirement requirement,
            RequirementType requirementType,
            TenancyType tenancyType,
            Tenancy tenancy,
            CreateTenancyCommand command,
            CreateTenancyCommandHandler handler)
        {
            // Arrange
            tenancyType.EnumCode = Domain.Common.Enums.RequirementType.ResidentialLetting.ToString();
            requirementType.EnumCode = Domain.Common.Enums.RequirementType.ResidentialLetting.ToString();
            requirement.RequirementType = requirementType;
            
            requirementRepository.Setup(
                x => x.GetWithInclude(
                    It.IsAny<Expression<Func<Requirement, bool>>>(),
                    It.IsAny<Expression<Func<Requirement, object>>>())).Returns(new[] { requirement });

            tenancyTypeRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<TenancyType, bool>>>())).Returns(new List<TenancyType> { tenancyType });
            Tenancy addTenancy = null;
            tenancyRepository.Setup(x => x.Add(It.IsAny<Tenancy>())).Callback<Tenancy>(x => addTenancy = x);

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists<Activity>(command.ActivityId));
            entityValidator.Verify(x => x.EntityExists<Requirement>(command.RequirementId));

            entityValidator.Verify(x => x.EntitiesExist<Contact>(command.LandlordContacts));
            entityValidator.Verify(x => x.EntitiesExist<Contact>(command.TenantContacts));

            attributeValidator.Verify(x => x.Validate(
                PageType.Create, 
                It.Is<Tuple<Domain.Common.Enums.TenancyType, Domain.Common.Enums.RequirementType>>(t => t.Item1 == Domain.Common.Enums.TenancyType.ResidentialLetting && t.Item2 == Domain.Common.Enums.RequirementType.ResidentialLetting), 
                command));

            tenancyContactsMapper.Verify(x => x.ValidateAndAssign(command, It.Is<Tenancy>(y => y.Contacts.Count == 0)));
            termMapper.Verify(x => x.ValidateAndAssign(command, It.Is<Tenancy>(y => y.Terms.Count == 0)));
            tenancyRepository.Verify(x => x.Save(), Times.Once);

            addTenancy.RequirementId.Should().Be(command.RequirementId);
            addTenancy.ActivityId.Should().Be(command.ActivityId);
            addTenancy.TenancyTypeId.Should().Be(tenancyType.Id);
        }
    }
}