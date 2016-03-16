/// <reference path="../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Directive {
            export class ListNotEmptyDirective implements ng.IDirective {
                restrict = 'E';
                require = 'ngModel';
                scope = {
                    ngModel: '=ngModel'
                };

                link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController){

                    ngModel.$validators['listNotEmpty'] = (modelValue) => (modelValue !== undefined && modelValue.length > 0);

                    scope.$watch('ngModel', (newValue, oldValue) => {
                        if ((newValue !== undefined || oldValue !== undefined) && newValue !== oldValue) {
                            ngModel.$setDirty();
                        }
                    });
                };

                static factory() {
                    var directive = () => {
                        return new ListNotEmptyDirective();
                    };

                    directive['$inject'] = [];
                    return directive;
                }
            }

            angular.module('app').directive('listNotEmpty', ListNotEmptyDirective.factory());
        }
    }
}