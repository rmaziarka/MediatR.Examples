module Antares.Common.Models.Dto {
    export class Contact {

        id:string;

        constructor(public firstName:string = '',
                    public surname:string = '',
                    public title:string = '') {

        }
    }
}