/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import LatestViewsService = Services.LatestViewsService;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import LatestViewCommand = Common.Models.Commands.ICreateLatestViewCommand;
    import EntityTypeEnum = Common.Models.Enums.EntityTypeEnum;
    import LatestViewGenerator = TestHelpers.LatestViewGenerator;
    import LatestViewResultItem = Common.Models.Dto.ILatestViewResultItem;
    import Address = Common.Models.Business.Address;
    import Contact = Common.Models.Business.Contact;
    import Company = Common.Models.Business.Company;

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
                spyOn(provider, 'loadRequirements');
                spyOn(provider, 'loadCompanies');

                // act 
                provider.loadLatestData(result);
            });

            it('then loadProperties method should be called with http result data', () =>{
                expect(provider.loadProperties).toHaveBeenCalledWith(result.data);
            });

            it('then loadActivities method should be called with http result data', () =>{
                expect(provider.loadActivities).toHaveBeenCalledWith(result.data);
            });

            it('then loadRequirements method should be called with http result data', () => {
                expect(provider.loadRequirements).toHaveBeenCalledWith(result.data);
            });

            it('then loadCompanies method should be called with http result data', () => {
                expect(provider.loadCompanies).toHaveBeenCalledWith(result.data);
            });
        });

        describe('when loadRequirements is called with latest views', () => {

            beforeEach(inject(($state: angular.ui.IState) => {
                // arrange 
                spyOn($state, 'href')
                    .and.callFake((state: string, obj: any) => {
                        return state + obj.id;
                    });
            }));

            it('when there are no requirement views then requirement field is undefined', () => {
                //arrange
                var views: LatestViewResultItem[] = [];

                //act
                provider.loadRequirements(views);

                //assert
                expect(provider.requirements).toBeUndefined();
            });

            it('when there are three requirement views then requirements field should contain three entries', () => {
                //arrange
                var requirementViews = LatestViewGenerator.generateRequirementList(3);
                var views = [
                    requirementViews
                ];

                //act
                provider.loadRequirements(views);

                //assert
                expect(provider.requirements.length).toBe(3);
            });

            it('entry should contain all contact names', () => {
                //arrange
                var requirementViews = LatestViewGenerator.generateRequirementList(1);
                var views = [
                    requirementViews
                ];

                //act
                provider.loadRequirements(views);

                //assert
                var requirementView = requirementViews.list[0];
                var expectedId = requirementView.id;
                var expectedContactNames = requirementView.data.map((c: Contact) => new Contact(c).getName()).join(", ");
                var expectedUrl = "app.requirement-view" + expectedId;

                var listRequirement = provider.requirements[0];
                expect(listRequirement.id).toBe(expectedId);
                expect(listRequirement.name).toBe(expectedContactNames);
                expect(listRequirement.url).toBe(expectedUrl);
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

            it('if there are three property views then properties field should contain three entries', () => {
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

            it('if property address is fully filled then entry should contain all data', () => {
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

            it('if property address is empty then entry should have dash as title', () => {
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

        describe('when loadActivities is called with latest views', () => {

            beforeEach(inject(($state: angular.ui.IState) => {
                // arrange 
                spyOn($state, 'href')
                    .and.callFake((state: string, obj: any) => {
                        return state + obj.id;
                    });
            }));

            it('if there is no activity views then activities field is undefined', () => {
                //arrange
                var views: LatestViewResultItem[] = [];

                //act
                provider.loadActivities(views);

                //assert
                expect(provider.activities).toBeUndefined();
            });

            it('if there are three activity views then activities field should contains three entries', () => {
                //arrange
                var activityViews = LatestViewGenerator.generateActivityList(3);
                var views = [
                    activityViews
                ];

                //act
                provider.loadActivities(views);

                //assert
                expect(provider.activities.length).toBe(3);
            });

            it('if activity address is fulfiled then entry should contain all data', () => {
                //arrange
                var activityViews = LatestViewGenerator.generateActivityList(1);
                var views = [
                    activityViews
                ];

                //act
                provider.loadActivities(views);

                //assert
                var activityView = activityViews.list[0];
                var expectedId = activityView.id;
                var expectedAddressLine = new Address(activityView.data).getAddressText();
                var expectedUrl = "app.activity-view" + expectedId;

                var listActivity = provider.activities[0];
                expect(listActivity.id).toBe(expectedId);
                expect(listActivity.name).toBe(expectedAddressLine);
                expect(listActivity.url).toBe(expectedUrl);
            });

            it('if activity address is empty then entry should have dash as title', () => {
                //arrange
                var activityViews = LatestViewGenerator.generateActivityList(1);
                var activityView = activityViews.list[0];
                activityView.data = new Address({});
                var views = [
                    activityViews
                ];

                //act
                provider.loadActivities(views);

                //assert
                var listActivity = provider.activities[0];
                expect(listActivity.name).toBe('-');
            });
        });

        describe('when loadCompanies is called with latest views', () => {

            beforeEach(inject(($state: angular.ui.IState) => {
                // arrange 
                spyOn($state, 'href')
                    .and.callFake((state: string, obj: any) => {
                        return state + obj.id;
                    });
            }));

            it('if there is no company views then companies field is undefined', () => {
                //arrange
                var views: LatestViewResultItem[] = [];

                //act
                provider.loadCompanies(views);

                //assert
                expect(provider.companies).toBeUndefined();
            });

            it('if there are three company views then companies field should contains three entries', () => {
                //arrange
                var companyViews = LatestViewGenerator.generateCompanyList(3);
                var views = [
                    companyViews
                ];

                //act
                provider.loadCompanies(views);

                //assert
                expect(provider.companies.length).toBe(3);
            });

            it('if activity company is fulfiled then entry should contain all data', () => {
                //arrange
                var companyViews = LatestViewGenerator.generateCompanyList(1);
                var views = [
                    companyViews
                ];

                //act
                provider.loadCompanies(views);

                //assert
                var companyView = companyViews.list[0];
                var expectedId = companyView.id;
                var expectedName = new Company(companyView.data).name;
                var expectedUrl = "app.company-view" + expectedId;

                var listCompany = provider.companies[0];
                expect(listCompany.id).toBe(expectedId);
                expect(listCompany.name).toBe(expectedName);
                expect(listCompany.url).toBe(expectedUrl);
            });
        });
    });
}