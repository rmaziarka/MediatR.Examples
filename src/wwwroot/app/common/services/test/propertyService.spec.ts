/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    describe('propertyService', () => {
        var service: Services.PropertyService;
        var http: ng.IHttpBackendService;
        var q: ng.IQService;

        beforeEach(inject((propertyService: Services.PropertyService, $q: ng.IQService, $httpBackend: ng.IHttpBackendService) => {
            q = $q;
            service = propertyService;
            http = $httpBackend;
        }));

        describe('when getSearchResult is invoked', () => {
            it('then http request is invoked with proper parameters', () => {
                var queryParams: any = {
                    query: 'foo',
                    page: 12,
                    size: 50
                };

                var expectedParams = { query : queryParams.query, page : '12', size : '50' };

                var resultParams: any;

                http.expectGET(/\/api\/properties\/search\?.*/).respond((method, url, data, headers, params) => {
                    resultParams = params;
                    return [200, new Business.PropertySearchResult()];
                });

                service.getSearchResult(queryParams.query, queryParams.page, queryParams.size);
                http.flush();

                expect(resultParams).toEqual(expectedParams);
            });
        });
    });
}