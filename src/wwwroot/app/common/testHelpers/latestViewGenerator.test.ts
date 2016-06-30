/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    declare var moment: any;

    export class LatestViewGenerator {
        public static generateActivityList(listLength: number): Dto.ILatestViewResultItem {
            var item: Dto.ILatestViewResultItem = {
                entityTypeCode: 'Activity',
                list: LatestViewGenerator.generateManyActivityListItems(listLength)
            };

            return item;
        }

        public static generateActivityListItem(): Dto.ILatestViewData {
            return {
                id: LatestViewGenerator.makeRandom('id'),
                createDate: moment().days(1),
                data: Mock.AddressForm.FullAddress
            }
        }

        public static generateManyActivityListItems(n: number): Dto.ILatestViewData[] {
            return _.times(n, LatestViewGenerator.generateActivityListItem);
        }

        public static generatePropertyList(listLength: number): Dto.ILatestViewResultItem {
            var item: Dto.ILatestViewResultItem = {
                entityTypeCode: 'Property',
                list: LatestViewGenerator.generateManyPropertyListItems(listLength)
            };
            
            return item;
        }

        public static generatePropertyListItem(): Dto.ILatestViewData{
            return {
                id: LatestViewGenerator.makeRandom('id'),
                createDate: moment().days(1),
                data: Mock.AddressForm.FullAddress
            }
        }

        public static generateManyPropertyListItems(n: number): Dto.ILatestViewData[] {
            return _.times(n, LatestViewGenerator.generatePropertyListItem);
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }

        public static generateRequirementList(listLength: number): Dto.ILatestViewResultItem {
            var item: Dto.ILatestViewResultItem = {
                entityTypeCode: 'Requirement',
                list: LatestViewGenerator.generateManyRequirementListItems(listLength)
            };

            return item;
        }

        public static generateRequirementListItem(): Dto.ILatestViewData {
            return {
                id: LatestViewGenerator.makeRandom('id'),
                createDate: moment().days(1),
                data: ContactGenerator.generateMany(3)
            }
        }

        public static generateManyRequirementListItems(n: number): Dto.ILatestViewData[] {
            return _.times(n, LatestViewGenerator.generateRequirementListItem);
        }

        public static generateCompanyList(listLength: number): Dto.ILatestViewResultItem {
            var item: Dto.ILatestViewResultItem = {
                entityTypeCode: 'Company',
                list: LatestViewGenerator.generateManyCompanyListItems(listLength)
            };

            return item;
        }

        public static generateCompanyListItem(): Dto.ILatestViewData {
            return {
                id: LatestViewGenerator.makeRandom('id'),
                createDate: moment().days(1),
                data: CompanyGenerator.generate()
            }
        }

        public static generateManyCompanyListItems(n: number): Dto.ILatestViewData[] {
            return _.times(n, LatestViewGenerator.generateCompanyListItem);
        }
    }
}