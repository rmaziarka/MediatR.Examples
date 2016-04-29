/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import CharacteristicListController = Common.Component.CharacteristicListController;
    import Dto = Common.Models.Dto;

    import mockTranslateFilterForCharacteristicGroupItems = Mock.TranslateFilter.mockTranslateFilterForCharacteristicGroupItems;

    describe('Given characteristic list view component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            compile: ng.ICompileService,
            controller: CharacteristicListController;

        var pageObjectSelectors = {
            characteristicsGroups: 'div[data-type="characteristics-group"]',
            characteristicsGroupIdPrefix: 'div#characteristics-group-',
            characteristicItemName: 'span[data-type=characteristic-name]',
            characteristicItemText: 'span[data-type=characteristic-comment]'
        };

        var propertyTypes: any = {
            House: { id: "8b152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "House", children: [], order: 22 }
        }
        var countries: any = {
            GB: { id: 'countryGB', isoCode: 'GB' }
        };

        describe('when component has been loaded', () => {
            var characteristicGroup3ItemsMock: Array<Dto.ICharacteristicGroupItem> = TestHelpers.CharacteristicGroupItemGenerator.generateManyDtos(3);
            var characteristicGroup4ItemsMock: Array<Dto.ICharacteristicGroupItem> = TestHelpers.CharacteristicGroupItemGenerator.generateManyDtos(3);
            var enabledCharacteristicGroup1Item = TestHelpers.CharacteristicGroupItemGenerator.generateDto();
            var disabledCharacteristicGroup1Item = TestHelpers.CharacteristicGroupItemGenerator.generateDto();
            disabledCharacteristicGroup1Item.characteristic.isEnabled = false;

            var characteristicGroupsMock = [
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId: 'groupid1', order: 4, characteristicGroupItems: [enabledCharacteristicGroup1Item, disabledCharacteristicGroup1Item] }),
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId: 'groupid2', order: 1 }),
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId: 'groupid3', order: 3, isDisplayLabel: false, characteristicGroupItems: characteristicGroup3ItemsMock }),
                TestHelpers.CharacteristicGroupUsageGenerator.generateDto({ characteristicGroupId: 'groupid4', order: 2, characteristicGroupItems: characteristicGroup4ItemsMock })
            ];

            var characteristicsMapMock: any = {};
            characteristicsMapMock[enabledCharacteristicGroup1Item.characteristic.id] = TestHelpers.CharacteristicSelectGenerator.generate({ characteristicId: enabledCharacteristicGroup1Item.characteristic.id });
            characteristicsMapMock[disabledCharacteristicGroup1Item.characteristic.id] = TestHelpers.CharacteristicSelectGenerator.generate({ characteristicId: disabledCharacteristicGroup1Item.characteristic.id });
            characteristicsMapMock[characteristicGroup3ItemsMock[1].characteristic.id] = TestHelpers.CharacteristicSelectGenerator.generate({ characteristicId: characteristicGroup3ItemsMock[1].characteristic.id, text: 'TestText' });
            characteristicsMapMock[characteristicGroup3ItemsMock[2].characteristic.id] = TestHelpers.CharacteristicSelectGenerator.generate({ characteristicId: characteristicGroup3ItemsMock[2].characteristic.id, text: '' });
            characteristicsMapMock['otherCharacteristicId'] = TestHelpers.CharacteristicSelectGenerator.generate({ characteristicId: 'otherCharacteristicId' });

            var requestMock = new RegExp('/api/characteristicGroups\\?countryId=' + countries.GB.id + '&propertyTypeId=' + propertyTypes.House.id);

            mockTranslateFilterForCharacteristicGroupItems(characteristicGroup3ItemsMock, ['alfa', 'gamma', 'beta']);

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
                element = compile('<characteristic-list-view characteristics-map="characteristicsMap"></characteristic-list-view>')(scope);
                scope.$apply();
                controller = element.controller('characteristicListView');

                controller.loadCharacteristics(propertyTypes.House.id, countries.GB.id);
                $http.flush();
            }));

            describe('then characteristics list', () => {

                it('displays characteristics groups that contain characteristics with data', () => {
                    // assert
                    var groupElements = element.find(pageObjectSelectors.characteristicsGroups);

                    expect(groupElements.length).toBe(2);
                });

                it('displays characteristics groups sorted by order', () => {
                    // assert
                    var groupElements = element.find(pageObjectSelectors.characteristicsGroups);
                    var group1Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid1');
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');

                    expect(groupElements[0]).toBe(group3Element[0]);
                    expect(groupElements[1]).toBe(group1Element[0]);
                });

                it('displays proper characteristics groups name', () => {
                    // assert
                    var group1Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid1');
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');

                    expect(group1Element.find('[data-characteristicgroup-id="groupid1"]').length).toBe(1);
                    expect(group3Element.find('[data-characteristicgroup-id="groupid3"]').length).toBe(0);
                });

                it('displays characterictic items that contain data', () => {
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');
                    var group3CharacteristicsElements = group3Element.find('li');

                    expect(group3CharacteristicsElements.find(pageObjectSelectors.characteristicItemName).length).toBe(2);
                });

                it('displays characterictic items text if its set', () => {
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');
                    var characteristicItem1Element = group3Element.find('li[data-characteristic-id="' + characteristicGroup3ItemsMock[1].characteristic.id + '"]');
                    var characteristicItem2Element = group3Element.find('li[data-characteristic-id="' + characteristicGroup3ItemsMock[2].characteristic.id + '"]');

                    expect(characteristicItem1Element.find(pageObjectSelectors.characteristicItemText).length).toBe(1);
                    expect(characteristicItem1Element.find(pageObjectSelectors.characteristicItemText)[0].innerText.trim()).toBe("TestText");
                    expect(characteristicItem2Element.find(pageObjectSelectors.characteristicItemText).length).toBe(0);
                });

                it('displays disabled characterictic items that contain data', () => {
                    var group1Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid1');
                    var group1CharacteristicsElements = group1Element.find('li');

                    expect(group1CharacteristicsElements.length).toBe(2);
                });

                it('displays characterictic items sorted by translated name', () => {
                    var group3Element = element.find(pageObjectSelectors.characteristicsGroupIdPrefix + 'groupid3');

                    expect(group3Element.find('li').find(pageObjectSelectors.characteristicItemName)[0].innerText.trim()).toBe("beta");
                    expect(group3Element.find('li').find(pageObjectSelectors.characteristicItemName)[1].innerText.trim()).toBe("gamma");
                });
            });
        });
    });
}