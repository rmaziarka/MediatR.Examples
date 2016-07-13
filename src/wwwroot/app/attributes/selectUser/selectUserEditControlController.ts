/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import SearchOptions = Antares.Common.Component.SearchOptions;
    import DepartmentUserResourceParameters = Common.Models.Resources.IDepartmentUserResourceParameters;

    export class SelectUserEditControlController {
        // bindings 
        public config: ISelectUserEditControlConfig;
        public schema: IEnumItemEditControlSchema;
        public user: Business.User;
        
        // controller
        public searchOptions: SearchOptions = new SearchOptions();
        public usersSearchMaxCount: number = 100;
        public isEditMode: boolean = false;

        constructor(private dataAccessService: Services.DataAccessService){
        }

        public edit = () => {
            this.isEditMode = true;
        }

        public changeUser = (user: Dto.IUser) => {
            this.user = new Business.User(user);

            this.isEditMode = false;
        }

        public removeUser = (user: Dto.IUser) => {
            this.user = null;

            this.isEditMode = false;
        }

        public cancelChange = () => {
            this.isEditMode = false;
        }
        
        public getUsersQuery = (searchValue: string): DepartmentUserResourceParameters =>{
            var excludedIds: string[] = [];

            if (this.user) {
                excludedIds.push(this.user.id);
            }

            return { partialName: searchValue, take: this.usersSearchMaxCount, 'excludedIds[]': excludedIds };
        }

        public getUsers = (searchValue: string) => {
            var query = this.getUsersQuery(searchValue);

            return this.dataAccessService
                .getDepartmentUserResource()
                .query(query)
                .$promise
                .then((users: any) => {
                    return users.map((user: Common.Models.Dto.IUser) => new Common.Models.Business.User(user) );
                });
        }
    }

    angular.module('app').controller("SelectUserEditControlController", SelectUserEditControlController);
};