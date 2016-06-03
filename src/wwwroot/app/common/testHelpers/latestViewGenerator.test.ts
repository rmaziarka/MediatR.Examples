/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    declare var moment: any;

    export class LatestViewGenerator {
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
    }
}