/// <reference path="../../../typings/_all.d.ts" />
module Antares {

    import LocalizationLoaderFactory = Antares.Common.Factories.LocalizationLoaderFactory;

    describe('Given translation', () => {

        var
            options: any,            
            httpBackend: ng.IHttpBackendService,
            createFactory: () => LocalizationLoaderFactory;

        describe('when page is loaded', () => {

            options = { key: 'key' };

            var staticTranslationMock = { STATIC: { S : 'SS' }};
            var enumsTranslationMock = { VALA: { A : 'AA' } };
            var resourcesTranslationMock = { VALB: { B : 'BB' } };

            beforeEach(inject((                
                $httpBackend: ng.IHttpBackendService,
                $q: ng.IQService,                
                dataAccessService: Antares.Services.DataAccessService) => {
                
                httpBackend = $httpBackend;

                httpBackend.whenGET(/translations\/.*\.json/).respond(() => {                    
                    return [200, staticTranslationMock];
                });

                httpBackend.whenGET(/\/api\/translations\/enums\/.*/).respond(() => {                    
                    return [200, enumsTranslationMock];
                });

                httpBackend.whenGET(/\/api\/translations\/resources\/.*/).respond(() => {
                    return [200, resourcesTranslationMock];
                });
                
                createFactory = () => {
                    return new LocalizationLoaderFactory($q, dataAccessService);
                };

            }));

            it('then should load translation static and dynamic for enums and resources', () => {
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
                expect(translations.DYNAMICTRANSLATIONS).toBeDefined();
                expect(translations.DYNAMICTRANSLATIONS).not.toBeNull();
                expect(translations.DYNAMICTRANSLATIONS.VALA).toBeDefined();
                expect(translations.DYNAMICTRANSLATIONS.VALA).not.toBeNull();                
                expect(translations.DYNAMICTRANSLATIONS.VALB).toBeDefined();
                expect(translations.DYNAMICTRANSLATIONS.VALB).not.toBeNull();                
                expect(error).toBeDefined();
                expect(error).toBeNull();
            });
        });

    });

}