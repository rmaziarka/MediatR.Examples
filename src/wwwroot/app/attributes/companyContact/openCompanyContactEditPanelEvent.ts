module Antares.Attributes {
    import Event = Core.Event;
    import CompanyContactType = Common.Models.Enums.CompanyContactType;

    export class OpenCompanyContactEditPanelEvent extends Event {
        private openCompanyContactEditPanelEvent: boolean;

        constructor(public type?: CompanyContactType){
            super();
        }

        getKey(): string { return "companyContactEdit.openPanel"; }
    }
}