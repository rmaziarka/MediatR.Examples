/// <reference path="typings/_all.d.ts" />

module Antares {
    import IAppConfig = Common.Models.IAppConfig;
    declare var deferredBootstrapper: any;

    deferredBootstrapper.bootstrap({
        element: document.body,
        module: 'app',
        resolve: {
            appConfig: ($http: ng.IHttpService) => $http.get('app.json')
                .then((result: ng.IHttpPromiseCallbackArg<any>): IAppConfig => {
                    return {
                        rootUrl: result.data["App.Settings"].Api.RootUrl,
                        fileChunkSizeInBytes: result.data["App.Settings"].FileUpload.ChunkSizeInBytes
                    }
                })
        }
    });
}