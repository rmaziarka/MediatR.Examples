/// <reference path="../../../typings/_all.d.ts" />
module Antares {

    import LocalizationLoaderFactory = Antares.Factories.LocalizationLoaderFactory;

    describe('Given translation', () => {

        var
            options: any,            
            httpBackend: ng.IHttpBackendService,
            createFactory: () => LocalizationLoaderFactory;

        describe('when page is loaded', () => {

            options = { key: 'key' };

            var staticTranslationMock = { STATIC: {}};
            var enumsTranslationMock = { VAL: {}};

            beforeEach(angular.mock.module('app'));

            beforeEach(inject((                
                $httpBackend: ng.IHttpBackendService,
                $q: ng.IQService,                
                dataAccessService: Antares.Services.DataAccessService) => {
                
                httpBackend = $httpBackend;

                httpBackend.whenGET(/\/translations\/.*\.json/).respond(() => {                    
                    return [200, staticTranslationMock];
                });

                httpBackend.whenGET(/\/api\/enums\/translations\/.*/).respond(() => {                    
                    return [200, enumsTranslationMock];
                });
                
                createFactory = () => {
                    return new LocalizationLoaderFactory($q, dataAccessService);
                };

            }));

            it('then should load translation static and dynamic for enums', () => {
                // arrange
                var
                    translations: any,
                    error: any,
                    localizationLoaderFactory = createFactory();

                // act                
                localizationLoaderFactory.getTranslation(options).then(function (data) {
                    translations = data;
                    error = null;
                }, function (errorData) {
                    translations = null;
                    error = errorData;
                });

                httpBackend.flush();
               
                // assert
                expect(translations).toBeDefined();
                expect(translations).not.toBeNull();
                expect(translations.STATIC).not.toBeNull();
                expect(translations.ENUMS).not.toBeNull();
                expect(translations.ENUMS.VAL).not.toBeNull();
                expect(error).toBeDefined();
                expect(error).toBeNull();
            });
        });

    });

}