/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Component {
    import User = Common.Models.Dto.IUser;
    import DepartmentUserResourceParameters = Common.Models.Resources.IDepartmentUserResourceParameters;
    import Business = Common.Models.Business;
    import ISearchUserControlSchema = Attributes.ISearchUserControlSchema;

    export class SearchUserController {
        // Bindings 
        public config: any;
        public schema: ISearchUserControlSchema;
        public user: User;

        // Properties
        public isInEditMode: boolean = true;
        public searchOptions: SearchOptions = new SearchOptions();

        constructor(
            private dataAccessService: Services.DataAccessService) {
        }

        public changeUser = (user: Business.User) => {
            this.user = user;
            this.isInEditMode = false;
        }

        public editUser = () => {
            this.isInEditMode = true;
        }

        public cancelChangeUser = () => {
            this.isInEditMode = false;
        }

        public getUsersQuery = (searchValue: string): DepartmentUserResourceParameters => {
            return { partialName: searchValue, take: this.schema.usersSearchMaxCount, 'excludedIds[]': [] };
        }

        public getUsers = (searchValue: string) => {
            var query = this.getUsersQuery(searchValue);

            return this.dataAccessService
                .getDepartmentUserResource()
                .query(query)
                .$promise
                .then((users: any) => {
                    return users.map((user: Common.Models.Dto.IUser) => { return new Common.Models.Business.User(<Common.Models.Dto.IUser>user); });
                });
        }
    }

    angular.module('app').controller('SearchUserController', SearchUserController);
};