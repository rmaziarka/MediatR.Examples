namespace KnightFrank.Antares.Domain.LatestView.QueryResults
{
    using System;

    public class LatestViewData
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier of the entity.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The creation date of the entity.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data associated with the entity.
        /// </value>
        public object Data { get; set; }
    }
}