/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attribues {
    import Dto = Common.Models.Dto;

    describe('Checkbox-list-edit-control', () => {
        var
            scope: ng.IScope,
            controller: CheckboxListEditControlController,
            schema: Attributes.ICheckboxListEditControlSchema = {
                formName: 'form',
                controlId: 'controlId',
                fieldName: 'field',
                translationKey: 'KEY',
                checkboxes: []
            },
            portals: Dto.IPortal[] = [
                {
                    id: 'portal1',
                    name: 'portal1'
                },
                {
                    id: 'portal2',
                    name: 'portal2'
                },
                {
                    id: 'portal3',
                    name: 'portal3'
                }
            ];

        describe('when initialized with simple model', () => {
            var
                simpleCheckboxes: Attributes.ICheckboxSchema[] = [
                    { translationKey: 'value1', value: 'value1' },
                    { translationKey: 'value2', value: 'value2' },
                    { translationKey: 'value3', value: 'value3' }
                ],
                simpleModel: string[] = ['value2'];

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any) => {

                scope = $rootScope.$new();
                schema.checkboxes = simpleCheckboxes;
                var bindings = { schema: schema, ngModel: simpleModel };
                controller = <CheckboxListEditControlController>$controller('CheckboxListEditControlController', { $scope: scope }, bindings);
            }));

            it('proper checkboxes are preselected', () => {
                expect(schema.checkboxes[0].selected).toBeFalsy();
                expect(schema.checkboxes[1].selected).toBeTruthy();
                expect(schema.checkboxes[2].selected).toBeFalsy();
            });

            it('when selected checkbox is clicked model is updated', () => {
                controller.changeSelection(schema.checkboxes[1]);

                expect(simpleModel.length).toEqual(0);
            });

            it('when unselected checkbox is clicked model is updated', () => {
                controller.changeSelection(schema.checkboxes[0]);

                expect(simpleModel[0]).toEqual(schema.checkboxes[0].value);
            });
        });


        describe('when initialized with complex model', () => {
            var
                complexChecbkoxes: Attributes.ICheckboxSchema[] = [
                    { translationKey: 'value1', value: portals[0] },
                    { translationKey: 'value2', value: portals[1] },
                    { translationKey: 'value3', value: portals[2] }
                ],
                complexModel: Dto.IPortal[] = [portals[1]];

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any) => {

                scope = $rootScope.$new();
                schema.checkboxes = complexChecbkoxes;
                schema.compareMember = 'id';
                var bindings = { schema: schema, ngModel: complexModel };
                controller = <CheckboxListEditControlController>$controller('CheckboxListEditControlController', { $scope: scope }, bindings);
            }));

            it('proper checkboxes are preselected', () => {
                expect(schema.checkboxes[0].selected).toBeFalsy();
                expect(schema.checkboxes[1].selected).toBeTruthy();
                expect(schema.checkboxes[2].selected).toBeFalsy();
            });

            it('when selected checkbox is clicked model is updated', () => {
                controller.changeSelection(schema.checkboxes[1]);

                expect(complexModel.length).toEqual(0);
            });

            it('when unselected checkbox is clicked model is updated', () => {
                controller.changeSelection(schema.checkboxes[0]);

                expect(complexModel[0].id).toEqual(schema.checkboxes[0].value.id);
            });
        });
    });
}