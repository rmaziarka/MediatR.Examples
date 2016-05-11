namespace KnightFrank.Antares.Domain.UnitTests.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using Xunit;

    [Collection("CollectionValidator")]
    [Trait("FeatureTitle", "Common validators")]
    public class CollectionValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_TwoDifferentCollections_Then_ExceptionThrown(
            CollectionValidator collectionValidator, 
            List<Guid> collection, 
            List<Guid> otherCollection)
        {
            Assert.Throws<BusinessValidationException>(() => collectionValidator.CollectionContainsAll(collection, otherCollection, ErrorMessage.Missing_Requirement_Attendees_Id));
        }

        [Theory]
        [AutoMoqData]
        public void Given_TwoIdenticalCollections_Then_NoExceptionThrown(
            CollectionValidator collectionValidator,
            List<Guid> collection)
        {
            collectionValidator.CollectionContainsAll(collection, collection, ErrorMessage.Missing_Requirement_Attendees_Id);
        }

        [Theory]
        [AutoMoqData]
        public void Given_SubsetOfTheCollection_Then_NoExceptionThrown(
            CollectionValidator collectionValidator,
            List<Guid> collection)
        {
            List<Guid> subset = collection.Take(1).ToList();

            collectionValidator.CollectionContainsAll(collection, subset, ErrorMessage.Missing_Requirement_Attendees_Id);
        }
    }
}
