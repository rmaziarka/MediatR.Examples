/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import RequirementViewController = Requirement.View.RequirementViewController;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    describe('Given requirement view controller', () => {
        var scope: ng.IScope,
            evtAggregator: Core.EventAggregator,
            controller: RequirementViewController,
            $http: ng.IHttpBackendService;

        var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            eventAggregator: Core.EventAggregator,
            $httpBackend: ng.IHttpBackendService) => {
            scope = $rootScope.$new();
            $http = $httpBackend;
            evtAggregator = eventAggregator;

            var bindings = { requirement: requirementMock };
            controller = <RequirementViewController>$controller("requirementViewController", { $scope: scope }, bindings);
        }));

        beforeAll(() => {
            jasmine.addMatchers(TestHelpers.CustomMatchers.AttachmentCustomMatchersGenerator.generate());
        });

        describe('when AttachmentSavedEvent event is triggered', () => {
            it('then addSavedAttachmentToList should be called', () => {
                // arrange
                spyOn(controller, 'addSavedAttachmentToList');

                var attachmentDto = TestHelpers.AttachmentGenerator.generateDto();
                var command = new Common.Component.Attachment.AttachmentSavedEvent(attachmentDto);

                // act
                evtAggregator.publish(command);

                // assert
                expect(controller.addSavedAttachmentToList).toHaveBeenCalledWith(command.attachmentSaved);
            });
        });

        describe('when addSavedAttachmentToList is called', () => {
            it('then attachment should be added to list', () => {
                // arrange
                var attachmentDto = TestHelpers.AttachmentGenerator.generateDto();
                controller.requirement.attachments = [];

                // act
                controller.addSavedAttachmentToList(attachmentDto);

                // assert
                var expectedAttachment = new Business.Attachment(attachmentDto);
                expect(controller.requirement.attachments[0]).toBeSameAsAttachment(expectedAttachment);
            });
        });

        describe('when saveAttachment is called', () => {
            it('then proper API request should be sent', () => {
                // arrange
                var attachmentModel = TestHelpers.AttachmentUploadCardModelGenerator.generate();
                var requestData: Requirement.Command.RequirementAttachmentSaveCommand;

                var expectedUrl = `/api/requirements/${controller.requirement.id}/attachments/`;
                $http.expectPOST(expectedUrl, (data: string) => {
                    requestData = JSON.parse(data);
                    return true;
                }).respond(201, {});

                // act
                controller.saveAttachment(attachmentModel);
                $http.flush();

                // assert
                expect(requestData).not.toBe(null);
                expect(requestData.requirementId).toBe(controller.requirement.id);
                expect(requestData.attachment).toBeSameAsAttachmentModel(attachmentModel);
            });
        });

        describe('when requirement exists', () => {
            var requirementViewModel: Business.RequirementViewModel = null;

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any) => {
                var requirementDto = TestHelpers.RequirementGenerator.generateDto();
                requirementViewModel = new Business.RequirementViewModel(requirementDto);
                requirementViewModel.offers = TestHelpers.OfferGenerator.generateMany(1);

                scope = $rootScope.$new();
                var bindings = { requirement: requirementViewModel };
                controller = <RequirementViewController>$controller("requirementViewController", { $scope: scope }, bindings);
            }));

            describe('when requirement type is residential letting', () => {
                it('and an offer has status accepted and no tenancy exists then tenancy can be negotiated', () => {
                    // arrange
                    var offerStatus: any = TestHelpers.EnumTypeItemGenerator.generateDto("Accepted");
                    controller.requirement.offers[0].status = offerStatus;

                    // act
                    var result = controller.canTenancyBeNegotiated(requirementViewModel.offers[0]);

                    // assert
                    expect(result).toBe(true);
                });

                it('and there are no offers in status accepted then tenancy cannot be negotiatied', () => {
                    // arrange
                    var offerStatus: any = TestHelpers.EnumTypeItemGenerator.generateDto("New");
                    controller.requirement.offers[0].status = offerStatus;

                    // act
                    var result = controller.canTenancyBeNegotiated(requirementViewModel.offers[0]);

                    // assert
                    expect(result).toBe(false);
                });

                it('and there is accepted offer and tenancy exists then cannot negotiate another tenancy', () => {
                    // arrange
                    var offerStatus: any = TestHelpers.EnumTypeItemGenerator.generateDto("Accepted");
                    controller.requirement.offers[0].status = offerStatus;
                    controller.requirement.tenancy = TestHelpers.TenancyGenerator.generateDto();

                    // act
                    var result = controller.canTenancyBeNegotiated(requirementViewModel.offers[0]);

                    // assert
                    expect(result).toBe(false);
                });

                it('and offer does not exist then cannot negotiate tenancy', () => {
                    // arrange & act
                    var result = controller.canTenancyBeNegotiated(null);

                    // assert
                    expect(result).toBe(false);
                });
            });

            describe('when requirement type is residential sale', () => {
                it('then tenancy cannot be negotiatied', () => {
                    // arrange
                    controller.requirement.requirementType.enumCode = Antares.Common.Models.Enums.RequirementType[Antares.Common.Models.Enums.RequirementType.ResidentialSale];
                    var offerStatus: any = TestHelpers.EnumTypeItemGenerator.generateDto("Accepted");
                    controller.requirement.offers[0].status = offerStatus;
                    controller.requirement.tenancy = null;

                    // act
                    var result = controller.canTenancyBeNegotiated(requirementViewModel.offers[0]);

                    // assert
                    expect(result).toBe(false);
                });
            });
        });
    });
}