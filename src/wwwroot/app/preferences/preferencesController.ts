/// <reference path="../typings/_all.d.ts" />

module Antares.Preferences {
    import Dto = Common.Models.Dto;
   
    export class PreferencesController {
        userData: Dto.ICurrentUser;
        enumTypeSalutationFormat: Dto.EnumTypeCode = Dto.EnumTypeCode.SalutationFormat;
        private salutationFormats: any;
        private currentUserResource: Common.Models.Resources.ICurrentUserResourceClass;

        selectedSalutaionFormatId:string;
        defaultSalutationFormatId: string= "";

        constructor(
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $state: ng.ui.IStateService
        ){}

        $onInit = () =>{
            this.defaultSalutationFormatId = this.userData.salutationFormatId;
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        onEnumLoaded = (result: any) =>{
            //find matching code
            var list = result[Dto.EnumTypeCode.SalutationFormat];
            var defaultSalutationFormat: string;

            if (this.defaultSalutationFormatId==undefined || this.defaultSalutationFormatId.length === 0 ) {
                //set default based on division
                if (this.userData.division.code === "Residential") {
                    defaultSalutationFormat = "JohnSmithEsq";
                }
                else {
                    defaultSalutationFormat = "MrJohnSmith";
                }
            }

            if (defaultSalutationFormat != undefined && defaultSalutationFormat.length > 0) {
                var thisVal = <Dto.IEnumTypeItem>_.find(list, (item: Dto.IEnumTypeItem) =>{
                    return item.code === defaultSalutationFormat;
                });

                this.defaultSalutationFormatId = thisVal.id;     
            }               
        }

        save() {
           this.currentUserResource = this.dataAccessService.getCurrentUserResource();
            this.userData.salutationFormatId = this.defaultSalutationFormatId;
            this.currentUserResource
                .update(this.userData)
                .$promise
                .then(() =>{
                    this.$state.go('app.contact-add');
                });
        }
    }
    angular.module('app').controller('PreferencesController', PreferencesController);
}
