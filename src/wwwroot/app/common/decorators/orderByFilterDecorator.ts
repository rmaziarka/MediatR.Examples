/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Decorators {
    export class OrderByFilterDecorator {
        public static decoratorFunction($delegate: Function, $parse: ng.IParseService){
            return (list: any[], predicates: any, reverseOrder?: boolean, invertEmpties?: boolean) =>{
                if (angular.isDefined(invertEmpties)) {
                    if (!angular.isArray(predicates)) {
                        predicates = [predicates];
                    }
                    var newPredicates: any[] = [];
                    angular.forEach(predicates, (predicate: any) =>{
                        if (angular.isString(predicate)) {
                            var trimmed: any = predicate;
                            if (trimmed.charAt(0) === '-') {
                                trimmed = trimmed.slice(1);
                            }
                            var keyFn: any = $parse(trimmed);
                            newPredicates.push(item =>{
                                var value: any = keyFn(item);
                                return (angular.isDefined(value) && value != null) === invertEmpties;
                            });
                        }
                        newPredicates.push(predicate);
                    });
                    predicates = newPredicates;
                }
                return $delegate(list, predicates, reverseOrder);
            }
        }
    }
}