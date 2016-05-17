/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    var getAddress = (): Business.Address => {
        var address: Dto.IAddress = {
            propertyName: "West Forum",
            propertyNumber: "142A",
            line2: "Strzegomska"
        };

        return new Business.Address(address);
    }

    describe('Given address', () => {
        it('when PropertyName, PropertyNumber and AddressLine2 are set then address text should be West Forum, 142A Strzegomska', () => {
            // arrange
            var address: Business.Address = getAddress();

            // act 
            var addressText = address.getAddressText();

            // assert
            expect(addressText).toBe("West Forum, 142A Strzegomska");
        });

        it('when PropertyName and PropertyNumber are set then address text should be West Forum, 142A', () => {
            // arrange
            var address: Business.Address = getAddress();
            address.line2 = null;

            // act 
            var addressText = address.getAddressText();

            // assert
            expect(addressText).toBe("West Forum, 142A");
        });

        it('when PropertyName and AddressLine2 are set then address text should be West Forum, Strzegomska', () => {
            // arrange
            var address: Business.Address = getAddress();
            address.propertyNumber = null;

            // act 
            var addressText = address.getAddressText();

            // assert
            expect(addressText).toBe("West Forum, Strzegomska");
        });

        it('when PropertyNumber and AddressLine2 are set then address text should be 142A Strzegomska', () => {
            // arrange
            var address: Business.Address = getAddress();
            address.propertyName = null;

            // act 
            var addressText = address.getAddressText();

            // assert
            expect(addressText).toBe("142A Strzegomska");
        });

        it('when PropertyNumber is set then address text should be 142A', () => {
            // arrange
            var address: Business.Address = getAddress();
            address.propertyName = null;
            address.line2 = null;

            // act 
            var addressText = address.getAddressText();

            // assert
            expect(addressText).toBe("142A");
        });

        it('when AddressLine2 is set then address text should be Strzegomska', () => {
            // arrange
            var address: Business.Address = getAddress();
            address.propertyName = null;
            address.propertyNumber = null;

            // act 
            var addressText = address.getAddressText();

            // assert
            expect(addressText).toBe("Strzegomska");
        });

        it('when PropertyName is set, then address text should be West Forum', () => {
            // arrange
            var address: Business.Address = getAddress();
            address.line2 = null;
            address.propertyNumber = null;

            // act 
            var addressText = address.getAddressText();

            // assert
            expect(addressText).toBe("West Forum");
        });
    });    
}