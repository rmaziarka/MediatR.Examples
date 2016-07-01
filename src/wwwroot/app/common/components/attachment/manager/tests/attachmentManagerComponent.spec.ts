/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AttachmentsManagerController = Common.Component.Attachment.AttachmentsManagerController;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    import Dto = Antares.Common.Models.Dto;
    import IAttachmentsManagerData = Antares.Common.Component.Attachment.IAttachmentsManagerData;

    interface IAttachmentsManagerScope extends ng.IScope {
        data: IAttachmentsManagerData
    }

    describe('Given attachment manager component', () => {
        var scope: IAttachmentsManagerScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            filter: ng.IFilterService,
            controller: AttachmentsManagerController,
            httpBackend: ng.IHttpBackendService;

        var pageObjectSelectors = {
            common: {
                detailsElement: '.card-body'
            },
            attachments: {
                list: 'card-list#card-list-attachments'
            }
        };

        describe('and attachments are loaded', () => {
            var entityId = 'testEntityId',
                entityType = Enums.EntityTypeEnum.Property,
                entityDocumentType = Dto.EnumTypeCode.PropertyDocumentType;

            var attachments: Business.Attachment[] = [];

            var fileSizeFilter = Mock.FileSize.generate();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService) => {

                filter = $filter;
                httpBackend = $httpBackend;
                scope = <IAttachmentsManagerScope>$rootScope.$new();
                compile = $compile;

                recreateManagerData();

                element = compile('<attachments-manager data="data"></attachments-manager>')(scope);
                scope.$apply();

                controller = element.controller('attachmentsManager');
            }));

            it('when no attachments then "no items" element should be visible', () => {
                // arrange / act
                attachments = [];
                recreateManagerData();
                scope.$apply();

                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                    cardListNoItemsElementContent = cardListNoItemsElement.find('[translate="ATTACHMENT.LIST.NO_ATTACHMENTS"]'),
                    cardListItemElement = cardListElement.find('card-list-item');

                expect(cardListNoItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(cardListNoItemsElementContent.length).toBe(1);
                expect(cardListItemElement.length).toBe(0);
            });

            it('when existing attachments then card components should be visible', () => {
                // arrange / act
                attachments = TestHelpers.AttachmentGenerator.generateMany(2);
                recreateManagerData();
                scope.$apply();

                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                    cardListItemElement = cardListElement.find('card-list-item'),
                    cardListItemCardElement = cardListItemElement.find('card');

                expect(cardListNoItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(cardListItemElement.length).toBe(2);
                expect(cardListItemCardElement.length).toBe(2);

            });

            it('when existing attachments then card components should have proper order', () => {
                // arrange / act
                var date1Mock = new Date('2016-01-12');
                var date2Mock = new Date('2016-01-18');
                attachments = TestHelpers.AttachmentGenerator.generateMany(2);
                attachments[0].createdDate = date1Mock;
                attachments[1].createdDate = date2Mock;

                recreateManagerData();

                scope.$apply();

                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListItemCardElements = cardListElement.find('card-list-item card'),
                    cardListItem1CardElement = cardListElement.find('card[id="attachment-card-' + attachments[0].id + '"]'),
                    cardListItem2CardElement = cardListElement.find('card[id="attachment-card-' + attachments[1].id + '"]');

                expect(cardListItemCardElements[0]).toBe(cardListItem2CardElement[0]);
                expect(cardListItemCardElements[1]).toBe(cardListItem1CardElement[0]);
            });

            it('when existing attachments then card components should have proper data', () => {
                // arrange / act
                var dateMock = new Date('2011-01-01');
                var attachmentMock = TestHelpers.AttachmentGenerator.generate({ user: new Business.User({ id: 'us1', firstName: 'firstName1', lastName: 'lastName1', departmentId: 'depId', department: null }) });
                attachmentMock.createdDate = dateMock;

                attachments = [attachmentMock];
                recreateManagerData();

                scope.$apply();

                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListItemElement = cardListElement.find('card-list-item'),
                    cardListItemCardElement = cardListItemElement.find('card[id="attachment-card-' + attachmentMock.id + '"]');

                var attachmentDataElement = cardListItemCardElement.find('[id="attachment-data-' + attachmentMock.id + '"]');
                var attachmentDateElement = cardListItemCardElement.find('[id="attachment-created-date-' + attachmentMock.id + '"]');
                var attachmentTypeElement = cardListItemCardElement.find('[id="attachment-type-' + attachmentMock.id + '"]');
                var attachmentFileSizeElement = cardListItemCardElement.find('[id="attachment-file-size-' + attachmentMock.id + '"]');

                expect(attachmentDataElement.text()).toBe(attachmentMock.fileName);
                var formattedDate = filter('date')(dateMock, 'dd-MM-yyyy');
                expect(attachmentDateElement.text()).toBe(formattedDate);
                expect(attachmentTypeElement.text()).toBe('DYNAMICTRANSLATIONS.' + attachmentMock.documentTypeId);
                expect(fileSizeFilter).toHaveBeenCalledWith(attachmentMock.size);
            });

            describe('and attachment details is clicked', () => {
                it('then selectedAttachment object should be updated', () => {
                    // arrange

                    var expectedUrl = new RegExp(`\/api\/services\/attachment\/download`);
                    httpBackend.whenGET(expectedUrl).respond(() => {
                        return [200, <Antares.Common.Models.Dto.IAzureDownloadUrlContainer>{ url: '' }];
                    });

                    attachments = TestHelpers.AttachmentGenerator.generateMany(4);
                    recreateManagerData();
                    scope.$apply();

                    var attachmentForDetailsClick = attachments[2];

                    // act
                    var cardElement = element.find('card[id="attachment-card-' + attachmentForDetailsClick.id + '"]');
                    cardElement.find(pageObjectSelectors.common.detailsElement).click();

                    // assert
                    expect(controller.selectedAttachment).toBe(attachmentForDetailsClick);
                });
            });

            describe('and limited to 1',
                () => {
                    beforeEach(() => {
                        element =
                            compile('<attachments-manager data="data" files-number-limit="1"></attachments-manager>')(scope);

                        controller = element.controller('attachmentsManager');
                    });

                    it('when existing 2 attachments then card component must show 1 item', () => {
                        // arrange / act
                        attachments = TestHelpers.AttachmentGenerator.generateMany(2);
                        recreateManagerData();
                        scope.$apply();

                        // assert
                        var cardListElement = element.find(pageObjectSelectors.attachments.list),
                            cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                            cardListItemElement = cardListElement.find('card-list-item'),
                            cardListItemCardElement = cardListItemElement.find('card');

                        expect(cardListNoItemsElement.hasClass('ng-hide')).toBeTruthy();
                        expect(cardListItemElement.length).toBe(1);
                        expect(cardListItemCardElement.length).toBe(1);

                    });
                });

            var recreateManagerData = () => {
                scope.data = {
                    entityId: entityId,
                    entityType: entityType,
                    attachments: attachments,
                    enumDocumentType: entityDocumentType,
                    isPreviewPanelVisible: Enums.SidePanelState.Untouched,
                    isUploadPanelVisible: Enums.SidePanelState.Untouched
                };
            }
        });
    });
}