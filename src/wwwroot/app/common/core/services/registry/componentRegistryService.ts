///<reference path="../../../../../typings/main.d.ts"/>

module Antares {
    export module Core {
        export module Service {

            /**
             * Component registry service.
             */
            export class ComponentRegistry {
                static $inject: Array<string> = [];

                constructor(private instances: { [id: string]: any }) {

                }

                deregister = (componentId: string) => {
                    delete this.instances[componentId];
                }

                register = (instance, componentId) => {
                    this.instances[componentId] = instance;
                    return () => {
                        this.deregister(componentId);
                    }
                }

                get = (componentId) => {
                    return this.instances[componentId];
                }
            }

            function componentRegistryFactory() {
                var instances: { [id: string]: any } = {};
                return new ComponentRegistry(instances);
            }

            angular.module('app').factory('componentRegistry', componentRegistryFactory);
        }
    }
}