/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Component {
            import LatestViewsProvider = Antares.Providers.LatestViewsProvider;
            import ILatestViewData = Common.Models.Dto.ILatestViewData;

            export class NavigationDrawerController {
                private titleKey: string;
                private drawerFieldName: string;

                constructor(private latestViewsProvider: LatestViewsProvider){
                    
                }

                public getLatestViews = (): ILatestViewData[] =>{
                    var views: ILatestViewData[] = this.latestViewsProvider[this.drawerFieldName];
                    return views;
                }
            }

            angular.module('app').controller('navigationDrawerController', NavigationDrawerController);
        }
    }
}