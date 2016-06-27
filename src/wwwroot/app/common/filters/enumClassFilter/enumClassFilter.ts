/// <reference path="../../../typings/_all.d.ts" />

module Antares {

    export module Common {

        export module Filters {

	        export class EnumClassFilter {
		        constructor(private enumProvider: Antares.Providers.EnumProvider){
		        }

		        getEnumValue = (enumItemId: string) =>{
			        return this.enumProvider.getEnumCodeById(enumItemId);
		        }
				
		        camelToDash = (str: string) =>{
					return str.replace(/\W+/g, '-')
								.replace(/([a-z\d])([A-Z])/g, '$1-$2')
								.toLowerCase();
				}

                static getInstance(enumProvider: Antares.Providers.EnumProvider) {
                    var mappingFunc: any = (id: string) =>{
	                    var filter = new EnumClassFilter(enumProvider);
	                    var code = filter.getEnumValue(id);
	                    if (code !== undefined) {
		                    return filter.camelToDash(code);
	                    }
	                    else {
		                    return "";
	                    }

                    };
                    mappingFunc.$stateful = true;

                    return mappingFunc;
                };
            }

            angular.module('app').filter('enumClass', EnumClassFilter.getInstance);
        }
    }
}