namespace KnightFrank.Antares.Domain.UnitTests.Tenancy.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Tenancy.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Tenancy.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class TenancyContactMapperTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldUpdateLandlords(
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           [Frozen] Mock<IEntityValidator> entityValidator,
           Tenancy tenancy,
           CreateTenancyCommand command,
           TenancyContactMapper mapper,
           IFixture fixture)
        {
            // Arrange
            command.TenantContacts = new List<Guid>();
            tenancy.Contacts = new List<TenancyContact>();
            EnumTypeItem landLordEnumTypeItem = fixture.BuildEnumTypeItem(Domain.Common.Enums.EnumType.TenancyContactType.ToString(), TenancyContactType.Landlord.ToString());
            EnumTypeItem tenantEnumTypeItem = fixture.BuildEnumTypeItem(Domain.Common.Enums.EnumType.TenancyContactType.ToString(), TenancyContactType.Tenant.ToString());

            var enumTypeItems = new List<EnumTypeItem>
            {
                landLordEnumTypeItem,
                tenantEnumTypeItem
            };

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                .Returns((Expression<Func<EnumTypeItem, bool>> expr) => enumTypeItems.Where(expr.Compile()));

            // Act
            mapper.ValidateAndAssign(command, tenancy);

            // Assert
            List<TenancyContact> addedLandlords = tenancy.Contacts.Where(x => x.ContactTypeId == landLordEnumTypeItem.Id).ToList();
            addedLandlords.Count.Should().Be(command.LandlordContacts.Count);
            addedLandlords.Select(x => x.ContactId).ShouldBeEquivalentTo(command.LandlordContacts);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldUpdateTenants(
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           [Frozen] Mock<IEntityValidator> entityValidator,
           Tenancy tenancy,
           CreateTenancyCommand command,
           TenancyContactMapper mapper,
           IFixture fixture)
        {
            // Arrange
            command.LandlordContacts = new List<Guid>();
            tenancy.Contacts = new List<TenancyContact>();
            EnumTypeItem landLordenumTypeItem = fixture.BuildEnumTypeItem(Domain.Common.Enums.EnumType.TenancyContactType.ToString(), TenancyContactType.Landlord.ToString());
            EnumTypeItem tenantEnumTypeItem = fixture.BuildEnumTypeItem(Domain.Common.Enums.EnumType.TenancyContactType.ToString(), TenancyContactType.Tenant.ToString());

            var enumTypeItems = new List<EnumTypeItem>
            {
                landLordenumTypeItem,
                tenantEnumTypeItem
            };

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                .Returns((Expression<Func<EnumTypeItem, bool>> expr) => enumTypeItems.Where(expr.Compile()));

            // Act
            mapper.ValidateAndAssign(command, tenancy);

            // Assert
            List<TenancyContact> addedLandlords = tenancy.Contacts.Where(x => x.ContactTypeId == tenantEnumTypeItem.Id).ToList();
            addedLandlords.Count.Should().Be(command.TenantContacts.Count);
            addedLandlords.Select(x => x.ContactId).ShouldBeEquivalentTo(command.TenantContacts);
        }
    }
}