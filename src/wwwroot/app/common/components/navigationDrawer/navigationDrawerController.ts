/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Component {
            import LatestViewsProvider = Antares.Providers.LatestViewsProvider;
            import LatestListEntry = Common.Models.Dto.ILatestListEntry;

            export class NavigationDrawerController {
                private type: string;

                private titleKey: string;
                private fieldName: string;

                private navigationDrawerData: { [type: string]: INavigationDrawerData; } = {
                    "property": {
                        titleKey: "COMMON.NAVIGATIONDRAWER.RECENTPROPERTIES",
                        fieldName: "properties"
                    },
                    "activity": {
                        titleKey: "COMMON.NAVIGATIONDRAWER.RECENTACTIVITIES",
                        fieldName: "activities"
                    },
                    "requirement": {
                        titleKey: "COMMON.NAVIGATIONDRAWER.RECENTREQUIREMENTS",
                        fieldName: "requirements"
                    }
                }

                constructor(private latestViewsProvider: LatestViewsProvider) {
                    var data = this.navigationDrawerData[this.type];

                    this.titleKey = data.titleKey;
                    this.fieldName = data.fieldName;
                }

                public getLatestViews = (): LatestListEntry[] => {
                    var views: LatestListEntry[] = this.latestViewsProvider[this.fieldName];
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