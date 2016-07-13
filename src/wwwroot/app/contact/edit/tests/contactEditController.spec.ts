/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import ContactEditController = Contact.ContactEditController;
    import CurrentUserGenerator = Antares.TestHelpers.CurrentUserGenerator;

    describe('Given contactEdit controller', () => {
        var
            state: ng.ui.IStateService,
            $http: ng.IHttpBackendService,
            controller: ContactEditController;
        
        var contactMock = TestHelpers.ContactGenerator.generate();
        
        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $httpBackend: ng.IHttpBackendService,
            $state: ng.ui.IStateService,
            $controller: ng.IControllerService) => {

            state = $state;
            $http = $httpBackend;
            
            controller = getMockedController($rootScope, $controller);

            spyOn($state, 'go').and.callFake((state: any, params: any) => {
                return {
                     then() {}
                }
            });

            $http.whenGET(/\/api\/contacts\/titles/).respond(() => {
                return [200, {}];
            });
        }));
        
        it('cancel redirects to view page', () => {
            controller.cancelEdit();

            expect(state.go).toHaveBeenCalledWith('app.contact-view', contactMock);
        });

        it('save redirects to view page', () => {
            $http.whenPUT(/\/api\/contacts/).respond(() => {
                return [200, {}];
            });
            
            controller.save();
            $http.flush();

            expect(state.go).toHaveBeenCalledWith('app.contact-view', contactMock);
        });

        it('save sends whole object to service', () => {
            var contactSentToService: Business.Contact;

            $http.whenPUT(/\/api\/contacts/).respond((method: string, url: string, data: string) => {
                contactSentToService = new Business.Contact(JSON.parse(data));
                return [200, {}];
            });

            controller.save();
            $http.flush();
            
            expect(contactSentToService).toEqual(contactMock);
        });
        
        function getMockedController($rootScope: ng.IRootScopeService, $controller: ng.IControllerService) {
            let scope = $rootScope.$new();

            let controller = $controller(ContactEditController, {
                $scope: scope
            });
            controller.contact = contactMock;
            controller.userData = CurrentUserGenerator.generateDto();
            controller.editContactForm = {
                $setPristine: () => { }
            };
            controller.$onInit();

            return controller;
        }
    });
}