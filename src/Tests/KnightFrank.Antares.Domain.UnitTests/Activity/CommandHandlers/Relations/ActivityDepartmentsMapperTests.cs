namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("ActivityDepartmentsMapper")]
    public class ActivityDepartmentsMapperTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Guid managingDepartmentTypeId = Guid.NewGuid();
        private readonly Guid standardDepartmentTypeId = Guid.NewGuid();

        [Theory]
        [InlineAutoMoqData(0, 1)]
        [InlineAutoMoqData(0, 2)]
        [InlineAutoMoqData(2, 2)]
        [InlineAutoMoqData(3, 5)]
        public void Given_ValidCommand_When_Handling_Then_ShouldAssignDepartments(
            int existingDepartments,
            int departmentsInCommand,
            [Frozen] Mock<IGenericRepository<Department>> departmentRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityDepartmentsMapper mapper,
            IFixture fixture)
        {
            // Arrange
            var command = new UpdateActivityCommand();
            command.Departments =
                Enumerable.Range(0, departmentsInCommand)
                          .Select(i =>
                          {
                              var department = fixture.Create<UpdateActivityDepartment>();
                              department.DepartmentTypeId = i == 0 ? this.managingDepartmentTypeId : this.standardDepartmentTypeId;
                              return department;
                          })
                          .ToList();
            var activity = fixture.Create<Activity>();
            activity.ActivityDepartments =
                Enumerable.Range(0, existingDepartments).Select(i => fixture.Create<ActivityDepartment>()).ToList();
            departmentRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Department, bool>>>()))
                                .Returns(command.Departments.Select(x => new Department { Id = x.DepartmentId }));
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            activity.ActivityDepartments.Select(x => new { x.DepartmentId, x.DepartmentTypeId })
                    .Should()
                    .BeEquivalentTo(command.Departments.Select(x => new { x.DepartmentId, x.DepartmentTypeId }));
        }

        [Theory]
        [InlineAutoMoqData(0, 0, true)]
        [InlineAutoMoqData(0, 1, true)]
        [InlineAutoMoqData(1, 0, false)]
        [InlineAutoMoqData(1, 1, false)]
        [InlineAutoMoqData(2, 0, true)]
        [InlineAutoMoqData(2, 1, true)]
        public void Given_CommandWithOrWithoutManagingDepartment_When_Handling_Then_ShouldThrowOrSucceed(
            int managingDepartmentsCount,
            int standardDepartmentsCount,
            bool shouldThrow,
            [Frozen] Mock<IGenericRepository<Department>> departmentRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityDepartmentsMapper mapper,
            IFixture fixture)
        {
            // Arrange
            var command = new UpdateActivityCommand();
            command.Departments =
                Enumerable.Range(0, managingDepartmentsCount + standardDepartmentsCount)
                          .Select(i =>
                          {
                              var department = fixture.Create<UpdateActivityDepartment>();
                              department.DepartmentTypeId = i < managingDepartmentsCount
                                  ? this.managingDepartmentTypeId
                                  : this.standardDepartmentTypeId;
                              return department;
                          })
                          .ToList();

            var activity = fixture.Create<Activity>();
            activity.ActivityDepartments = new List<ActivityDepartment>();

            departmentRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Department, bool>>>()))
                                .Returns(command.Departments.Select(x => new Department { Id = x.DepartmentId }));
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

            // Act & Assert
            Action act = () => mapper.ValidateAndAssign(command, activity);
            if (shouldThrow)
            {
                Assert.Throws<BusinessValidationException>(act);
            }
            else
            {
                act.ShouldNotThrow();
            }
        }

        private void SetupEnumTypeItemRepository(Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository, IFixture fixture)
        {
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                      {
                                          this.GetManagingDepartmentType(fixture),
                                          this.GetStandardDepartmentType(fixture)
                                      }.Where(expr.Compile()));
        }

        private EnumTypeItem GetManagingDepartmentType(IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>()
                          .With(i => i.Id, this.managingDepartmentTypeId)
                          .With(i => i.Code, ActivityDepartmentType.Managing.ToString()).Create();
        }

        private EnumTypeItem GetStandardDepartmentType(IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>()
                          .With(i => i.Id, this.standardDepartmentTypeId)
                          .With(i => i.Code, ActivityDepartmentType.Standard.ToString()).Create();
        }
    }
}
