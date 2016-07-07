/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import PropertyViewController = Property.View.PropertyViewController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    describe('Given property view controller', () => {
        var controller: PropertyViewController,
            $http: ng.IHttpBackendService,
            evtAggregator: Antares.Core.EventAggregator,
            scope: ng.IScope;

        var propertyMock = TestHelpers.PropertyGenerator.generatePropertyView();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            eventAggregator: Antares.Core.EventAggregator,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            evtAggregator = eventAggregator;
            scope = $rootScope.$new();

            var bindings = { property: propertyMock };
            controller = <PropertyViewController>$controller('propertyViewController', { $scope: scope }, bindings);
        }));

        beforeAll(() => {
            jasmine.addMatchers(TestHelpers.CustomMatchers.AttachmentCustomMatchersGenerator.generate());
        });

        describe('when showAreaAdd is executed', () => {
            var clearAreasSpy = jasmine.createSpy('clearAreasSpy');
            var shownPanel: any;

            beforeEach(() => {
                spyOn(controller.components, 'areaAdd').and.returnValue({
                    clearAreas: clearAreasSpy
                });

                spyOn(controller, 'showPanel').and.callFake((panel: any) => { shownPanel = panel; });
            });

            it('then clearAreas should be called on add area component', () => {
                // act
                controller.showAreaAdd();

                // assert
                expect(clearAreasSpy).toHaveBeenCalled();
            });

            it('then side panel for adding area should be shown', () => {
                // act
                controller.showAreaAdd();

                // assert
                expect(shownPanel).toBe(controller.components.panels.areaAdd);
            });
        });

        describe('when saveArea is executed', () => {
            var $q: ng.IQService;
            var saveAreaDefferedMock: ng.IDeferred<Business.PropertyAreaBreakdown[]>;

            beforeEach(inject((
                _$q_: ng.IQService) => {

                $q = _$q_;

                saveAreaDefferedMock = $q.defer();

                spyOn(controller.components, 'areaAdd').and.returnValue({
                    saveAreas: (propertyId: string) => {
                        return saveAreaDefferedMock.promise;
                    }
                });
                spyOn(controller, 'cancelAddArea').and.callFake(() => { });
            }));

            it('then new areas should be added to the areas list', () => {
                // arrange
                var newAreasCount: number = 3;
                var areas: Business.PropertyAreaBreakdown[] = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generateMany(newAreasCount);

                // act
                controller.saveArea();
                saveAreaDefferedMock.resolve(areas);
                scope.$apply();

                // assert
                expect(controller.property.propertyAreaBreakdowns.length).toBe(newAreasCount);
                expect(controller.property.propertyAreaBreakdowns).toContain(areas[0]);
                expect(controller.property.propertyAreaBreakdowns).toContain(areas[1]);
                expect(controller.property.propertyAreaBreakdowns).toContain(areas[2]);
            });

            it('then side panel for adding areas should be hiden', () => {
                // act
                controller.saveArea();
                saveAreaDefferedMock.resolve();
                scope.$apply();

                // assert
                expect(controller.cancelAddArea).toHaveBeenCalled();
            });
        });

        describe('when showAreaEdit is executed', () => {
            var editPropertyAreaBreakdownSpy = jasmine.createSpy('editPropertyAreaBreakdownSpy');
            var shownPanel: any;

            beforeEach(() => {
                spyOn(controller.components, 'areaEdit').and.returnValue({
                    editPropertyAreaBreakdown : editPropertyAreaBreakdownSpy
                });

                spyOn(controller, 'showPanel').and.callFake((panel: any) =>{ shownPanel = panel; });
            });

            it('then editPropertyAreaBreakdown should be called on edit area component with area', () => {
                // arrange
                var areas: Business.PropertyAreaBreakdown[] = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generateMany(4);
                var areaToUpdate = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate();

                controller.property.propertyAreaBreakdowns = areas;
                areaToUpdate.id = areas[1].id;

                // act
                controller.showAreaEdit(areaToUpdate);

                // assert
                expect(editPropertyAreaBreakdownSpy).toHaveBeenCalledWith(areaToUpdate);
            });

            it('then side panel for updating area should be shown', () => {
                // arrange
                var areaToUpdate = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate();
                controller.property.propertyAreaBreakdowns.push(areaToUpdate);

                // act
                controller.showAreaEdit(areaToUpdate);

                // assert
                expect(shownPanel).toBe(controller.components.panels.areaEdit);
            });
        });

        describe('when updateArea is executed', () => {
            var $q: ng.IQService;
            var updateAreaDefferedMock: ng.IDeferred<Business.PropertyAreaBreakdown>;

            beforeEach(inject((
                _$q_: ng.IQService) => {

                $q = _$q_;

                updateAreaDefferedMock = $q.defer();

                spyOn(controller.components, 'areaEdit').and.returnValue({
                    updatePropertyAreaBreakdown: (propertyId: string) => {
                        return updateAreaDefferedMock.promise;
                    }
                });
                spyOn(controller, 'cancelEditArea').and.callFake(() => { });
            }));

            it('then area should be updated within the areas list', () => {
                // arrange
                var areas: Business.PropertyAreaBreakdown[] = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generateMany(4);
                var updatedArea = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate();

                controller.property.propertyAreaBreakdowns = areas;
                updatedArea.id = areas[1].id;

                // act
                controller.updateArea();
                updateAreaDefferedMock.resolve(updatedArea);
                scope.$apply();

                // assert
                expect(controller.property.propertyAreaBreakdowns[1]).toEqual(updatedArea);
            });

            it('then side panel for updating area should be hiden', () => {
                // arrange
                var updatedArea = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate();
                controller.property.propertyAreaBreakdowns.push(updatedArea);

                // act
                controller.updateArea();
                updateAreaDefferedMock.resolve(updatedArea);
                scope.$apply();

                // assert
                expect(controller.cancelEditArea).toHaveBeenCalled();
            });
        });

        describe('when area is dragged and dropped', () => {
            var $http: ng.IHttpBackendService,
                event: Common.Models.IDndEvent,
                areaToDrag: Business.PropertyAreaBreakdown,
                newIndexOfDraggedArea: number = 1;

            beforeEach(inject((
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;
                areaToDrag = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate();

                controller.property.propertyAreaBreakdowns.push(areaToDrag);

                var eventSource: Common.Models.IDndEventSource = <Common.Models.IDndEventSource>{
                    itemScope: {
                        modelValue: areaToDrag
                    }
                }

                var eventDest: Common.Models.IDndEventDest = <Common.Models.IDndEventDest>{
                    index: newIndexOfDraggedArea
                }

                event = <Common.Models.IDndEvent>{
                    source: eventSource,
                    dest: eventDest
                };

            }));

            it('then new position should be saved', () => {
                // arrange
                var requestData: Dto.IUpdatePropertyAreaBreakdownOrderResource;

                $http.expectPUT(/\/api\/properties\/[0-9a-zA-Z]*\/areabreakdown\/order/, (data: string) => {
                    requestData = JSON.parse(data);
                    return true;
                }).respond(201, {});

                // act
                controller.onAreaDraggedAndDropped()(event);
                scope.$apply();

                // assert
                expect(requestData).not.toBe(null);
                expect(requestData.areaId).toBe(areaToDrag.id);
                expect(requestData.order).toBe(newIndexOfDraggedArea);
                expect(requestData.propertyId).toBe(controller.property.id);
            });
        });

        describe('when AttachmentSavedEvent event is triggered', () => {
            it('then addSavedAttachmentToList should be called', () => {
                // arrange
                spyOn(controller, 'addSavedAttachmentToList')

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
                controller.property.attachments = [];

                // act
                controller.addSavedAttachmentToList(attachmentDto);

                // assert
                var expectedAttachment = new Business.Attachment(attachmentDto);
                expect(controller.property.attachments[0]).toBeSameAsAttachment(expectedAttachment);
            });
        });

        describe('when saveAttachment is called', () => {
            it('then proper API request should be sent', () => {
                // arrange
                var attachmentModel = TestHelpers.AttachmentUploadCardModelGenerator.generate();
                var requestData: Property.Command.PropertyAttachmentSaveCommand;

                var expectedUrl = `/api/properties/${controller.property.id}/attachments/`;
                $http.expectPOST(expectedUrl, (data: string) => {
                    requestData = JSON.parse(data);
                    return true;
                }).respond(201, {});

                // act
                controller.saveAttachment(attachmentModel);
                $http.flush();

                // assert
                expect(requestData).not.toBe(null);
                expect(requestData.propertyId).toBe(controller.property.id);
                expect(requestData.attachment).toBeSameAsAttachmentModel(attachmentModel);

            });
        });
    });
}