namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;
    using PropertyType = KnightFrank.Antares.Domain.Common.Enums.PropertyType;

    public class ActivityChainMapperTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_CommandChainTransactionShouldMapToActivityChainTransactions(
            [Frozen] Mock<IGenericRepository<ChainTransaction>> chainTransactionRepository,
            [Frozen] Mock<IEnumParser> enumParser,
            [Frozen] Mock<IControlsConfiguration<Tuple<PropertyType, ActivityType>>> activityControlsConfiguration,
            ActivityChainMapper mapper)
        {
            // Arrange
            Guid guid = Guid.Parse("60FAB3B3-2584-41ED-ADA5-05111009C749");
            var command = new UpdateActivityCommand
            {
                ChainTransactions = new List<UpdateChainTransaction>
                {
                    new UpdateChainTransaction { Id = guid }
                }
            };
            var activity = new Activity
            {
                ChainTransactions = new List<ChainTransaction>
                {
                    new ChainTransaction { Id = guid },
                    new ChainTransaction { Id = Guid.NewGuid(), ParentId = guid }
                },
                Property = new Property()
            };

            enumParser.Setup(x => x.Parse<Dal.Model.Property.Activities.ActivityType, ActivityType>(It.IsAny<Guid>()))
                      .Returns(ActivityType.FreeholdSale);

            enumParser.Setup(x => x.Parse<Dal.Model.Property.PropertyType, PropertyType>(It.IsAny<Guid>()))
                      .Returns(PropertyType.House);

            activityControlsConfiguration.Setup(x => x.GetInnerFieldsState(PageType.Update,
                It.IsAny<Tuple<PropertyType, ActivityType>>(), command))
                                         .Returns(new List<InnerFieldState>
                                         {
                                             new InnerFieldState
                                             {
                                                 ControlCode = ControlCode.Offer_UpwardChain,
                                                 Readonly = false,
                                                 Hidden = false
                                             }
                                         });

            chainTransactionRepository.Setup(x => x.Delete(It.IsAny<ChainTransaction>()))
                                      .Callback<ChainTransaction>(x => activity.ChainTransactions.Remove(x));

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            chainTransactionRepository.Verify(r => r.Delete(It.IsAny<ChainTransaction>()), Times.Exactly(1));

            activity.ChainTransactions.ShouldBeEquivalentTo(command.ChainTransactions, options => options.ExcludingMissingMembers());
        }

        [Theory]
        [AutoMoqData]
        public void
            Given_ValidCommand_When_HandlingChainsOnLetting_Then_CommandChainTransactionShouldNotMapToActivityChainTransactions(
            [Frozen] Mock<IGenericRepository<ChainTransaction>> chainTransactionRepository,
            [Frozen] Mock<IEnumParser> enumParser,
            [Frozen] Mock<IControlsConfiguration<Tuple<PropertyType, ActivityType>>> activityControlsConfiguration,
            ActivityChainMapper mapper)
        {
            // Arrange
            var command = new UpdateActivityCommand
            {
                ChainTransactions = new List<UpdateChainTransaction>
                {
                    new UpdateChainTransaction()
                }
            };
            var activity = new Activity
            {
                Property = new Property(),
                ChainTransactions = new List<ChainTransaction>()
            };

            enumParser.Setup(x => x.Parse<Dal.Model.Property.Activities.ActivityType, ActivityType>(It.IsAny<Guid>()))
                      .Returns(ActivityType.FreeholdSale);

            enumParser.Setup(x => x.Parse<Dal.Model.Property.PropertyType, PropertyType>(It.IsAny<Guid>()))
                      .Returns(PropertyType.House);

            activityControlsConfiguration.Setup(x => x.GetInnerFieldsState(PageType.Update,
                It.IsAny<Tuple<PropertyType, ActivityType>>(), command))
                                         .Returns(new List<InnerFieldState>
                                         {
                                             new InnerFieldState
                                             {
                                                 ControlCode = ControlCode.Offer_UpwardChain,
                                                 Readonly = true,
                                                 Hidden = true
                                             }
                                         });

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            activity.ChainTransactions.Should().BeNullOrEmpty();
        }

        [Theory]
        [MemberData("InvalidChainTransactions")]
        public void Given_InvalidCommand_When_Handling_Then_ShouldThrowException(
            List<UpdateChainTransaction> chainTransactions)
        {
            // Arrange
            var activity = new Activity
            {
                Property = new Property()
            };
            var command = new UpdateActivityCommand
            {
                ChainTransactions = chainTransactions
            };

            var fixture = new Fixture();
            var entityValidator = fixture.Freeze<Mock<IEntityValidator>>();
            var enumTypeValidator = fixture.Freeze<Mock<IEnumTypeItemValidator>>();
            var chainTransactionRepository = fixture.Freeze<Mock<IGenericRepository<ChainTransaction>>>();
            var controlConfiguration = fixture.Freeze<Mock<IControlsConfiguration<Tuple<PropertyType, ActivityType>>>>();
            var enumParser = fixture.Freeze<Mock<IEnumParser>>();
            enumParser.Setup(x => x.Parse<Dal.Model.Property.Activities.ActivityType, ActivityType>(It.IsAny<Guid>()))
                      .Returns(ActivityType.FreeholdSale);

            enumParser.Setup(x => x.Parse<Dal.Model.Property.PropertyType, PropertyType>(It.IsAny<Guid>()))
                      .Returns(PropertyType.House);

            var mapper = new ActivityChainMapper(entityValidator.Object, enumTypeValidator.Object,
                chainTransactionRepository.Object, controlConfiguration.Object, enumParser.Object);

            // Act
            // Assert
            Assert.Throws<BusinessValidationException>(() => mapper.ValidateAndAssign(command, activity));
        }

        public static IEnumerable<object[]> InvalidChainTransactions => new[]
        {
            new object[]
            {
                new List<UpdateChainTransaction>
                {
                    new UpdateChainTransaction
                    {
                        IsEnd = true,
                    },
                    new UpdateChainTransaction
                    {
                        IsEnd = true,
                    }
                }
            },
            new object[]
            {
                new List<UpdateChainTransaction>
                {
                    new UpdateChainTransaction
                    {
                        AgentContactId = Guid.Parse("60FAB3B3-2584-41ED-ADA5-05111009C749"),
                        AgentUserId = Guid.Parse("50FAB333-2584-41ED-ADA5-051110091749")
                    }
                }
            },
            new object[]
            {
                new List<UpdateChainTransaction>
                {
                    new UpdateChainTransaction
                    {
                        Vendor =
                            "50FAB333-2584-41ED-ADA5-05111009174950FAB333-2584-41ED-ADA5-05111009174950FAB333-2584-41ED-ADA5-05111009174950FAB333-2584-41ED-ADA5-0511100917491"
                    }
                }
            },
            new object[]
            {
                new List<UpdateChainTransaction>
                {
                    new UpdateChainTransaction
                    {
                        Id = Guid.Parse("60FAB3B3-2584-41ED-ADA5-05111009C749"),
                        IsEnd = true
                    },
                    new UpdateChainTransaction
                    {
                        ParentId = Guid.Parse("60FAB3B3-2584-41ED-ADA5-05111009C749")
                    }
                }
            }
        };
    }
}
