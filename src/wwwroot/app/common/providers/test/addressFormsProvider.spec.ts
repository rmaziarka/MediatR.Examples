/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('addressFormsProvider', () => {
        var
            service: Services.AddressFormsService,
            provider: Providers.AddressFormsProvider,
            q: ng.IQService,
            rootScope: ng.IRootScopeService;

        beforeEach(inject((
            addressFormsService: Services.AddressFormsService,
            addressFormsProvider: Providers.AddressFormsProvider,
            $q: ng.IQService,
            $rootScope: ng.IRootScopeService) => {

            service = addressFormsService;
            provider = addressFormsProvider;
            q = $q;
            rootScope = $rootScope;
        }));

        describe('when loadDefinitions is called', () => {
            var returnedValue: any = { foo: 'bar' };

            beforeEach(() => {
                spyOn(service, 'getAllDefinitons').and.callFake(() => {
                    var deffered = q.defer();
                    deffered.resolve(returnedValue);
                    return deffered.promise;
                });
                
                provider.loadDefinitions();
                rootScope.$digest();
            });

            it('then addressFormsService getAllDefinitions method should be called twice', () => {
                expect(service.getAllDefinitons).toHaveBeenCalledTimes(2);

                expect(service.getAllDefinitons).toHaveBeenCalledWith("Property");
                expect(service.getAllDefinitons).toHaveBeenCalledWith("Requirement");
            });

            it('then returned values from service should be assigned to provider properties', () =>{
                expect(provider.property).toBeDefined();
                expect(provider.requirement).toBeDefined();

                expect(provider.property).toBe(returnedValue);
                expect(provider.requirement).toBe(returnedValue);
            });
        });
    });
}