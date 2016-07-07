/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Decorators {
    export class FormDecorator {
        public static decoratorFunction($delegate: ng.IDirective[]) {
            var directive: ng.IDirective = $delegate[0];
            var compile: ng.IDirectiveCompileFn = directive.compile;

            directive.compile = function(tElement: any, tAttrs: any) {
                var link = compile.apply(this, arguments);

                return {
                    pre: function(scope: ng.IScope, el: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrls: IKfFormController[]) {
                        link.pre.apply(this, arguments);

                        var formController: IKfFormController = ctrls[0];

                        // TODO: is one nested level enough for forms? should it be recursive?
                        formController.setNestedSubmitted = (): void => {
                            formController.$setSubmitted();

                            angular.forEach(formController, function (item) {
                                if (item && item.$$parentForm === formController && item.$setSubmitted) {
                                    item.$setSubmitted();
                                }
                            });
                        }

                        formController.isFormValid = (): boolean => {
                            formController.setNestedSubmitted();

                            return formController.$valid;
                        }
                    }
                }
            }

            return $delegate;
        }
    }

    export interface IKfFormController extends ng.IFormController {
        setNestedSubmitted(): void;
        isFormValid(): boolean;
    }
}


