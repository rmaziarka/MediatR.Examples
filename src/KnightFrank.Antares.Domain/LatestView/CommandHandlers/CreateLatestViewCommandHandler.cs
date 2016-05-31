namespace KnightFrank.Antares.Domain.LatestView.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.Commands;

    using MediatR;

    public class CreateLatestViewCommandHandler : IRequestHandler<CreateLatestViewCommand, Guid>
    {
        private readonly IGenericRepository<LatestView> latestViewRepository;
        private readonly IReadGenericRepository<User> userRepository;

        public CreateLatestViewCommandHandler(IGenericRepository<LatestView> latestViewRepository, IReadGenericRepository<User> userRepository)
        {
            this.latestViewRepository = latestViewRepository;
            this.userRepository = userRepository;
        }

        public Guid Handle(CreateLatestViewCommand message)
        {
            var latestView = AutoMapper.Mapper.Map<LatestView>(message);
            latestView.UserId = this.userRepository.Get().First().Id;
            latestView.CreatedDate = DateTime.UtcNow;

            this.latestViewRepository.Add(latestView);
            this.latestViewRepository.Save();

            return latestView.Id;
        }
    }
}
