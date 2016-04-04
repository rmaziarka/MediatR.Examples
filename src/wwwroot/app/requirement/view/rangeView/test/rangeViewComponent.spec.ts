/// <reference path="../../../../typings/_all.d.ts" />
/// <reference path="../rangeviewcontroller.ts" />

module Antares {
    import RangeViewController = Requirement.View.RangeViewController;
    describe('Given range component is rendered', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery;
        
        var controller: RangeViewController;

        it('when min value is provided the component is visible', inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) =>{

            scope = $rootScope.$new();
            element = $compile('<range-view min="5"></range-view>')(scope);
            scope.$apply();

            controller = element.controller('rangeView');

            expect(controller.isVisible).toBeTruthy();
        }));

        it('when max value is provided the component is visible', inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            element = $compile('<range-view max="5"></range-view>')(scope);
            scope.$apply();

            controller = element.controller('rangeView');

            expect(controller.isVisible).toBeTruthy();
        }));

        it('when input parameters are provided they are properly displayed', inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) =>{

            var min = 1;
            var max = 2;
            var suffix = 'suffix';
            var label = 'label';

            scope = $rootScope.$new();
            element = $compile('<range-view min="' + min + '" max="' + max + '" label="' + label + '" suffix="\'' + suffix +'\'"></range-view>')(scope);
            scope.$apply();

            controller = element.controller('rangeView');

            var labelElement = element.find('div div').first();
            var valueElement = element.find('div div').last();
            var expectedValueText = min + ' - ' + max + ' ' + suffix;

            expect(controller.isVisible).toBeTruthy();
            expect(labelElement.text()).toBe(label);
            expect(valueElement.text()).toBe(expectedValueText);
        }));
    });
}