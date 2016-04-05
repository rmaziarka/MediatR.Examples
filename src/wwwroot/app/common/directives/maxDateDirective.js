/// <reference path="../../typings/_all.d.ts" />
var Antares;
(function (Antares) {
    var Common;
    (function (Common) {
        var Directive;
        (function (Directive) {
            var KfMaxDateDirective = (function () {
                function KfMaxDateDirective() {
                    this.restrict = 'A';
                }
                KfMaxDateDirective.prototype.link = function (scope, element, attrs) {
                    var ngModel = element.controller('ngModel'), maxDate = attrs['kfMaxDate'];
                    if (maxDate === '') {
                        return;
                    }
                    if (isNaN(Date.parse(maxDate))) {
                        throw new Error("kf-max-date: Max date " + maxDate + " provided by user is invalid");
                    }
                    var maxAllowedDate = new Date(maxDate);
                    var setValidation = function (isValid) {
                        ngModel.$setValidity('kfMaxDate', isValid);
                        return isValid;
                    };
                    ngModel.$validators['kfMaxDate'] = function (modelValue) {
                        if (modelValue === null || modelValue === '') {
                            return setValidation(true);
                        }
                        var modelDate = new Date(modelValue);
                        var isValid = modelDate <= maxAllowedDate;
                        return setValidation(isValid);
                    };
                };
                ;
                KfMaxDateDirective.factory = function () {
                    var directive = function () {
                        return new KfMaxDateDirective();
                    };
                    directive['$inject'] = [];
                    return directive;
                };
                KfMaxDateDirective.InvalidDateErrorMessage = "kf-max-date: Max date ${maxDate} provided by user is invalid";
                return KfMaxDateDirective;
            }());
            Directive.KfMaxDateDirective = KfMaxDateDirective;
            angular.module('app').directive('kfMaxDate', KfMaxDateDirective.factory());
        })(Directive = Common.Directive || (Common.Directive = {}));
    })(Common = Antares.Common || (Antares.Common = {}));
})(Antares || (Antares = {}));

//# sourceMappingURL=maxDateDirective.js.map
