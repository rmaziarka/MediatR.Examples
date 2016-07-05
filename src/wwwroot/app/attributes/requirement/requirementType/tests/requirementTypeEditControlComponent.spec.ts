/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import RequirementTypeEditControlController = Attributes.RequirementTypeEditControlController;

    describe('Given requirement type edit component', () =>{
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: RequirementTypeEditControlController,
            assertValidator: TestHelpers.AssertValidators;
        
        var typeChangedMock = () =>{
            var i = 1;
        };
        var pageObjectSelector = {
            type: '#type'
        };

        var mockedComponentHtml = '<requirement-type-edit-control ng-model="vm.ngModel" config="vm.config" on-requirement-type-changed="vm.typeChanged()"></requirement-type-edit-control>';

        var requirementTypes = [
            { id: "type1", code: "type1" },
            { id: "type2", code: "type2" }
        ];
        
        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            requirementService: Requirement.RequirementService,
            $q: ng.IQService) => {
            
            // init
            scope = $rootScope.$new();
            requirementService.getRequirementTypes = () =>{
                var deffered = $q.defer();
                var serviceResult = {
                    data: requirementTypes
                }
                deffered.resolve(serviceResult);
                return deffered.promise;
            };

            scope['vm'] = { ngModel: 'type1', typeChanged: typeChangedMock };
            spyOn(scope['vm'], 'typeChanged').and.callThrough();

            compile = $compile;
            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
            controller = element.controller('requirementTypeEditControl');
            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        it('when requirement type is required then validation message should be displayed', () => {
            assertValidator.assertRequiredValidator(null, false, pageObjectSelector.type);
        });

        describe('when select value is change', () => {
            it('then on change method is called', () =>{
                element.find(pageObjectSelector.type).val("type2");
                element.find(pageObjectSelector.type).triggerHandler('change');
                expect(scope['vm']['typeChanged']).toHaveBeenCalled();
            });
        });
    });
}