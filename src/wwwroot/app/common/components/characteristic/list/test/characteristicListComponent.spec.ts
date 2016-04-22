/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CharacteristicListController = Common.Component.CharacteristicListController;
    import Business = Common.Models.Business;

    describe('Given characteristic list component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            compile: ng.ICompileService,
            controller: CharacteristicListController;

        var pageObjectSelectors = {
            characteristicsGroups: 'div[data-type="characteristics-group"]',
            characteristicsGroupIdPrefix: 'div#characteristics-group-'
        };

        describe('when component has been loaded', () => {
            var propertyMock: Business.Property = TestHelpers.PropertyGenerator.generate({
                propertyTypeId: 'testPropertyTypeId'
            });

            var characteristicGroupsMock = [
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId : 'groupid1', order : 2 }),
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId : 'groupid2', order : 1, isDisplayLabel : false }),
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId : 'groupid3', order : 3,
                    characteristicGroupItems : [
                        TestHelpers.CharacteristicGroupItemGenerator.generateDto({}),
                        TestHelpers.CharacteristicGroupItemGenerator.generateDto({}),
                        TestHelpers.CharacteristicGroupItemGenerator.generateDto({})]
                })
            ];

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // arrange
                scope = $rootScope.$new();
                compile = $compile;
                $http = $httpBackend;

                $http.expectGET(/\/api\/characteristicGroups\?countryCode=GB&propertyTypeId=testPropertyTypeId/).respond(() => {
                    return [200, characteristicGroupsMock];
                });

                // act
                scope['property'] = propertyMock;
                element = compile('<characteristic-list property="property"></characteristic-list>')(scope);
                scope.$apply();
                controller = element.controller('characteristicList');

                $http.flush();
            }));

            it('then characteristics form has proper characteristics grouped and ordered', () => {
                // assert
                var groupElements = element.find(pageObjectSelectors.characteristicsGroups);
                var group1Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid1');
                var group2Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid2');
                var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');

                var group3CharacteristicsElements = group3Element.find('characteristic-select');

                expect(groupElements.length).toBe(3);
                expect(groupElements[0]).toBe(group2Element[0]);
                expect(groupElements[1]).toBe(group1Element[0]);
                expect(groupElements[2]).toBe(group3Element[0]);

                expect(group3CharacteristicsElements.length).toBe(3);

                expect(group1Element.find('[translate="DYNAMICTRANSLATIONS.groupid1"]').length).toBe(1);
                expect(group2Element.find('[translate="DYNAMICTRANSLATIONS.groupid2"]').length).toBe(0);
                expect(group3Element.find('[translate="DYNAMICTRANSLATIONS.groupid3"]').length).toBe(1);
            });
        });
    });
}