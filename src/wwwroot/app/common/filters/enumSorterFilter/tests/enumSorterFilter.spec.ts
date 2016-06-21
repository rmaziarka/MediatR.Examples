/// <reference path="../../../../typings/_all.d.ts" />
module Antares {

    import EnumSorterFilter = Common.Filters.EnumSorterFilter;
    type Dictionary = { [id: string]: string };

    describe('Given enum sort configuration', () => {
        var appConfigMock: Common.Models.IAppConfig =
                {
                    rootUrl: "",
                    fileChunkSizeInBytes: 12,
                    enumOrder: {
                        "OfferStatus": {
                            "New": 2,
                            "Withdrawn": 4,
                            "Rejected": 3,
                            "Accepted": 1
                        }
                    }
                },
            enumSorterFilter: EnumSorterFilter,
            offerStatusToCodeDict: Dictionary = {
                '1': 'New' ,
                '2': 'Withdrawn' ,
                '3': 'Rejected' ,
                '4': 'Accepted' 
            };

        beforeEach(inject((enumProvider: Providers.EnumProvider) =>{

            enumProvider.getEnumCodeById = (statusId: string) =>{
                return offerStatusToCodeDict[statusId];
            }
            
            enumSorterFilter = new EnumSorterFilter(enumProvider, appConfigMock);
        }));
        
        describe('when enum sorting filter', () => {
            it('offers should be sorted properly', () => {
                // arrange
                var offersMock = TestHelpers.OfferGenerator.generateMany(4);
                offersMock[0].statusId = '1';
                offersMock[1].statusId = '2';
                offersMock[2].statusId = '3';
                offersMock[3].statusId = '4';

                // act                
                var sortedOffers = enumSorterFilter.sort(offersMock, "OfferStatus", 'statusId');
                expect(sortedOffers[0].statusId).toEqual("4");
                expect(sortedOffers[1].statusId).toEqual("1");
                expect(sortedOffers[2].statusId).toEqual("3");
                expect(sortedOffers[3].statusId).toEqual("2");
            });
        });

    });

}