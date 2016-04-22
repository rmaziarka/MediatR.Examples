/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
	import Business = Common.Models.Business;
	
	interface IScope extends ng.IScope {
		characteristicSelect: Business.CharacteristicSelect;
		characteristic: Business.Characteristic; 
	}
	
	describe('Given characteristic component', () =>{

		var pageObjectSelector = {
			characteristicText : 'input[data-type="characteristicText"]',
			characteristicCloud : 'button[data-type"characteristicCloudIcon"]'
	    }
		var assertValidator: TestHelpers.AssertValidators;
		var scope: IScope,
			compile: ng.ICompileService,
			element: ng.IAugmentedJQuery;

		var mockedComponentHtml = '<characteristic-select characteristic-select="characteristicSelect" characteristic="characteristic">'            
			+ '</characteristic-select>';

		beforeEach(inject((
			$rootScope: ng.IRootScopeService,
			$compile: ng.ICompileService) => {

			// init
			scope = <IScope>$rootScope.$new();
			scope.characteristicSelect = new Business.CharacteristicSelect();
			scope.characteristic = new Business.Characteristic(); 
			compile = $compile;

			element = compile(mockedComponentHtml)(scope);
			scope.$apply();

            assertValidator = new TestHelpers.AssertValidators(element, scope);
		}));
        
		it('when text exceeds 50 characters length then error should appear', () =>{
			var maxLength = 50;
            
			scope.$apply();
			assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelector.characteristicText);
		});
    });
}