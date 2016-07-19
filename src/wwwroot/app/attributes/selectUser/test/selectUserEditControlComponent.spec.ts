/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import SelectUserEditControlController = Attributes.SelectUserEditControlController;
    
    describe('Given selectUser component', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: SelectUserEditControlController;

        var pageObjectSelector = {
            userItem: '#card-user [id^="user-"]',
            edit: '#edit-btn',
            search: "#user-search",
            removeBtn: "context-menu-item[type='remove'] .contextMenuItem"
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            // init
            scope = $rootScope.$new();
            scope["user"] = TestHelpers.UserGenerator.generate();
            scope["schema"] = { formName : "form1", placeholderTranslationKey : "hint" };
            scope["config"] = {};

            compile = $compile;
            element = compile('<select-user-edit-control user="user" config="config" schema="schema"></contact-negotiators-edit-control>')(scope);
            scope.$apply();
            controller = element.controller('selectUserEditControl');
        }));

        it('then user should be displayed', () => {
            var userData = element.find(pageObjectSelector.userItem);
            expect(userData.text()).toEqual(controller.user.getName());
        });

        it('when remove clicked then user should be empty', () =>{
            let removeButton = element.find(pageObjectSelector.removeBtn);
            removeButton.click();
            
            var userData = element.find(pageObjectSelector.userItem);
            expect(userData.text().trim()).toEqual("");
        });

        it('than search is invisible', () => {
            let searchInput = element.find(pageObjectSelector.search);
            
            expect(searchInput.hasClass('ng-hide')).toBeTruthy();
        });

        it('when edit clicked than search is visible', () => {
            let searchInput = element.find(pageObjectSelector.search);
            let editButton = element.find(pageObjectSelector.edit);

            // act
            editButton.click();
            
            expect(searchInput.hasClass('ng-hide')).toBeFalsy();
        });
    });
}