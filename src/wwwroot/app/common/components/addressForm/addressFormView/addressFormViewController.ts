/// <reference path="../../../../typings/_all.d.ts" />

namespace Antares.Common.Component {
    'use strict';
    
    export class AddressFormViewController {
        address:any;
        constructor() {
            
        }
    }

    angular
        .module('app')
        .controller('addressFormViewController', AddressFormViewController);
}