/// <reference path="../../../typings/_all.d.ts" />
module Antares {

    import EnumTranslateFilter = Antares.Common.Filters.EnumTranslateFilter;

    describe('Given enum translation', () => {

        var
            translate: ng.translate.ITranslateService,
            createFilter: (translate: ng.translate.ITranslateService) => EnumTranslateFilter;

        describe('when using', () => {

            beforeEach(inject((
                $translate: ng.translate.ITranslateService) => {

                translate = $translate;

                createFilter = (translate: ng.translate.ITranslateService) => {
                    var enumTranslateFilter = new EnumTranslateFilter(translate);
                    return enumTranslateFilter;
                };

            }));

            it('then should translate', () => {
                // arrange
                var toTranslate = "toTranslate";
                spyOn(translate, 'instant').and.callThrough();
                var enumTranslateFilter = createFilter(translate);
                
                // act                
                enumTranslateFilter.translate(toTranslate);
                               
                // assert
                expect(translate.instant).toHaveBeenCalledWith("ENUMS." + toTranslate);
            });
        });

    });

}