/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Component {
            import LatestViewsProvider = Antares.Providers.LatestViewsProvider;
            import ILatestViewData = Common.Models.Dto.ILatestViewData;

            export class NavigationDrawerController {
                private type: string;

                private titleKey: string;
                private fieldName: string;

                private navigationDrawerData: { [type: string]: INavigationDrawerData; } = {
                    "property": {
                        titleKey: "COMMON.NAVIGATIONDRAWER.RECENTPROPERTIES",
                        fieldName:"properties"
                    }
                }

                constructor(private latestViewsProvider: LatestViewsProvider){
                    var data = this.navigationDrawerData[this.type];

                    this.titleKey = data.titleKey;
                    this.fieldName = data.fieldName;
                }

                public getLatestViews = (): ILatestViewData[] =>{
                    var views: ILatestViewData[] = this.latestViewsProvider[this.fieldName];
                    return views;
                }
            }

            interface INavigationDrawerData {
                titleKey: string;
                fieldName: string;
            }

            angular.module('app').controller('navigationDrawerController', NavigationDrawerController);
        }
    }
}