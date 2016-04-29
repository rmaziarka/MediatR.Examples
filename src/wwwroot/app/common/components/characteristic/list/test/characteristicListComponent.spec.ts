/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CharacteristicListController = Common.Component.CharacteristicListController;
    import Dto = Common.Models.Dto;

    import mockTranslateFilterForCharacteristicGroupItems = Mock.TranslateFilter.mockTranslateFilterForCharacteristicGroupItems;

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

        var propertyTypes: any = {
            House: { id: "8b152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "House", children: [], order: 22 }
        }
        var countries: any = {
            GB: { id: 'countryGB', isoCode: 'GB' }
        };

        describe('when component has been loaded', () => {
            var characteristicGroupItemsMock: Array<Dto.ICharacteristicGroupItem> = TestHelpers.CharacteristicGroupItemGenerator.generateManyDtos(3);
            var disabledCharacteristicGroupItem = TestHelpers.CharacteristicGroupItemGenerator.generateDto();
            disabledCharacteristicGroupItem.characteristic.isEnabled = false;

            var characteristicGroupsMock = [
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId: 'groupid1', order: 2, characteristicGroupItems: [disabledCharacteristicGroupItem] }),
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId: 'groupid2', order: 1, isDisplayLabel: false }),
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId: 'groupid3', order: 3, characteristicGroupItems: characteristicGroupItemsMock })
            ];

            var characteristicsMapMock: any = {};
            characteristicsMapMock[characteristicGroupItemsMock[0].characteristic.id] = TestHelpers.CharacteristicSelectGenerator.generate({ characteristicId: characteristicGroupItemsMock[0].characteristic.id });
            characteristicsMapMock[characteristicGroupItemsMock[1].characteristic.id] = TestHelpers.CharacteristicSelectGenerator.generate({ characteristicId: characteristicGroupItemsMock[1].characteristic.id });
            characteristicsMapMock['otherCharacteristicId'] = TestHelpers.CharacteristicSelectGenerator.generate({ characteristicId: 'otherCharacteristicId' });

            var requestMock = new RegExp('/api/characteristicGroups\\?countryId=' + countries.GB.id + '&propertyTypeId=' + propertyTypes.House.id);

            mockTranslateFilterForCharacteristicGroupItems(characteristicGroupItemsMock, ['alfa', 'gamma', 'beta']);

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // arrange
                scope = $rootScope.$new();
                compile = $compile;
                $http = $httpBackend;


                $http.expectGET(requestMock).respond(() => {
                    return [200, characteristicGroupsMock];
                });

                // act
                scope['characteristicsMap'] = characteristicsMapMock;
                element = compile('<characteristic-list characteristics-map="characteristicsMap"></characteristic-list>')(scope);
                scope.$apply();
                controller = element.controller('characteristicList');

                controller.loadCharacteristics(propertyTypes.House.id, countries.GB.id);
                $http.flush();
            }));

            describe('then characteristics form', () => {
                it('displays all characteristics groups', () => {
                    // assert
                    var groupElements = element.find(pageObjectSelectors.characteristicsGroups);

                    expect(groupElements.length).toBe(3);
                });

                it('displays characteristics groups sorted by order', () => {
                    // assert
                    var groupElements = element.find(pageObjectSelectors.characteristicsGroups);
                    var group1Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid1');
                    var group2Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid2');
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');

                    expect(groupElements[0]).toBe(group2Element[0]);
                    expect(groupElements[1]).toBe(group1Element[0]);
                    expect(groupElements[2]).toBe(group3Element[0]);
                });

                it('displays proper characteristics groups name', () => {
                    // assert
                    var group1Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid1');
                    var group2Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid2');
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');

                    expect(group1Element.find('[data-characteristicgroup-id="groupid1"]').length).toBe(1);
                    expect(group2Element.find('[data-characteristicgroup-id="groupid2"]').length).toBe(0);
                    expect(group3Element.find('[data-characteristicgroup-id="groupid3"]').length).toBe(1);
                });

                it('has characterictic items sorted by translated name', () => {
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');

                    var group3CharacteristicsElements = group3Element.find('characteristic-select');

                    expect(group3CharacteristicsElements.length).toBe(3);
                    expect(group3Element.find('characteristic-select').find('label')[0].innerText.trim()).toBe("alfa");
                    expect(group3Element.find('characteristic-select').find('label')[1].innerText.trim()).toBe("beta");
                    expect(group3Element.find('characteristic-select').find('label')[2].innerText.trim()).toBe("gamma");
                });

                it('should not display disabled characterictics', () => {
                    var group1Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid1');

                    var group1CharacteristicsElements = group1Element.find('characteristic-select');

                    expect(group1CharacteristicsElements.length).toBe(0);
                });

                it('should display mapped property characteristics values within characteristics form', () => {
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');
                    var characteristicSelectElement1 = group3Element.find('characteristic-select[data-characteristic-id="' + characteristicGroupItemsMock[0].characteristic.id + '"]');

                    expect(characteristicSelectElement1.find('input[type="checkbox"]').is(':checked')).toBeTruthy();
                    expect(characteristicSelectElement1.find('input[type="text"]').val().trim()).toBe(characteristicsMapMock[characteristicGroupItemsMock[0].characteristic.id].text);
                });

                it('should display empty values within characteristics form when no data for characteristic from property', () => {
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');
                    var characteristicSelectElement3 = group3Element.find('characteristic-select[data-characteristic-id="' + characteristicGroupItemsMock[2].characteristic.id + '"]');

                    expect(characteristicSelectElement3.find('input[type="checkbox"]').is(':checked')).toBeFalsy();
                    expect(characteristicSelectElement3.find('input[type="text"]').val().trim()).toBe('');
                });
            });
        });
    });
}