/// <reference path="../typings/_all.d.ts" />

module Antares.Activity {
    export class ActivityConfigUtils {
        public merge(...configs: IActivityConfig[]) {
            var resultConfig: IActivityConfig = <IActivityConfig>({});

            configs.forEach((config: IActivityConfig) =>{
                Object.keys(config)
                    .forEach((key: string) => {
                        if (resultConfig[key]) {
                            return;
                        }

                        resultConfig[key] = config[key];
                    });
            });
            
            return resultConfig;
        }
    }

    angular.module('app').service('activityConfigUtils', ActivityConfigUtils);
}