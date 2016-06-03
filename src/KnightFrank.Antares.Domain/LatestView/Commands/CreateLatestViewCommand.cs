namespace KnightFrank.Antares.Domain.LatestView.Commands
{
    using System;

    using Dal.Model.Common;

    using MediatR;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class CreateLatestViewCommand : IRequest<Guid>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityTypeEnum EntityType { get; set; }
        public Guid EntityId { get; set; }
    }
}
