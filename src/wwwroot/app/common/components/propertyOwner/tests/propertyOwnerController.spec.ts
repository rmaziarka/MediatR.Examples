/// <reference path="../../../../typings/_all.d.ts" />

module Antares {

    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import runDescribe = TestHelpers.runDescribe;
    import PropertyOwnerController = Antares.Common.Component.PropertyOwnerController;

    var leaseholderType: Dto.IEnumItem = { id: "leaseholderid", code: Enums.OwnershipTypeEnum.Leaseholder.toString() };
    var freeholderType: Dto.IEnumItem = { id: "freeholderid", code: Enums.OwnershipTypeEnum.Freeholder.toString() };


    describe('Given ownerships for property', () => {
        var $scope: ng.IScope,

            createGeneratedOwnership: (specificData?: any) => Business.PropertySearchResultOwnership,
            createController: (ownerships: Business.PropertySearchResultOwnership[]) => PropertyOwnerController;

        beforeEach(inject((
            enumService: Mock.EnumServiceMock,
            $rootScope: ng.IRootScopeService,
            $componentController: any) => {

            // init
            enumService.setEnum(Dto.EnumTypeCode.OwnershipType.toString(), [leaseholderType, freeholderType]);

            $scope = $rootScope.$new();

            createGeneratedOwnership = (specificData?: any): Business.PropertySearchResultOwnership => {
                return TestHelpers.PropertySearchResultOwnershipGenerator.generate(specificData);
            }

            createController = (ownerships: Business.PropertySearchResultOwnership[]): PropertyOwnerController => {
                var bindings = { ownerships: ownerships };
                return <PropertyOwnerController>$componentController('propertyOwner', { $scope: $scope }, bindings);
            };
        }));

        describe('when use propertyOwner controller', () => {
            var currectLeaseholder: Business.PropertySearchResultOwnership,
                pastLeaseholder: Business.PropertySearchResultOwnership,
                currentFreeholder: Business.PropertySearchResultOwnership,
                pastFreeholder: Business.PropertySearchResultOwnership;

            var assertList: (givenData: Business.PropertySearchResultContact[], expectedData: Business.PropertySearchResultContact[]) => void;

            beforeEach(() => {
                currectLeaseholder = createGeneratedOwnership({ ownershipTypeId: leaseholderType.id, sellDate: null });
                pastLeaseholder = createGeneratedOwnership({ ownershipTypeId: leaseholderType.id });
                currentFreeholder = createGeneratedOwnership({ ownershipTypeId: freeholderType.id, sellDate: null });
                pastFreeholder = createGeneratedOwnership({ ownershipTypeId: freeholderType.id });

                assertList = (givenData: Business.PropertySearchResultContact[], expectedData: Business.PropertySearchResultContact[]) => {
                    expect(givenData.length).toEqual(expectedData.length);

                    givenData = _.sortBy(givenData, ['id']);
                    expectedData = _.sortBy(expectedData, ['id']);

                    for (var index = 0; index < givenData.length; index++) {
                        expect(givenData[index]).toEqual(expectedData[index]);
                    }
                }
            });

            describe('and exist current Leaseholder ownership', () => {
                it('then should show component', () => {
                    // arrange
                    var ownerships: Business.PropertySearchResultOwnership[] = [
                        currectLeaseholder, pastLeaseholder, currentFreeholder, pastLeaseholder
                    ];

                    // act
                    var controller = createController(ownerships);
                    $scope.$apply();

                    // assert
                    expect(controller.isShow()).toEqual(true);
                    expect(controller.currentOwnershipTypeId).toEqual(leaseholderType.id);

                    assertList(controller.currentContacts, currectLeaseholder.contacts);

                });
            });

            describe('and exist current Freeholder ownership', () => {
                it('then should show component', () => {
                    // arrange
                    var ownerships: Business.PropertySearchResultOwnership[] = [
                        pastLeaseholder, currentFreeholder, pastLeaseholder
                    ];

                    // act
                    var controller = createController(ownerships);
                    $scope.$apply();

                    // assert
                    expect(controller.isShow()).toEqual(true);
                    expect(controller.currentOwnershipTypeId).toEqual(freeholderType.id);

                    assertList(controller.currentContacts, currentFreeholder.contacts);
                });
            });

            describe('and not exist current ownership', () => {
                it('then should not show component', () => {
                    // arrange
                    var ownerships: Business.PropertySearchResultOwnership[] = [
                        pastLeaseholder,
                        pastFreeholder
                    ];

                    // act
                    var controller = createController(ownerships);
                    $scope.$apply();

                    // assert
                    expect(controller.isShow()).toEqual(false);
                });
            });


        });
    });
}