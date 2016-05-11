/// <reference path="../../../../../typings/_all.d.ts" />

module Antares.Mock.TranslateFilter {
    import Dto = Common.Models.Dto;

    export function mockTranslateFilterForCharacteristicGroupItems(characteristicGroupItemsParam: Dto.ICharacteristicGroupItem[], translationsParam: string[]){
        var characteristicGroupItems = characteristicGroupItemsParam,
            translations = translationsParam;

        var translateFilterMock = (value: string) =>{
            var translationValue: string = value;

            characteristicGroupItems.forEach((item: Dto.ICharacteristicGroupItem, index: number) =>{
                if (value === item.characteristic.id && !!translations[index]) {
                    translationValue = translations[index];
                }
            });

            return translationValue;
        };

        beforeEach(() =>{
            angular.mock.module(($provide: any) =>{
                $provide.value('dynamicTranslateFilter', translateFilterMock);
            });
        });
    }
}
