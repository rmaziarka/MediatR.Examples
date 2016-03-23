/// <reference path="../../../../typings/_all.d.ts" />
/// <reference path="../../../../common/services/dataaccessservice.ts" />
/// <reference path="../../../../common/models/dto/address.ts" />
/// <reference path="../../../../common/core/lowerCaseFirstCharacter.ts" />
/// <reference path="../../../../common/models/dto/addressForm.ts" />

namespace Antares.Common.Component {
    'use strict';

    import AddressForm = Antares.Common.Models.Dto.AddressForm;
    import AddressFormFieldDefinition = Antares.Common.Models.Dto.AddressFormFieldDefinition;
    import Address = Antares.Common.Models.Dto.Address;
    import AddressFormResource = Antares.Common.Models.Resources.IAddressFormResource;

    export class AddressFormViewController {
        addressFormId:string;
        addressForm: any;
        address:Address;
        constructor(private dataAccessService: Services.DataAccessService) {
            this.addressForm = this.dataAccessService.getAddressFormResource().get({id:this.addressFormId});
        }
        public getAddresRowText(row:number):string {

            var addressFormFields:AddressFormFieldDefinition[] = this.addressForm.addressFieldRows[row];

            var addressLine:string = addressFormFields.sort((a:AddressFormFieldDefinition,b:AddressFormFieldDefinition)=>{
                return a.columnOrder - b.columnOrder;
            }).map((item:AddressFormFieldDefinition,index:number)=>{
                var name = Antares.Core.lowerCaseFirstCharacter(item.name);
                return this.address[name];
            }).join(' ');

            return addressLine;
        }
    }

    angular
        .module('app')
        .controller('addressFormViewController', AddressFormViewController);
}