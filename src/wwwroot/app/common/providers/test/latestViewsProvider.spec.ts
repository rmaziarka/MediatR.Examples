/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import LatestViewsService = Services.LatestViewsService;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import LatestViewCommand = Common.Models.Commands.ICreateLatestViewCommand;
    import EntityTypeEnum = Common.Models.Enums.EntityTypeEnum;
    import LatestViewGenerator = TestHelpers.LatestViewGenerator;
    import LatestViewResultItem = Common.Models.Dto.ILatestViewResultItem;
    import Address = Common.Models.Business.Address;

    describe('latestViewsProvider', () => {
        var
            service: LatestViewsService,
            provider: LatestViewsProvider,
            q: ng.IQService,
            rootScope: ng.IRootScopeService;

        beforeEach(inject((
            latestViewsService: LatestViewsService,
            latestViewsProvider: LatestViewsProvider,
            $q: ng.IQService,
            $rootScope: ng.IRootScopeService) => {

            service = latestViewsService;
            provider = latestViewsProvider;
            q = $q;
            rootScope = $rootScope;
        }));

        describe('when refresh is called', () => {
            var returnedValue: any = { test: 1 };

            beforeEach(() =>{
                // arrange
                spyOn(service, 'get').and.callFake(() =>{
                        var deffered = q.defer();
                        deffered.resolve(returnedValue);
                        return deffered.promise;
                    });

                spyOn(provider, 'loadLatestData');

                // act 
                provider.refresh();
                rootScope.$digest();
            });

            it('then latestViewsService get method should be called', () =>{
                expect(service.get).toHaveBeenCalled();
            });

            it('then loadLatestData method should be called with passed value from service', () => {
                expect(provider.loadLatestData).toHaveBeenCalledWith(returnedValue);
            });
        });

        describe('when addView is called with command', () =>{
            var command: LatestViewCommand;
            var returnedValue: any = { test: 1 };

            beforeEach(() =>{
                // arrange
                command = { entityId: 'guid', entityType: EntityTypeEnum.Property };

                spyOn(service, 'post').and.callFake(() =>{
                        var deffered = q.defer();
                        deffered.resolve(returnedValue);
                        return deffered.promise;
                    });

                spyOn(provider, 'loadLatestData');

                // act 
                provider.addView(command);
                rootScope.$digest();
            });

            it('then latestViewsService post method should be called with command', () =>{
                expect(service.post).toHaveBeenCalledWith(command);
            });

            it('then loadLatestData method should be called with passed value from service', () => {
                expect(provider.loadLatestData).toHaveBeenCalledWith(returnedValue);
            });
        });

        describe('when loadLatestData is called with http result', () => {
            var result: angular.IHttpPromiseCallbackArg<LatestViewResultItem[]> = {
                data: []
            };

            beforeEach(() => {
                spyOn(provider, 'loadProperties');
                spyOn(provider, 'loadActivities');

                // act 
                provider.loadLatestData(result);
            });

            it('then loadProperties method should be called with http result data', () =>{
                expect(provider.loadProperties).toHaveBeenCalledWith(result.data);
            });

            it('then loadActivities method should be called with http result data', () =>{
                expect(provider.loadActivities).toHaveBeenCalledWith(result.data);
            });
        });

        describe('when loadProperties is called with latest views', () => {

            beforeEach(inject(($state: angular.ui.IState) => {
                // arrange 
                spyOn($state, 'href')
                    .and.callFake((state: string, obj:any) =>{
                        return state + obj.id;
                    });
            }));
            
            it('if there is no property views then properties field is undefined', () => {
                //arrange
                var views: LatestViewResultItem[] = [];

                //act
                provider.loadProperties(views);

                //assert
                expect(provider.properties).toBeUndefined();
            });

            it('if there are three property views then properties field should contains three entries', () => {
                //arrange
                var propertyViews = LatestViewGenerator.generatePropertyList(3);
                var views = [
                    propertyViews
                ];

                //act
                provider.loadProperties(views);

                //assert
                expect(provider.properties.length).toBe(3);
            });

            it('if property address is fulfiled then entry should contain all data', () => {
                //arrange
                var propertyViews = LatestViewGenerator.generatePropertyList(1);
                var views = [
                    propertyViews
                ];

                //act
                provider.loadProperties(views);

                //assert
                var propertyView = propertyViews.list[0];
                var expectedId = propertyView.id;
                var expectedAddressLine = new Address(propertyView.data).getAddressText();
                var expectedUrl = "app.property-view" + expectedId;

                var listProperty = provider.properties[0];
                expect(listProperty.id).toBe(expectedId);
                expect(listProperty.name).toBe(expectedAddressLine);
                expect(listProperty.url).toBe(expectedUrl);
            });

            it('if property address is empty then entry should has dash as title', () => {
                //arrange
                var propertyViews = LatestViewGenerator.generatePropertyList(1);
                var propertyView = propertyViews.list[0];
                propertyView.data = new Address({});
                var views = [
                    propertyViews
                ];

                //act
                provider.loadProperties(views);

                //assert
                var listProperty = provider.properties[0];
                expect(listProperty.name).toBe('-');
            });
        });
    });
}