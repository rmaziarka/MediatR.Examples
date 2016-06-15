/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AttachmentPreviewCardController = Common.Component.AttachmentPreviewCardController;
    import Business = Common.Models.Business;

    interface IAttachmentPreviewCardScope extends ng.IScope {
        entityId: string;
        entityType: string;
        attachment: Business.Attachment;
    }

    describe('Given attachment preview card component', () => {
        var scope: IAttachmentPreviewCardScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            filter: ng.IFilterService,
            fileSizeFilter = Mock.FileSize.generate(),
            controller: AttachmentPreviewCardController;

        var pageObjectSelectors = {
            fileNameLink: '#attachment-preview-fileName a',
            fileNameSpan: '#attachment-preview-fileName span',
            fileTypeId: '#attachment-preview-type',
            size: '#attachment-preview-size',
            createdDate: '#attachment-preview-created-date',
            user: '#attachment-preview-user'
        }

        describe('when attachment is set', () =>{
            var entityId = 'testEntityId',
                entityType = 'TestEntity'

            var attachment = TestHelpers.AttachmentGenerator.generate({ user: new Business.User({ id: 'us1', firstName: 'firstName1', lastName: 'lastName1', departmentId: 'depId', department: null }) });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService,
                $filter: ng.IFilterService) => {

                $http = $httpBackend;
                filter = $filter;
                scope = <IAttachmentPreviewCardScope>$rootScope.$new();

                // TODO: try url with complete params list - it doesnt work - mayby some misspeling
                //var expectedUrl = new RegExp(`\/api\/services\/attachment\/download\/${entityType}?documentTypeId=${attachment.documentTypeId}&localeIsoCode=en&externalDocumentId=${attachment.externalDocumentId}&entityReferenceId=${entityId}&filename=${attachment.documentTypeId}`);
                var expectedUrl = new RegExp(`\/api\/services\/attachment\/download\/${entityType}`);
                $http.whenGET(expectedUrl).respond(() => {
                    return [200, <Antares.Common.Models.Dto.IAzureDownloadUrlContainer>{ url: '' }];
                });

                scope.entityId = entityId;
                scope.entityType = entityType;
                scope.attachment = attachment;

                element = $compile('<attachment-preview-card entity-id="entityId" entity-type="{{entityType}}" attachment="attachment"></attachment-preview-card>')(scope);
                $http.flush();
                scope.$apply();

                // arrange + act
                controller = element.controller('attachmentPreviewCard');
            }));

            it('when attachment is changed then API request for url is called', () => {
                // arrange / act
                var responseUrl = 'responseUrl';

                var expectedUrl = new RegExp(`\/api\/services\/attachment\/download\/${entityType}`);
                $http.expectGET(expectedUrl).respond(() => {
                    return [200, <Antares.Common.Models.Dto.IAzureDownloadUrlContainer>{ url: responseUrl }];
                });

                // act
                scope.attachment = TestHelpers.AttachmentGenerator.generate();
                $http.flush();

                // assert
                expect(controller.attachmentUrl).toBe(responseUrl);
            });

            it('then attachment file name value should be displayed as link if file url exists', () => {
                // arrange / act
                controller.attachmentUrl = 'some_url';
                scope.$apply();

                // assert
                var fileNameElement = element.find(pageObjectSelectors.fileNameLink);
                expect(fileNameElement.text()).toBe(attachment.fileName);
            });

            it('then attachment file name value should be displayed as span if file url does not exists', () => {
                // arrange / act
                controller.attachmentUrl = '';
                scope.$apply();

                // assert
                var fileNameElement = element.find(pageObjectSelectors.fileNameSpan);
                expect(fileNameElement.text()).toBe(attachment.fileName);
            });

            it('then attachment file type value should be displayed', () => {
                // arrange / act / assert
                var fileTypeIdElement = element.find(pageObjectSelectors.fileTypeId);
                expect(fileTypeIdElement.text()).toBe('DYNAMICTRANSLATIONS.' + attachment.documentTypeId);
            });

            it('then attachment file size value should be displayed', () => {
                // arrange / act / assert
                expect(fileSizeFilter).toHaveBeenCalledWith(attachment.size);
            });

            it('then attachment creation date value should be displayed in proper format', () => {
                // arrange / act / assert
                var formattedDate = filter('date')(attachment.createdDate, 'dd-MM-yyyy');
                var dateElement = element.find(pageObjectSelectors.createdDate);
                expect(dateElement.text()).toBe(formattedDate);
            });

            it('then attachment user value should be displayed', () => {
                // arrange / act / assert
                var userElement = element.find(pageObjectSelectors.user);
                expect(userElement.text()).toBe('firstName1 lastName1');
            });
        });
    });
}