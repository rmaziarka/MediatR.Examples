namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;
    public class CreateActivityCommandHandlerTests : IClassFixture<CreateActivityCommandHandler>
    {
        private readonly IFixture fixture;

        public CreateActivityCommandHandlerTests()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());
            this.fixture.Behaviors.Clear();
            this.fixture.RepeatCount = 1;
            this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}