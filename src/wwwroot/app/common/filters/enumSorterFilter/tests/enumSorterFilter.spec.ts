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
            createFilter: (translate: ng.IFilterService) => EnumSorterFilter,
            mockDynamicTranslateFilter = (value: string) => {
                switch (value) {
                    case '1':
                        return "New";
                    case '2':
                        return "Accepted";
                    case '3':
                        return "Rejected";
                    case '4':
                        return "Withdrawn";
                }
            };

        describe('when enum sorting filter', () => {

            beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) => {
                $provide.value('dynamicTranslateFilter', mockDynamicTranslateFilter);
            }));

            beforeEach(inject((
                $filter: ng.IFilterService) => {

                filter = $filter;
                createFilter = ($filter: ng.IFilterService) => {
                    var enumSorterFilter = new EnumSorterFilter(filter, appConfigMock);
                    return enumSorterFilter;
                };
            }));

            it('offers should be sorted properly', () => {
                // arrange
                var enumSorterFilter = createFilter(filter);

                var offersMock = TestHelpers.OfferGenerator.generateMany(4);
                offersMock[0].statusId = '1';
                offersMock[1].statusId = '2';
                offersMock[2].statusId = '3';
                offersMock[3].statusId = '4';
                
                // act                
                var sortedOffers = enumSorterFilter.sort(offersMock, "OfferStatus", 'statusId');
                expect(sortedOffers[0].statusId).toEqual("2");
                expect(sortedOffers[1].statusId).toEqual("1");
                expect(sortedOffers[2].statusId).toEqual("3");
                expect(sortedOffers[3].statusId).toEqual("4");
            });
        });

    });

}