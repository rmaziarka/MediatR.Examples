namespace KnightFrank.Antares.Domain.UnitTests.RequirementNote.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.RequirementNote.Queries;
    using KnightFrank.Antares.Domain.RequirementNote.QueryHandlers;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("RequirementNoteQueryHandler")]
    [Trait("FeatureTitle", "RequirementNote")]
    public class RequirementNoteQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistingRequirementNoteId_When_Handling_Then_ShouldReturnNotNullValue(
            Guid requirementNoteId,
            [Frozen] Mock<IReadGenericRepository<RequirementNote>> requirementNoteRepository,
            ICollection<RequirementNote> requirementNotes,
            RequirementNote expectedRequirementNote,
            RequirementNoteQuery query,
            RequirementNoteQueryHandler handler)
        {
            // Arrange
            query.Id = requirementNoteId;
            expectedRequirementNote.Id = requirementNoteId;

            requirementNotes.Add(expectedRequirementNote);
            requirementNoteRepository.Setup(r => r.Get()).Returns(requirementNotes.AsQueryable());

            // Act
            RequirementNote requirementNote = handler.Handle(query);

            // Assert
            requirementNote.Should().NotBeNull();
            requirementNote.ShouldBeEquivalentTo(expectedRequirementNote, options => options.IncludingProperties().ExcludingMissingMembers());
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingRequirementNoteId_When_Handling_Then_ShouldReturnNull(
            RequirementNoteQuery query,
            [Frozen] Mock<IReadGenericRepository<RequirementNote>> requirementNoteRepository,
            RequirementNoteQueryHandler handler)
        {
            // Arrange
            requirementNoteRepository.Setup(r => r.Get()).Returns(new RequirementNote[] { }.AsQueryable());

            // Act
            RequirementNote requirementNote = handler.Handle(query);

            // Assert
            requirementNote.Should().BeNull();
        }
    }
}
