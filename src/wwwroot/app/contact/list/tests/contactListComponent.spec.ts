///<reference path="../../../../typings/main.d.ts"/>

module Antares {
    import Contact = Antares.Common.Models.Dto.Contact;

    describe('Given contacts are displayed', () => {

        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http;

        beforeEach(angular.mock.module('app'));

        beforeEach(inject((_$httpBackend_) => {
            $http = _$httpBackend_;
            $http.whenGET(/\/api\/contact/).respond(() => {
                return [200, [
                    new Contact('C1'),
                    new Contact('C2')
                ]];
            })
        }));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            element = $compile('<contact-list></contact-list>')(scope);
            $http.flush();
            scope.$apply();
        }));


        it('Test if is running', () => {
            console.log(element.controller);
            expect(true).toBe(true);
            debugger;
        });

    });

}