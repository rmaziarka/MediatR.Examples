/// <reference path="../../typings/_all.d.ts" />

module Antares.Factories {
    export class AzureBlobUploadFactory {
        private uploadResult: ng.IDeferred<any>;
        private reader:FileReader;

        private maxBlockSize: number;
        private totalBytesRemaining: number;
        private currentFilePointer: number;
        private chunkIds: string[];
        private chunkIdPrefix: string;
        private file: File;
        private fileUrl: string;

        constructor(private $http: ng.IHttpService, private appConfig: Common.Models.IAppConfig, private $q: ng.IQService) {
        }

        private init = (file: File, fileUrl: string) => {
            var blockSize = this.appConfig.fileChunkSizeInBytes;
            var maxBlockSize = blockSize;
            var fileSize = file.size;

            if (fileSize < blockSize) {
                maxBlockSize = fileSize;
            }

            this.maxBlockSize = maxBlockSize;
            this.totalBytesRemaining = fileSize;
            this.currentFilePointer = 0;
            this.chunkIds = [];
            this.chunkIdPrefix = _.random(1000000000).toString();
            this.file = file;
            this.fileUrl = fileUrl;

            this.uploadResult = this.$q.defer();
            this.reader = new FileReader();
        }

        uploadFile = (file: File, fileUrl: string) => {
            this.init(file, fileUrl);

            this.reader.onloadend = (event: any) => {
                if (event.target.readyState !== FileReader.prototype.DONE) {
                    return;
                }

                var uri = this.fileUrl + '&comp=block&blockid=' + this.chunkIds[this.chunkIds.length - 1];
                var requestData = new Uint8Array(event.target.result);

                this.$http.put(uri, requestData, { headers: { 'x-ms-blob-type': 'BlockBlob', 'Content-Type': this.file.type }, transformRequest: [] })
                    .success(() => {
                        this.readNextChunk(this.reader);
                    })
                    .error(() => {
                        this.uploadResult.reject();
                    });
            };

            this.readNextChunk(this.reader);

            return this.uploadResult.promise;
        }

        private readNextChunk = (reader: FileReader) => {
            if (this.totalBytesRemaining > 0) {
                var fileContent = this.file.slice(this.currentFilePointer, this.currentFilePointer + this.maxBlockSize);

                var chunkId = this.generateChunkId(this.chunkIdPrefix, this.chunkIds.length);
                this.chunkIds.push(chunkId);

                reader.readAsArrayBuffer(fileContent);

                this.currentFilePointer += this.maxBlockSize;
                this.totalBytesRemaining -= this.maxBlockSize;

                if (this.totalBytesRemaining < this.maxBlockSize) {
                    this.maxBlockSize = this.totalBytesRemaining;
                }
            } else {
                this.commitChunkList();
            }
        }

        private commitChunkList = () => {
            var uri = this.fileUrl + '&comp=blocklist';
            var requestBody = this.generateChunksCommit(this.chunkIds);

            this.$http.put(uri, requestBody, { headers: { 'x-ms-blob-content-type': this.file.type } })
                .success(() => {
                    this.uploadResult.resolve();
                })
                .error(() => {
                    this.uploadResult.reject();
                });
        };

        private generateChunkId = (uploadId: string, chunkNumber: number) => {
            var chunkId = uploadId + _.padLeft(String(chunkNumber), 6, '0');
            var chunkIdInBase64 = btoa(chunkId);

            return chunkIdInBase64;
        }

        private generateChunksCommit = (chunkIds: string[]) => {
            var requestBody = '<?xml version="1.0" encoding="utf-8"?><BlockList>';
            for (var i = 0; i < chunkIds.length; i++) {
                requestBody += '<Latest>' + chunkIds[i] + '</Latest>';
            }
            requestBody += '</BlockList>';

            return requestBody;
        }

        static getInstance() {
            var azureBlobUploadFactory = ($http: ng.IHttpService, appConfig: Common.Models.IAppConfig, $q: ng.IQService): AzureBlobUploadFactory => {
                return new AzureBlobUploadFactory($http, appConfig, $q);
            };

            azureBlobUploadFactory['$inject'] = ["$http", "appConfig", "$q"];
            return azureBlobUploadFactory;
        }
    }
    
    angular.module('app').factory('azureBlobUploadFactory', AzureBlobUploadFactory.getInstance());
}