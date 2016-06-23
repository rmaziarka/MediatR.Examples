/// <reference path="../../typings/_all.d.ts" />
module Antares.Core {
    export interface IMessageHandler<T extends BaseMessage> { (event: T): void }

    export class PubSub {
        prefix: string = "pubsub.";
        constructor(private $rootScope: ng.IRootScopeService) { }

        public publish = <T extends BaseMessage>(event: T) => {
            this.$rootScope.$broadcast(this.prefix + event.getKey(), event);
        }

        public with(controller: any) {
            return {
                subscribe: <T extends BaseMessage>(type: { new (): T; }, callback: IMessageHandler<T>) => {
                    var listener = this.subscribe(controller, type, callback);

                    var $destroy = controller.$destroy;
                    controller.$destroy = () => {
                        listener();
                        if ($destroy)
                            $destroy();
                    }
                }
            };
        }

        private subscribe<T extends BaseMessage>(controller: any, type: { new (): T; }, callback: IMessageHandler<T>) {
            var obj = new type();
            var listener = this.$rootScope.$on(this.prefix + obj.getKey(), (angularEvent: ng.IAngularEvent, message: T) => {
                callback.call(controller, message);
            });

            return listener;
        }
    }

    angular.module('app').service('pubSub', PubSub);
}