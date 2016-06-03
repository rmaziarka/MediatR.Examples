/// <reference path="../../../../typings/_all.d.ts" />
module Antares {

    import EnumSorterFilter = Common.Filters.EnumSorterFilter;

    describe('Given enum sort configuration', () => {
        var filter: ng.IFilterService,
            appConfigMock: Common.Models.IAppConfig =
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
            enumSorterFilter: EnumSorterFilter;

        // beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) => {
        //     $provide.value('enumService', Antares.Mock.EnumServiceMock);
        // }));

        beforeEach(inject((enumService: Antares.Services.EnumService
        ) => {
            var enumItems = [
                { id: '1', code: 'New' },
                { id: '2', code: 'Withdrawn' },
                { id: '3', code: 'Rejected' },
                { id: '4', code: 'Accepted' }
            ];

            (<any>enumService).setEnum('OfferStatus', enumItems);
            enumSorterFilter = new EnumSorterFilter(enumService, appConfigMock);
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