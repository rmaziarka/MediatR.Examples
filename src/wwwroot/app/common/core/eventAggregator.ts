/// <reference path="../../typings/_all.d.ts" />
module Antares.Core {
    export interface IEventHandler<T extends Event> { (event: T): void }

    export class EventAggregator {

        constructor(private $rootScope: ng.IRootScopeService) { }

        public publish<T extends Event>(event: T) {
            this.$rootScope.$broadcast(event.getKey(), event);
        }
        
        public with(controller: any) {
            return {
                subscribe: <T extends Event>(type: { new (): T; }, callback: IEventHandler<T>) => {
                    var listener = this.subscribe(type, callback);

                    var $destroy = controller.$destroy;
                    controller.$destroy = () => {
                        listener();
                        if ($destroy)
                            $destroy();
                    }
                }
            };
        }
        
        private subscribe<T extends Event>(type: { new (): T; }, callback: IEventHandler<T>) {
            var obj = new type();
            var listener = this.$rootScope.$on(obj.getKey(), (angularEvent: ng.IAngularEvent, event: T) => {
                callback(event);
            });

            return listener;
        }
    }

    angular.module('app').service('eventAggregator', EventAggregator);
}