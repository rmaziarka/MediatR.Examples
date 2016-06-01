declare module Antares.Common.Models.Commands {
    import EntityTypeEnum = Antares.Common.Models.Enums.EntityTypeEnum;

    export interface ICreateLatestViewCommand {
        entityId: string;
        entityType: EntityTypeEnum;
    }
}