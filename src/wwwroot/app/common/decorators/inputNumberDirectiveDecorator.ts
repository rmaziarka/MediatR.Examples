/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Decorators {
    export class InputNumberDirectiveDecorator {
        public static decoratorFunction($delegate: any) {
            var directive: ng.IDirective = $delegate[0];
            var link: ng.IDirectivePrePost = directive.link;
            var baseLinkPre = link.pre;

            directive.compile = () => {

                return function (scope: ng.IScope, el: ng.IAugmentedJQuery, attrs: ng.IAttributes) {
                    if (attrs['type'] === "number") {
                        el.attr('ddp-decimal', "");
                    }

                    baseLinkPre.apply(this, arguments);
                };
            };

            return $delegate;
        }
    }
}