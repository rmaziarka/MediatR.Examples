/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class KfFileModelDirective implements ng.IDirective {
        restrict = "A";
        require = 'ngModel';

        constructor(private $parse: any) {
        }

        link = (scope: any, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) => {

            element.on('change', () =>{
                var el: any = element[0];
                this.$parse(attrs['kfFileModel'])
                    .assign(scope, el.files[0]);
                scope.$apply();
            });

            scope.$watch(attrs['kfFileModel'], (newValue: any, oldValue: any) => {
                if (newValue === null && oldValue === undefined) {
                    return;
                }

                var valueChanged = newValue !== oldValue;

                if (valueChanged && (newValue === undefined)) {
                    var el: any = element[0];
                    el.value = '';
                    el.files = null;
                }

                if (valueChanged && (newValue !== undefined)) {
                    ngModel.$setDirty();
                }
            });
        };

        static factory(): ng.IDirectiveFactory {
            var directive = ($parse: ng.IParseService): ng.IDirective => {
                return new KfFileModelDirective($parse);
            };

            directive.$inject = ['$parse'];
            return <ng.IDirectiveFactory>directive;
        }
    }

    angular.module('app').directive('kfFileModel', <ng.IDirectiveFactory>KfFileModelDirective.factory());
}