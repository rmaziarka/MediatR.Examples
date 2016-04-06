/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    describe('Given ownership', () => {

        it('when is not current and is not freeholder should not have vendors', () => {
            var dtoOwnership: Dto.IOwnership = getOwnership();
            dtoOwnership.sellDate = new Date();
            dtoOwnership.ownershipType.code = Enums.OwnershipTypeEnum.Leaseholder;

            var ownership: Business.Ownership = new Business.Ownership(dtoOwnership);

            expect(ownership.isVendor()).toBe(false);
        });

        it('when is not current and is freeholder should not have vendors', () => {
            var dtoOwnership: Dto.IOwnership = getOwnership();
            dtoOwnership.sellDate = new Date();
            dtoOwnership.ownershipType.code = Enums.OwnershipTypeEnum.Freeholder;

            var ownership: Business.Ownership = new Business.Ownership(dtoOwnership);

            expect(ownership.isVendor()).toBe(false);
        });

        it('when is current and is not freeholder should not have vendors', () => {
            var dtoOwnership: Dto.IOwnership = getOwnership();
            dtoOwnership.sellDate = null;
            dtoOwnership.ownershipType.code = Enums.OwnershipTypeEnum.Leaseholder;

            var ownership: Business.Ownership = new Business.Ownership(dtoOwnership);

            expect(ownership.isVendor()).toBe(false);
        });

        it('when is current and is freeholder should have vendors', () => {
            var dtoOwnership: Dto.IOwnership = getOwnership();
            dtoOwnership.sellDate = null;
            dtoOwnership.ownershipType.code = Enums.OwnershipTypeEnum.Freeholder;

            var ownership: Business.Ownership = new Business.Ownership(dtoOwnership);

            expect(ownership.isVendor()).toBe(true);
        });
    });

    // TODO move to generator, use fakerjs
    var getOwnership = (): Dto.IOwnership => {
        var ownership: Dto.IOwnership = {
            id: 'id',
            buyPrice: 21,
            contacts: [],
            createDate: new Date(),
            ownershipType: {
                code: Enums.OwnershipTypeEnum.Leaseholder
            },
            ownershipTypeId: 'typeId',
            purchaseDate: new Date(),
            sellDate: new Date(),
            sellPrice: 12
        };

        return ownership;
    }
}