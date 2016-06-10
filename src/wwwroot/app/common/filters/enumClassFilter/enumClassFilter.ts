/// <reference path="../../../typings/_all.d.ts" />

module Antares {

    export module Common {

        export module Filters {

	        export class EnumClassFilter {
		        constructor(private enumService: Antares.Services.EnumService){
		        }

		        getEnumValue = (enumItemId: string) =>{
			        return this.enumService.getEnumCodeById(enumItemId);
		        }
				
		        camelToDash = (str: string) =>{
					return str.replace(/\W+/g, '-')
								.replace(/([a-z\d])([A-Z])/g, '$1-$2')
								.toLowerCase();
				}

                static getInstance(enumService: Antares.Services.EnumService) {
                    var mappingFunc: any = (id: string) =>{
	                    var filter = new EnumClassFilter(enumService);
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