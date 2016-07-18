/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    interface IScope extends ng.IScope {
        characteristicSelect: Business.CharacteristicSelect;
        characteristic: Business.Characteristic;
    }
    
    describe('Given characteristic component', () =>{
        var pageObjectSelector = {
            characteristicCommentText : 'input[data-type="characteristicText"]',
            characteristicComment: 'button[data-type="characteristicCloudIcon"]',
            parentContainer: '.checkbox'
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
        
        it('when characteristic comment exceeds 50 characters length then error should appear', () =>{
            var maxCommentLength = 50;
            scope.characteristic.isDisplayText = true;
            scope.$apply();
            assertValidator.assertMaxLengthValidator(maxCommentLength + 1, false, pageObjectSelector.characteristicCommentText, pageObjectSelector.parentContainer);
        });
        
        it('when characteristic is selected then cloud icon is visible', () =>{
            var isCharacteristicCommentVisible = false;
            scope.characteristicSelect.isSelected = true;
            scope.characteristic.isDisplayText = true;
            scope.$apply();
            
            assertValidator.assertElementHasHideClass(isCharacteristicCommentVisible, pageObjectSelector.characteristicComment);
        });
        
        it('when characteristic is unselected then cloud icon invisible and comment is empty', () =>{
            // arrange 
            var isCharacteristicCommentInvisible = true;
            var isCharacteristicCommentTextInvisible = true;
            
            scope.characteristicSelect.isSelected = true;
            scope.characteristicSelect.text = 'test';
            scope.characteristic.isDisplayText = true;
            scope.$apply();
                           
            //act
            scope.characteristicSelect.isSelected=false;
            scope.$apply();
            
            // assert
            var commentValue = element.find(pageObjectSelector.characteristicComment).val();  
            
            assertValidator.assertElementHasHideClass(isCharacteristicCommentInvisible, pageObjectSelector.characteristicComment); 
            assertValidator.assertElementHasHideClass(isCharacteristicCommentTextInvisible, pageObjectSelector.characteristicCommentText);            
            expect(commentValue).toEqual('');
		});
        
        it('when characteristic forbids commenting then cloud icon is invisible', () =>{
            scope.characteristicSelect.isSelected = true;
            scope.characteristic.isDisplayText = false;
            scope.$apply();
            
            // assert 
            var characteristicComment = element.find(pageObjectSelector.characteristicComment);
            expect(characteristicComment.length).toBe(0);
        });
    });
}