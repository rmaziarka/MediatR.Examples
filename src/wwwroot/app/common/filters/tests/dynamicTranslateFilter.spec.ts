/// <reference path="../../../typings/_all.d.ts" />
module Antares {

    import DynamicTranslateFilter = Antares.Common.Filters.DynamicTranslateFilter;

    describe('Given enum translation', () => {

        var
            translate: ng.translate.ITranslateService,
            createFilter: (translate: ng.translate.ITranslateService) => DynamicTranslateFilter;

        describe('when using', () => {

            beforeEach(inject((
                $translate: ng.translate.ITranslateService) => {

                translate = $translate;

                createFilter = (translate: ng.translate.ITranslateService) => {
                    var dynamicTranslateFilter = new DynamicTranslateFilter(translate);
                    return dynamicTranslateFilter;
                };

            }));

            it('then should translate', () => {
                // arrange
                var toTranslate = "toTranslate";
                spyOn(translate, 'instant').and.callThrough();
                var dynamicTranslateFilter = createFilter(translate);
                
                // act                
                dynamicTranslateFilter.translate(toTranslate);
                               
                // assert
                expect(translate.instant).toHaveBeenCalledWith("DYNAMICTRANSLATIONS." + toTranslate);
            });
        });

    });

}