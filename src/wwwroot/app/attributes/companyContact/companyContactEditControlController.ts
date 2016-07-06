/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import CompanyContactType = Antares.Common.Models.Enums.CompanyContactType;

    export class CompanyContactEditControlController {
        // binding
        companyContact: Antares.Common.Models.Business.CompanyContactRelation;
        schema: ICompanyContactEditControlSchema;
        config: ICompanyContactControlConfig;
        type: CompanyContactType;

        //fields
        selectedCompanyContacts: Business.CompanyContact[];

        constructor(private eventAggregator: Core.EventAggregator){
            this.setSelectedCompanyContacts();
        }

        setSelectedCompanyContacts = () =>{

            if (this.companyContact == null) {
                this.selectedCompanyContacts = [];
            }
            else {
                this.selectedCompanyContacts = [new Business.CompanyContact(null, this.companyContact.contact, this.companyContact.company)];
            }
        }

        showSelectPanel = () =>{
            this.eventAggregator.publish(new OpenCompanyContactEditPanelEvent(this.type));
        }

        onContactSelected = (companyContacts: Business.CompanyContact[]) => {
            if (companyContacts.length > 0) {
                var model = companyContacts[0];
                this.companyContact = new Antares.Common.Models.Business.CompanyContactRelation(model.contact, model.company);
            }
            else {
                this.companyContact = null;
            }

            this.setSelectedCompanyContacts();

            this.eventAggregator.publish(new Common.Component.CloseSidePanelEvent());
        }
    }

    angular.module('app').controller('CompanyContactEditControlController', CompanyContactEditControlController);
};