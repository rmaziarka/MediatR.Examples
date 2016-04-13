///<reference path="../../../typings/_all.d.ts"/>

module Antares {
    import ContactListController = Component.ContactListController;
    import Dto = Common.Models.Dto;

    describe('Given contacts are displayed', () =>{

        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http:ng.IHttpBackendService;

        var contacts: Dto.IContact[] = [
            { id : '1', firstName : 'Test1', surname : 'Test1_S', title : 'Mr' },
            { id : '2', firstName : 'Test2', surname : 'Test2_S', title : 'Mr' }
        ];

        var controller: ContactListController;

        beforeEach(inject((_$httpBackend_:ng.IHttpBackendService) =>{
            $http = _$httpBackend_;
            $http.whenGET(/\/api\/contact/).respond(() =>{
                return [200, contacts];
            });
        }));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) =>{

            scope = $rootScope.$new();
            element = $compile('<contact-list></contact-list>')(scope);

            scope.$apply();

            controller = element.controller('contactList');
            controller.loadContacts();

            $http.flush();
            scope.$apply();
        }));

        it('contacts should be listed', () =>{
            var checkboxes = element.find('input:checkbox');
            expect(checkboxes.length).toBe(contacts.length);
        });

        it('when list element is clicked once then the list of selected elements should contain this element', () =>{
            var firstContact = element.find('input:checkbox:first');
            firstContact.click();

            var selectedElements = controller.getSelected();

            expect(firstContact.length).toBe(1);
            expect(firstContact.is(':checked')).toBe(true);
            expect(selectedElements.length).toBe(1);
            expect(selectedElements[0].id).toEqual(contacts[0].id);
        });

        it('when list element is clicked twice then the list of selected elements should not contain this element', () =>{
            var firstContact = element.find('input:checkbox:first');
            firstContact.click();
            firstContact.click();

            var selectedElements = controller.getSelected();

            expect(firstContact.length).toBe(1);
            expect(firstContact.is(':checked')).toBe(false);
            expect(selectedElements.length).toBe(0);
        });
    });

}